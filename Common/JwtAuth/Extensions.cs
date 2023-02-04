using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PmeMemberApi
{
    public static class Extensions
    {
        public static IServiceCollection AddJWT(this IServiceCollection services, IConfiguration configuration)
        {

            var authenticationProviderKey = "JwtBearer";
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);
            services.AddSingleton(jwtSettings);
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddSingleton<IJwtHandler, JwtHandler>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = authenticationProviderKey;
                    options.DefaultChallengeScheme = authenticationProviderKey;
                })
                .AddJwtBearer(authenticationProviderKey, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtSettings.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                    };
                });
            return services;
        }

    }
}
