namespace Accounts.Application.Features.GetAccountById
{
    public record GetAccountDto(
        int Id,
        string AccountName,
        string AccountNumber,
        string? AccountOwner,
        string Phone,
        string? Fax,
        string? Website,
        string BillingAddress,
        string ShippingAddress,
        string Type,
        string? Industry,
        decimal AnnualRevenue,
        int NumberOfEmployees,
        string? Ownership,
        DateTime CreatedAtUtc,
        DateTime ModifiedAtUtc
    );
}
