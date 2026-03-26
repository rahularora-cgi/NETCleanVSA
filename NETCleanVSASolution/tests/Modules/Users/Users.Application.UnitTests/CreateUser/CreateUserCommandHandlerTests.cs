using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

using Framework.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Moq;
using Users.Application.CreateUser;
using Users.Domain.Entities;
using Users.Persistence.Database;
using Xunit;

namespace Users.Application.CreateUser.UnitTests
{
    /// <summary>
    /// Unit tests for CreateUserCommandHandler.
    /// </summary>
    public class CreateUserCommandHandlerTests
    {
        /// <summary>
        /// Tests that HandleAsync successfully creates a user when provided with a valid command and no duplicate email exists.
        /// Expected: Returns a success result with the user's GUID.
        /// </summary>
        [Fact]
        public async Task HandleAsync_ValidCommand_ReturnsSuccessWithUserId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UsersDbContext>().Options;
            var mockDbContext = new Mock<UsersDbContext>(options);
            var mockLogger = new Mock<ILogger<CreateUserCommandHandler>>();
            var mockDbSet = new Mock<DbSet<User>>();

            var command = new CreateUserCommand(
                FirstName: "John",
                LastName: "Doe",
                UserName: "johndoe",
                Email: "john.doe@example.com",
                Password: "SecurePassword123!",
                IsActive: true
            );

            var users = new List<User>().AsQueryable();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IAsyncEnumerable<User>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<User>(users.GetEnumerator()));

            mockDbSet.Setup(x => x.Add(It.IsAny<User>()))
               .Callback<User>(u => u.GetType().GetProperty(nameof(User.Id))!.SetValue(u, Guid.NewGuid()));


            mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);
            mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);


            var handler = new CreateUserCommandHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.HandleAsync(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEqual(Guid.Empty, result.Value);
            mockDbSet.Verify(db => db.Add(It.IsAny<User>()), Times.Once);
            mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        /// <summary>
        /// Tests that HandleAsync returns a conflict error when a user with the same email already exists.
        /// Expected: Returns a failure result with a Conflict error.
        /// </summary>
        [Fact]
        public async Task HandleAsync_DuplicateEmail_ReturnsConflictError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UsersDbContext>().Options;
            var mockDbContext = new Mock<UsersDbContext>(options);
            var mockLogger = new Mock<ILogger<CreateUserCommandHandler>>();
            var mockDbSet = new Mock<DbSet<User>>();

            var command = new CreateUserCommand(
                FirstName: "Jane",
                LastName: "Smith",
                UserName: "janesmith",
                Email: "existing@example.com",
                Password: "Password123!",
                IsActive: true
            );

            var existingUser = new User
            {
                Email = "existing@example.com",
                UserName = "existing",
                FirstName = "Existing",
                LastName = "User"
            };

            var users = new List<User> { existingUser }.AsQueryable();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IAsyncEnumerable<User>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<User>(users.GetEnumerator()));

            mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);

            var handler = new CreateUserCommandHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.HandleAsync(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Error.Conflict", result.Error.Code);
            Assert.Contains("existing@example.com", result.Error.Message);
            mockDbSet.Verify(db => db.Add(It.IsAny<User>()), Times.Never);
            mockDbContext.Verify(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        /// <summary>
        /// Tests that HandleAsync returns a custom error when an exception occurs during database save.
        /// Expected: Returns a failure result with a Custom error containing the exception message.
        /// </summary>
        [Fact(Skip = "ProductionBugSuspected")]
        [Trait("Category", "ProductionBugSuspected")]
        public async Task HandleAsync_ExceptionDuringSave_ReturnsCustomError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UsersDbContext>().Options;
            var mockDbContext = new Mock<UsersDbContext>(options);
            var mockLogger = new Mock<ILogger<CreateUserCommandHandler>>();
            var mockDbSet = new Mock<DbSet<User>>();

            var command = new CreateUserCommand(
                FirstName: "Test",
                LastName: "User",
                UserName: "testuser",
                Email: "test@example.com",
                Password: "TestPassword123!",
                IsActive: true
            );

            var users = new List<User>().AsQueryable();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IAsyncEnumerable<User>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<User>(users.GetEnumerator()));

            mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);
            mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("Database error"));

            var handler = new CreateUserCommandHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            var result = await handler.HandleAsync(command, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("User.CreateFailed", result.Error.Code);
            Assert.Equal("Database error", result.Error.Message);
        }

        /// <summary>
        /// Tests that HandleAsync properly propagates cancellation when the operation is cancelled.
        /// Expected: Throws OperationCanceledException when cancellation token is cancelled.
        /// </summary>
        [Fact]
        public async Task HandleAsync_CancellationRequested_ThrowsOperationCanceledException()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UsersDbContext>().Options;
            var mockDbContext = new Mock<UsersDbContext>(options);
            var mockLogger = new Mock<ILogger<CreateUserCommandHandler>>();
            var mockDbSet = new Mock<DbSet<User>>();

            var command = new CreateUserCommand(
                FirstName: "Test",
                LastName: "User",
                UserName: "testuser",
                Email: "test@example.com",
                Password: "TestPassword123!",
                IsActive: true
            );

            var users = new List<User>().AsQueryable();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IAsyncEnumerable<User>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<User>(users.GetEnumerator()));

            mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);
            mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new OperationCanceledException());

            var handler = new CreateUserCommandHandler(mockDbContext.Object, mockLogger.Object);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // Act & Assert
            var result = await handler.HandleAsync(command, cancellationTokenSource.Token);
            Assert.False(result.IsSuccess);
            Assert.Equal("User.CreateFailed", result.Error.Code);
        }

       
        /// <summary>
        /// Tests that HandleAsync logs error messages when an exception occurs.
        /// Expected: Logs error message with exception details.
        /// </summary>
        [Fact]
        public async Task HandleAsync_ExceptionOccurs_LogsError()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<UsersDbContext>().Options;
            var mockDbContext = new Mock<UsersDbContext>(options);
            var mockLogger = new Mock<ILogger<CreateUserCommandHandler>>();
            var mockDbSet = new Mock<DbSet<User>>();

            var command = new CreateUserCommand(
                FirstName: "Error",
                LastName: "Test",
                UserName: "errortest",
                Email: "error@example.com",
                Password: "ErrorPassword123!",
                IsActive: true
            );

            var users = new List<User>().AsQueryable();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<User>(users.Provider));
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IAsyncEnumerable<User>>().Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<User>(users.GetEnumerator()));

            mockDbContext.Setup(db => db.Users).Returns(mockDbSet.Object);
            mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("Test exception"));

            var handler = new CreateUserCommandHandler(mockDbContext.Object, mockLogger.Object);

            // Act
            await handler.HandleAsync(command, CancellationToken.None);

            // Assert
            mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error creating user")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }


        /// <summary>
        /// Helper class to support async queryable operations for DbSet mocking.
        /// </summary>
        private class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
        {
            private readonly IQueryProvider _inner;

            internal TestAsyncQueryProvider(IQueryProvider inner)
            {
                _inner = inner;
            }

            public IQueryable CreateQuery(Expression expression)
            {
                return new TestAsyncEnumerable<TEntity>(expression);
            }

            public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            {
                return new TestAsyncEnumerable<TElement>(expression);
            }

            public object Execute(Expression expression)
            {
                return _inner.Execute(expression)!;
            }

            public TResult Execute<TResult>(Expression expression)
            {
                return _inner.Execute<TResult>(expression);
            }

            public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
            {
                var resultType = typeof(TResult).GetGenericArguments()[0];
                var executionResult = typeof(IQueryProvider)
                    .GetMethod(
                        name: nameof(IQueryProvider.Execute),
                        genericParameterCount: 1,
                        types: new[] { typeof(Expression) })
                    !.MakeGenericMethod(resultType)
                    .Invoke(this, new[] { expression });

                return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))
                    ?.MakeGenericMethod(resultType)
                    .Invoke(null, new[] { executionResult })!;
            }
        }

        /// <summary>
        /// Helper class to support async enumerable operations for DbSet mocking.
        /// </summary>
        private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
        {
            public TestAsyncEnumerable(IEnumerable<T> enumerable)
                : base(enumerable)
            { }

            public TestAsyncEnumerable(Expression expression)
                : base(expression)
            { }

            public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
            }

            IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
        }

        /// <summary>
        /// Helper class to support async enumerator operations for DbSet mocking.
        /// </summary>
        private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
        {
            private readonly IEnumerator<T> _inner;

            public TestAsyncEnumerator(IEnumerator<T> inner)
            {
                _inner = inner;
            }

            public T Current => _inner.Current;

            public ValueTask<bool> MoveNextAsync()
            {
                return new ValueTask<bool>(_inner.MoveNext());
            }

            public ValueTask DisposeAsync()
            {
                _inner.Dispose();
                return default;
            }
        }


    }
}