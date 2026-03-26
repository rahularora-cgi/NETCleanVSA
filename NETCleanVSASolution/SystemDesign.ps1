#Accounts

dotnet new classlib -n Accounts.Domain -o src/Modules/Accounts/Accounts.Domain --framework net10.0
#dotnet sln add src/Modules/Accounts/Accounts.Domain/Accounts.Domain.csproj

dotnet new classlib -n Accounts.Application -o src/Modules/Accounts/Accounts.Application --framework net10.0
#dotnet sln add src/Modules/Accounts/Accounts.Application/Accounts.Application.csproj

dotnet new classlib -n Accounts.Persistence -o src/Modules/Accounts/Accounts.Persistence	 --framework net10.0
#dotnet sln add src/Modules/Accounts/Accounts.Persistence/Accounts.Persistence.csproj

dotnet new classlib -n Accounts.Endpoint -o src/Modules/Accounts/Accounts.Endpoint --framework net10.0
#dotnet sln add src/Modules/Accounts/Accounts.Endpoint/Accounts.Endpoint.csproj

dotnet sln add src/Modules/Accounts/Accounts.Domain/Accounts.Domain.csproj src/Modules/Accounts/Accounts.Application/Accounts.Application.csproj src/Modules/Accounts/Accounts.Persistence/Accounts.Persistence.csproj src/Modules/Accounts/Accounts.Endpoint/Accounts.Endpoint.csproj

#Contacts

dotnet new classlib -n Contacts.Domain -o src/Modules/Contacts/Contacts.Domain --framework net10.0
#dotnet sln add src/Modules/Contacts/Contacts.Domain/Contacts.Domain.csproj

dotnet new classlib -n Contacts.Application -o src/Modules/Contacts/Contacts.Application --framework net10.0
#dotnet sln add src/Modules/Contacts/Contacts.Application/Contacts.Application.csproj

dotnet new classlib -n Contacts.Persistence -o src/Modules/Contacts/Contacts.Persistence --framework net10.0
#dotnet sln add src/Modules/Contacts/Contacts.Persistence/Contacts.Persistence.csproj

dotnet new classlib -n Contacts.Endpoint -o src/Modules/Contacts/Contacts.Endpoint --framework net10.0
#dotnet sln add src/Modules/Contacts/Contacts.Endpoint/Contacts.Endpoint.csproj

dotnet sln add src/Modules/Contacts/Contacts.Domain/Contacts.Domain.csproj src/Modules/Contacts/Contacts.Application/Contacts.Application.csproj src/Modules/Contacts/Contacts.Persistence/Contacts.Persistence.csproj src/Modules/Contacts/Contacts.Endpoint/Contacts.Endpoint.csproj

#Leads

dotnet new classlib -n Leads.Domain -o src/Modules/Leads/Leads.Domain --framework net10.0
#dotnet sln add src/Modules/Leads/Leads.Domain/Leads.Domain.csproj

dotnet new classlib -n Leads.Application -o src/Modules/Leads/Leads.Application --framework net10.0
#dotnet sln add src/Modules/Leads/Leads.Application/Leads.Application.csproj

dotnet new classlib -n Leads.Persistence -o src/Modules/Leads/Leads.Persistence --framework net10.0
#dotnet sln add src/Modules/Leads/Leads.Persistence/Leads.Persistence.csproj

dotnet new classlib -n Leads.Endpoint -o src/Modules/Leads/Leads.Endpoint --framework net10.0
#dotnet sln add src/Modules/Leads/Leads.Endpoint/Leads.Endpoint.csproj

dotnet sln add src/Modules/Leads/Leads.Domain/Leads.Domain.csproj src/Modules/Leads/Leads.Application/Leads.Application.csproj src/Modules/Leads/Leads.Persistence/Leads.Persistence.csproj src/Modules/Leads/Leads.Endpoint/Leads.Endpoint.csproj

#Opportunities

dotnet new classlib -n Opportunities.Domain -o src/Modules/Opportunities/Opportunities.Domain --framework net10.0
#dotnet sln add src/Modules/Opportunities/Opportunities.Domain/Opportunities.Domain.csproj

dotnet new classlib -n Opportunities.Application -o src/Modules/Opportunities/Opportunities.Application --framework net10.0
#dotnet sln add src/Modules/Opportunities/Opportunities.Application/Opportunities.Application.csproj

dotnet new classlib -n Opportunities.Persistence -o src/Modules/Opportunities/Opportunities.Persistence --framework net10.0
#dotnet sln add src/Modules/Opportunities/Opportunities.Persistence/Opportunities.Persistence.csproj

dotnet new classlib -n Opportunities.Endpoint -o src/Modules/Opportunities/Opportunities.Endpoint --framework net10.0
#dotnet sln add src/Modules/Opportunities/Opportunities.Endpoint/Opportunities.Endpoint.csproj

dotnet sln add src/Modules/Opportunities/Opportunities.Domain/Opportunities.Domain.csproj src/Modules/Opportunities/Opportunities.Application/Opportunities.Application.csproj src/Modules/Opportunities/Opportunities.Persistence/Opportunities.Persistence.csproj src/Modules/Opportunities/Opportunities.Endpoint/Opportunities.Endpoint.csproj

#Users

dotnet new classlib -n Users.Domain -o src/Modules/Users/Users.Domain --framework net10.0

dotnet new classlib -n Users.Application -o src/Modules/Users/Users.Application --framework net10.0

dotnet new classlib -n Users.Infrastructure -o src/Modules/Users/Users.Infrastructure --framework net10.0

dotnet new classlib -n Users.Persistence -o src/Modules/Users/Users.Persistence --framework net10.0

dotnet new classlib -n Users.Endpoint -o src/Modules/Users/Users.Endpoint --framework net10.0

dotnet sln add src/Modules/Users/Users.Domain/Users.Domain.csproj src/Modules/Users/Users.Application/Users.Application.csproj src/Modules/Users/Users.Infrastructure/Users.Infrastructure.csproj src/Modules/Users/Users.Persistence/Users.Persistence.csproj src/Modules/Users/Users.Endpoint/Users.Endpoint.csproj
#eof

