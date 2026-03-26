namespace Users.Application.QueryUser
{
    public record GetUserDto(Guid Id, string FirstName, string LastName, string UserName, string Email, string PasswordHash, bool IsActive, DateTime CreatedAtUtc, DateTime? ModifiedAtUtc);

}
