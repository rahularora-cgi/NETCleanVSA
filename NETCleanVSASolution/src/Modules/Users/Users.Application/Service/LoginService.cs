namespace Users.Application.Service
{
    public class LoginService(IQueryDispatcher _queryDispatcher, IJwtTokenProvider _jwtTokenProvider) : ILoginService
    {
        public async Task<Result<string>> HandleAsync(LoginRequestDto loginRequest)
        {
            //var userDto = await _queryDispatcher.QueryAsync<GetUserDto>(new GetUserByEmailQuery(loginRequest.Email));
            var userDto = await _queryDispatcher.QueryAsync<GetUserByEmailQuery, GetUserDto>(new GetUserByEmailQuery(loginRequest.Email));

            if (userDto.IsFailure)
            {
                return Result.Failure<string>(Error.NotFound("User", loginRequest.Email));
            }

            var userEntity = userDto.Value.ToEntity<User>();

            bool IsPasswordVerified = VerifyPassword(userEntity, loginRequest.Password, userDto.Value.Email);

            if (!IsPasswordVerified)
            {
                return Result.Failure<string>(Error.Unauthorized("Invalid password"));
            }

            var token = await _jwtTokenProvider.GenerateJwtToken(userEntity, GetUserRoles);

            return Result<string>.Success(token);
        }

        public async Task<IEnumerable<Role>> GetUserRoles(User user)
        {
            //var roles = await _queryDispatcher.QueryAsync<IEnumerable<GetRoleDto>>(
            //new GetRolesByUserIdQuery(user.Id));

            var roles = await _queryDispatcher.QueryAsync<GetRolesByUserIdQuery, IEnumerable<GetRoleDto>>(
                new GetRolesByUserIdQuery(user.Id));

            // Materialize as a list of Role
            return roles.Value
                .Select(r => new Role { Id = r.Id, RoleName = r.Name });
        }

        private bool VerifyPassword(User user, string password, string email)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            PasswordVerificationResult result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}
