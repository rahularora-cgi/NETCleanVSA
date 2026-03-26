namespace Users.Application.Service
{
    public interface ILoginService
    {
        Task<IEnumerable<Role>> GetUserRoles(User user);
        Task<Result<string>> HandleAsync(LoginRequestDto loginRequest);
    }
}