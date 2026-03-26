namespace Accounts.Application.Features.CreateAccount
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            RuleFor(x => x.AccountName)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.");
            
            RuleFor(x => x.AccountOwner)
                .NotEmpty()
                .WithMessage("'{PropertyName}' is required.");

            //if AccountAddress is child object of Account and has it's own validator then use like following
            //RuleFor(x => x.AccountAddress)
            //    .NotNull()
            //    .WithMessage(x=>$"'{nameof(x.AccountAddress)}' is required.")
            //    .SetValidator(new AnotherValidator());
        }
    }
}
