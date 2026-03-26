namespace Users.Infrastructure.Authentication.JWT
{
    public static class JwtAuthExtensions
    {
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();


            //JwtAuthSettings jwtAuthSettings = new();
            //configuration.GetSection(nameof(JwtAuthSettings)).Bind(jwtAuthSettings);
            //services.AddSingleton<JwtAuthSettings>(jwtAuthSettings);

            //options pattern for reading these values
            //services.Configure<JwtAuthSettings>(configuration.GetSection(nameof(JwtAuthSettings)));
            services.AddOptions<JwtAuthSettings>().Bind(configuration.GetSection(nameof(JwtAuthSettings)));

            // Configure JWT authentication

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["JwtAuthSettings:Issuer"];
                    options.Audience = configuration["JwtAuthSettings:Audience"];
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["JwtAuthSettings:Issuer"], //jwtAuthSettings.Issuer,
                        ValidAudience = configuration["JwtAuthSettings:Audience"], //jwtAuthSettings.Audience,
                        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtAuthSettings:Key"]!))

                    };
                });


            return services;
        }
    }
}
