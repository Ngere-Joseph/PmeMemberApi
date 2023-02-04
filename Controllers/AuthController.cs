using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PmeMemberApi.Core.IDao;
using PmeMemberApi.Core.Model;
using PmeMemberApi.SecureAuth;


namespace PmeMemberApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IAuthDao _authDao;

        public AuthController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IConfiguration configuration, IAuthDao authDao)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _authDao = authDao;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var tokenResult = await _authDao.Login(model);
            if (tokenResult == null)
            {
                return Unauthorized();
            }

            return Ok(tokenResult);

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistration model)
        {
            return Ok(await _authDao.Register(model));
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] UserRegistration model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthActivityFeedback { Status = "Error", Message = "User already exists!" });

            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new AuthActivityFeedback { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            /*if (!await _roleManager.RoleExistsAsync(AppUserRole.Admin))
                await _roleManager.CreateAsync(new AppRole(AppUserRole.Admin));
            if (!await _roleManager.RoleExistsAsync(AppUserRole.User))
                await _roleManager.CreateAsync(new IdentityRole(AppUserRole.User));*/

            if (await _roleManager.RoleExistsAsync(AppUserRole.Admin))
            {
                await _userManager.AddToRoleAsync(user, AppUserRole.Admin);
            }
            if (await _roleManager.RoleExistsAsync(AppUserRole.Admin))
            {
                await _userManager.AddToRoleAsync(user, AppUserRole.User);
            }
            return Ok(new AuthActivityFeedback { Status = "Success", Message = "User created successfully!" });
        }
    }
}

