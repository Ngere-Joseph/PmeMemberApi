using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PmeMemberApi.Common.Auth;
using PmeMemberApi.Core.Model;
using PmeMemberApi.SecureAuth;

namespace PmeMemberApi.Core
{
    public static class Extensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

            // services.AddAuthentication();

            services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddRoles<AppRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthorization(options =>
            {
                foreach (RolePermission permission in Permissions.AllPermissions)
                {
                    options.AddPolicy(permission.Id, builder => { builder.AddRequirements(new PermissionRequirement(permission.Id)); });
                }

                // add this to any controller that belongs to the Admin
                options.AddPolicy(Policy.Administrator, o => o.RequireRole(Roles.Administrator));
                // add  this to user api endpoint
                options.AddPolicy(Policy.Users, o => o.RequireRole(Roles.Users));
            });


            var jwtSettings = new JwtSettings
            {
                Secret = Configuration["JwtSettings:Secret"],
                ExpiryMinutes = int.Parse(Configuration["JwtSettings:ExpiryMinutes"]),
                Issuer = Configuration["JwtSettings:Issuer"]
            };

            services.AddSingleton<JwtSettings>(jwtSettings);

            return services;
        }

    }
}
