using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PmeMemberApi.Core.IDao;
using PmeMemberApi.Core.Model;
using PmeMemberApi.SecureAuth;

namespace PmeMemberApi.Core.Dao
{
    public class AuthDao : IAuthDao
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IJwtHandler _jwtHandler;

        public AuthDao(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager, IJwtHandler jwtHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtHandler = jwtHandler;
        }
        public async Task<JsonWebToken?> Login(Login request)
        {
            var claims = new List<Claim>();
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return null;
            }

            SignInResult result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    //BlockUser
                    user.ProfileStatus = ProfileStatus.Blocked;
                    await _userManager.UpdateAsync(user);
                    return null;
                }
                else
                {
                    int maxAttempts = _userManager.Options.Lockout.MaxFailedAccessAttempts;
                    int failedAttempts = await _userManager.GetAccessFailedCountAsync(user);
                    return null;
                }
            }

            var roleNames = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.Where(x => roleNames.Contains(x.Name)).ToList();
            foreach (var role in roles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                claims.AddRange(roleClaims);
            }

            var tokenResult = _jwtHandler.Create(user.Id, user.UserName, $"{user.FirstName} {user.LastName}", claims,
                user.Email);

            return tokenResult;
        }

        public async Task<AuthActivityFeedback> Register(UserRegistration userDetails)
        {
            var userExists = await _userManager.FindByNameAsync(userDetails.Username);
            if (userExists != null)
                return  new AuthActivityFeedback { Status = "Error", Message = "User already exists!" };

            AppUser user = new()
            {
                Email = userDetails.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = userDetails.Username,
                FirstName = userDetails.FirstName,
                LastName = userDetails.LastName,
                Dob = userDetails.Dob,
                Gender = userDetails.Gender
            };
            var result = await _userManager.CreateAsync(user, userDetails.Password);
            return !result.Succeeded ? new AuthActivityFeedback { Status = "Error", Message = "User creation failed! Please check user details and try again." } : new AuthActivityFeedback() {Status = "Success", Message = "User created successfully!"};
        }
    }
}
