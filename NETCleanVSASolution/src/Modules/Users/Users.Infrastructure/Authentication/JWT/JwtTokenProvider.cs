namespace Users.Infrastructure.Authentication.JWT
{
    public class JwtTokenProvider(IOptions<JwtAuthSettings> _jwtAuthSettingsOptions) : IJwtTokenProvider
    {
        private  JwtAuthSettings _jwtAuthSettings => _jwtAuthSettingsOptions.Value;
        public async Task<string> GenerateJwtToken<T>(User user, Func<User, Task<T>> GetRoles)
        {
            //add a delegate to get user roles if needed

            var roles = await GetRoles(user) as IEnumerable<Role> ;
            var claimsForRoles = roles?.Select(role => new Claim(ClaimTypes.Role, role.RoleName));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            claims.AddRange(claimsForRoles);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtAuthSettings.Issuer,
                Audience = _jwtAuthSettings.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtAuthSettings.ExpirationInMinutes),
                SigningCredentials = credentials
            };

            var tokenHandler = new JsonWebTokenHandler();
            string token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

    }
}
