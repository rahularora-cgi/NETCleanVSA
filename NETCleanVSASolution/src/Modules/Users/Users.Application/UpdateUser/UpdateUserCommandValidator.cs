namespace Users.Application.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("User ID is required");

            When(x => !string.IsNullOrWhiteSpace(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                    .MaximumLength(50).WithMessage("First name must not exceed 50 characters")
                    .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("First name can only contain letters, spaces, hyphens, and apostrophes");
            });

            When(x => !string.IsNullOrWhiteSpace(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                    .MaximumLength(50).WithMessage("Last name must not exceed 50 characters")
                    .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Last name can only contain letters, spaces, hyphens, and apostrophes");
            });

            When(x => !string.IsNullOrWhiteSpace(x.UserName), () =>
            {
                RuleFor(x => x.UserName)
                    .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                    .MaximumLength(50).WithMessage("Username must not exceed 50 characters");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Email), () =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress().WithMessage("Invalid email format")
                    .MaximumLength(100).WithMessage("Email must not exceed 100 characters");
            });

            When(x => !string.IsNullOrWhiteSpace(x.Password), () =>
            {
                RuleFor(x => x.Password)
                    .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                    .MaximumLength(100).WithMessage("Password must not exceed 100 characters")
                    .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                    .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter")
                    .Matches(@"[0-9]").WithMessage("Password must contain at least one number")
                    .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");
            });

            // At least one field must be provided for update
            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.FirstName) ||
                          !string.IsNullOrWhiteSpace(x.LastName) ||
                          !string.IsNullOrWhiteSpace(x.UserName) ||
                          !string.IsNullOrWhiteSpace(x.Email) ||
                          !string.IsNullOrWhiteSpace(x.Password))
                .WithMessage("At least one field (FirstName, LastName, UserName, Email, or Password) must be provided for update");
        }
    }
}