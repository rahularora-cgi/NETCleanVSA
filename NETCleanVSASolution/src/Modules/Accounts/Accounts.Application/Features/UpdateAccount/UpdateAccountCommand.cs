using Framework.Application.Abstractions.CQRS;

namespace Accounts.Application.Features.UpdateAccount
{
    public record UpdateAccountCommand(
        int Id,
        string? AccountName,
        string? AccountNumber,
        string? AccountOwner,
        string? Phone,
        string? Fax,
        string? Website,
        string? BillingAddress,
        string? ShippingAddress,
        string? Type,
        string? Industry,
        decimal? AnnualRevenue,
        int? NumberOfEmployees,
        string? Ownership
    ) : ICommand<Unit>;
}