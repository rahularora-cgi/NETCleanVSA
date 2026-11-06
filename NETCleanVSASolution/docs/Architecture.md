Clean + VSA architecture


             ┌───────────────────┐
             │ Accounts.Endpoint │
             └────────┬──────────┘
                      │
                 ┌────┴─────┐
                 ▼          ▼
        ┌─────────────┐   ┌─────────────────┐
        │ Application │◄──│   Persistence   │
        └──────┬──────┘   └────────┬────────┘
               │                   │
               └─────────┬─────────┘
                         ▼
                 ┌────────────────┐
                 │     Domain     │
                 └────────────────┘


| Project | Can reference |
|---|---|
| Domain | Nothing |
| Application | Domain |
| Persistence | Application + Domain |
| Endpoint | Application + Persistence |


The key architectural constraint is simply:

Never add a reference from Accounts.Application to Accounts.Persistence.

With that rule maintained, your CreateAccountCommandHandler can depend on IAccountsDbContext, the DI container supplies AccountsDbContext, and your vertical slices remain independent of the concrete persistence implementation.
