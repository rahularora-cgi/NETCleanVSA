namespace Accounts.Application.Features.GetAllAccounts
{
    public sealed class GetAccountListDto
    {
        public IEnumerable<GetAccountListItemDto> Accounts { get; set; } = Enumerable.Empty<GetAccountListItemDto>();
    }

    public sealed record GetAccountListItemDto(
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
