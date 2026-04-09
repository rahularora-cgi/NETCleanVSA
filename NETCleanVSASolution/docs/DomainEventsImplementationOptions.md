# Domain Events Implementation Options

## Overview
There are two primary patterns for handling domain events in your application:

---

## Option 1: Outbox Pattern (Current Implementation) ✅

### How it Works
1. Domain events are serialized and stored in the `OutboxMessages` database table
2. Events are persisted in the same transaction as your business data (transactional consistency)
3. A background worker processes events from the outbox table asynchronously
4. Events can be retried on failure

### Pros
- ✅ **Guaranteed delivery** - Events survive application crashes
- ✅ **Transactional consistency** - Events saved with domain changes
- ✅ **Resilience** - Failed events can be retried
- ✅ **Audit trail** - Complete history of all events
- ✅ **No external dependencies** - Just uses your database
- ✅ **Best for distributed systems** and microservices

### Cons
- ❌ **Eventual consistency** - Slight delay in processing
- ❌ **Requires background worker** - Need to implement processor
- ❌ **Database overhead** - Additional table writes

### When to Use
- Mission-critical events that cannot be lost
- Distributed systems / microservices architecture
- When you need event replay capability
- When you need guaranteed delivery

### Current Implementation
```csharp
private async Task PublishDomainEvents(CancellationToken cancellationToken)
{
    var aggregateRoots = ChangeTracker
        .Entries<AggregateRoot<object>>()
        .Where(entry => entry.Entity.DomainEvents.Any())
        .Select(entry => entry.Entity)
        .ToList();

    var domainEvents = aggregateRoots
        .SelectMany(aggregateRoot => aggregateRoot.DomainEvents)
        .ToList();

    aggregateRoots.ForEach(aggregateRoot => aggregateRoot.ClearDomainEvents());

    foreach (var domainEvent in domainEvents)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredAtUtc = DateTime.UtcNow,
            Type = domainEvent.GetType().Name,
            Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType())
        };

        await OutBoxMessages.AddAsync(outboxMessage, cancellationToken);
    }
}
```

### Required: Background Processor
You'll need to create a background service to process outbox messages:

```csharp
public class OutboxMessageProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OutboxMessageProcessor> _logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<YourDbContext>();
            
            var messages = await dbContext.OutBoxMessages
                .Where(m => m.ProcessedAtUtc == null)
                .Take(20)
                .ToListAsync(stoppingToken);

            foreach (var message in messages)
            {
                try
                {
                    // Deserialize and process event
                    // Call appropriate consumer (e.g., UserCreatedEventConsumer)
                    
                    message.ProcessedAtUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    message.Error = ex.Message;
                }
            }
            
            await dbContext.SaveChangesAsync(stoppingToken);
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
```

---

## Option 2: In-Memory Pattern with MediatR

### How it Works
1. Domain events are published immediately using MediatR
2. Event handlers execute synchronously within the same database transaction
3. Events are handled in-memory before `SaveChanges` commits
4. No persistence of events (unless you add it separately)

### Pros
- ✅ **Immediate consistency** - Events processed instantly
- ✅ **Simpler** - No background worker needed
- ✅ **Lower latency** - Instant event handling
- ✅ **Good for monoliths** - Single process handling

### Cons
- ❌ **No event history** - Events lost after processing
- ❌ **Transaction risk** - Handler failure rolls back entire transaction
- ❌ **Performance** - Long-running handlers slow down SaveChanges
- ❌ **Lost on crash** - Events disappear if app crashes before commit

### When to Use
- Simple monolithic applications
- Non-critical events
- When immediate consistency is required
- When you don't need event audit trail

### Implementation Steps

#### Step 1: Install MediatR
```bash
dotnet add package MediatR
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
```

#### Step 2: Make Domain Events Implement INotification
```csharp
// Update IDomainEvent interface
public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccurredAtUtc { get; }
}
```

#### Step 3: Update DbContextBase Constructor
```csharp
public abstract class DbContextBase : DbContext, IDbContextBase
{
    private readonly IMediator _mediator;

    protected DbContextBase(DbContextOptions options, IMediator mediator) 
        : base(options)
    {
        _mediator = mediator;
    }

    // ... rest of code
}
```

#### Step 4: Update PublishDomainEvents
```csharp
private async Task PublishDomainEvents(CancellationToken cancellationToken)
{
    var aggregateRoots = ChangeTracker
        .Entries<AggregateRoot<object>>()
        .Where(entry => entry.Entity.DomainEvents.Any())
        .Select(entry => entry.Entity)
        .ToList();

    var domainEvents = aggregateRoots
        .SelectMany(aggregateRoot => aggregateRoot.DomainEvents)
        .ToList();

    aggregateRoots.ForEach(aggregateRoot => aggregateRoot.ClearDomainEvents());

    // Publish all events in parallel
    var tasks = domainEvents
        .Select(domainEvent => _mediator.Publish(domainEvent, cancellationToken));
    
    await Task.WhenAll(tasks);
}
```

#### Step 5: Create Event Handlers
```csharp
public class UserCreatedDomainEventHandler 
    : INotificationHandler<UserCreatedDomainEvent>
{
    private readonly INotificationService _emailService;
    private readonly ILogger<UserCreatedDomainEventHandler> _logger;

    public UserCreatedDomainEventHandler(
        INotificationService emailService,
        ILogger<UserCreatedDomainEventHandler> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    public async Task Handle(
        UserCreatedDomainEvent notification, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling UserCreatedDomainEvent for user {Email}", 
            notification.User.Email);

        await _emailService.SendEmailAsync(
            notification.User.Email,
            "Welcome!",
            $"Welcome {notification.User.FirstName}!",
            cancellationToken);
    }
}
```

#### Step 6: Register MediatR
```csharp
// In ServiceRegistration
services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(YourAssemblyMarker).Assembly);
});
```

---

## Option 3: Hybrid Approach (Best of Both Worlds)

### How it Works
1. Use MediatR for immediate, non-critical events (e.g., logging, caching)
2. Use Outbox for critical events (e.g., emails, external notifications)

### Implementation
```csharp
private async Task PublishDomainEvents(CancellationToken cancellationToken)
{
    var aggregateRoots = ChangeTracker
        .Entries<AggregateRoot<object>>()
        .Where(entry => entry.Entity.DomainEvents.Any())
        .Select(entry => entry.Entity)
        .ToList();

    var domainEvents = aggregateRoots
        .SelectMany(aggregateRoot => aggregateRoot.DomainEvents)
        .ToList();

    aggregateRoots.ForEach(aggregateRoot => aggregateRoot.ClearDomainEvents());

    // Separate events by criticality
    var criticalEvents = domainEvents.Where(e => e is ICriticalEvent).ToList();
    var normalEvents = domainEvents.Except(criticalEvents).ToList();

    // Immediate handling for non-critical events
    if (normalEvents.Any() && _mediator != null)
    {
        var tasks = normalEvents
            .Select(e => _mediator.Publish(e, cancellationToken));
        await Task.WhenAll(tasks);
    }

    // Outbox for critical events
    foreach (var criticalEvent in criticalEvents)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            OccurredAtUtc = DateTime.UtcNow,
            Type = criticalEvent.GetType().Name,
            Content = JsonSerializer.Serialize(criticalEvent, criticalEvent.GetType())
        };

        await OutBoxMessages.AddAsync(outboxMessage, cancellationToken);
    }
}
```

---

## Recommendation

For your CRM system, I recommend **Option 1 (Outbox Pattern)** because:

1. ✅ User notifications (emails) should be guaranteed and not lost
2. ✅ You're already using RabbitMQ (EventPublisher), showing distributed architecture
3. ✅ Outbox provides audit trail for compliance
4. ✅ Events can be replayed if needed
5. ✅ Better for future microservices migration

### Next Steps for Outbox Pattern:
1. ✅ Current implementation is correct
2. ⬜ Create OutboxMessageProcessor background service
3. ⬜ Register processor in ServiceRegistration
4. ⬜ Create event router to map event types to consumers
5. ⬜ Add monitoring and alerting for failed events

---

## Comparison Table

| Feature | Outbox Pattern | MediatR In-Memory | Hybrid |
|---------|---------------|-------------------|---------|
| Guaranteed Delivery | ✅ Yes | ❌ No | ✅ For critical |
| Event History | ✅ Yes | ❌ No | ✅ For critical |
| Immediate Execution | ❌ No | ✅ Yes | ⚠️ Mixed |
| Transaction Safety | ✅ Safe | ⚠️ Risky | ⚠️ Mixed |
| Performance | ⚠️ Slower write | ✅ Fast | ⚠️ Mixed |
| Complexity | ⚠️ Higher | ✅ Lower | ⚠️ Highest |
| Best For | Enterprise/Distributed | Monolith/Simple | Complex requirements |

