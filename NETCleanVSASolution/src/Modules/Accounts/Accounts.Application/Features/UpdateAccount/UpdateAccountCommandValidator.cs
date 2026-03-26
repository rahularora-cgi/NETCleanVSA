namespace Accounts.Application.Features.UpdateAccount
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Account ID must be greater than 0");

            When(x => !string.IsNullOrWhiteSpace(x.AccountName), () =>
            {
                RuleFor(x => x.AccountName)
                    .MaximumLength(200).WithMessage("Account name must not exceed 200 characters");
            });

            When(x => !string.IsNullOrWhiteSpace(x.AccountNumber), () =>
            {
                RuleFor(x => x.AccountNumber)
                    .MaximumLength(50).WithMessage("Account number must not exceed 50 characters")
                    .Matches(@"^[A-Z0-9\-]+$").WithMessage("Account number can only contain uppercase letters, numbers, and hyphens");
            });

            When(x => !string.IsNullOrWhiteSpace(x.AccountOwner), () =>
            {
                RuleFor(x => x.AccountOwner)
                    .MaximumLength(200).WithMessage("Account owner must not exceed 200 characters");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
            {
                RuleFor(x => x.Phone)
                    .MaximumLength(20).WithMessage("Phone must not exceed 20 characters")
                    .Matches(@"^\+?[\d\s\-\(\)]+$").WithMessage("Phone number format is invalid");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Fax), () =>
            {
                RuleFor(x => x.Fax)
                    .MaximumLength(20).WithMessage("Fax must not exceed 20 characters")
                    .Matches(@"^\+?[\d\s\-\(\)]+$").WithMessage("Fax number format is invalid");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Website), () =>
            {
                RuleFor(x => x.Website)
                    .MaximumLength(500).WithMessage("Website must not exceed 500 characters")
                    .Matches(@"^https?://.*").WithMessage("Website must be a valid URL starting with http:// or https://");
            });

            When(x => !string.IsNullOrWhiteSpace(x.BillingAddress), () =>
            {
                RuleFor(x => x.BillingAddress)
                    .MaximumLength(1000).WithMessage("Billing address must not exceed 1000 characters");
            });

            When(x => !string.IsNullOrWhiteSpace(x.ShippingAddress), () =>
            {
                RuleFor(x => x.ShippingAddress)
                    .MaximumLength(1000).WithMessage("Shipping address must not exceed 1000 characters");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Type), () =>
            {
                RuleFor(x => x.Type)
                    .MaximumLength(50).WithMessage("Account type must not exceed 50 characters")
                    .Must(type => new[] { "Customer", "Partner", "Vendor", "Competitor", "Other" }.Contains(type))
                    .WithMessage("Account type must be one of: Customer, Partner, Vendor, Competitor, Other");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Industry), () =>
            {
                RuleFor(x => x.Industry)
                    .MaximumLength(200).WithMessage("Industry must not exceed 200 characters");
            });

            When(x => x.AnnualRevenue.HasValue, () =>
            {
                RuleFor(x => x.AnnualRevenue)
                    .GreaterThanOrEqualTo(0).WithMessage("Annual revenue must be greater than or equal to 0");
            });

            When(x => x.NumberOfEmployees.HasValue, () =>
            {
                RuleFor(x => x.NumberOfEmployees)
                    .GreaterThanOrEqualTo(0).WithMessage("Number of employees must be greater than or equal to 0");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Ownership), () =>
            {
                RuleFor(x => x.Ownership)
                    .MaximumLength(50).WithMessage("Ownership must not exceed 50 characters")
                    .Must(ownership => new[] { "Public", "Private", "Subsidiary", "Other" }.Contains(ownership))
                    .WithMessage("Ownership must be one of: Public, Private, Subsidiary, Other");
            });

            // At least one field must be provided for update
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.AccountName) ||
                          !string.IsNullOrWhiteSpace(x.AccountNumber) ||
                          x.AccountOwner != null ||
                          !string.IsNullOrWhiteSpace(x.Phone) ||
                          x.Fax != null ||
                          x.Website != null ||
                          !string.IsNullOrWhiteSpace(x.BillingAddress) ||
                          !string.IsNullOrWhiteSpace(x.ShippingAddress) ||
                          !string.IsNullOrWhiteSpace(x.Type) ||
                          x.Industry != null ||
                          x.AnnualRevenue.HasValue ||
                          x.NumberOfEmployees.HasValue ||
                          x.Ownership != null)
                .WithMessage("At least one field must be provided for update");
        }
    }
}