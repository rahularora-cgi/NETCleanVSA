//Accounts

dotnet new classlib -n Accounts.Domain -o src/Modules/Accounts/Accounts.Domain --framework net10.0
dotnet sln add src/Modules/Accounts/Accounts.Domain/Accounts.Domain.csproj

dotnet new classlib -n Accounts.Application -o src/Modules/Accounts/Accounts.Application --framework net10.0
dotnet sln add src/Modules/Accounts/Accounts.Application/Accounts.Application.csproj

dotnet new classlib -n Accounts.Persistence -o src/Modules/Accounts/Accounts.Persistence --framework net10.0
dotnet sln add src/Modules/Accounts/Accounts.Persistence/Accounts.Persistence.csproj

dotnet new classlib -n Accounts.Endpoint -o src/Modules/Accounts/Accounts.Endpoint --framework net10.0
dotnet sln add src/Modules/Accounts/Accounts.Endpoint/Accounts.Endpoint.csproj

//Contacts

dotnet new classlib -n Contacts.Domain -o src/Modules/Contacts/Contacts.Domain --framework net10.0
dotnet sln add src/Modules/Contacts/Contacts.Domain/Contacts.Domain.csproj

