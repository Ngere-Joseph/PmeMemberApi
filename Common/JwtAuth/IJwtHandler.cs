using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;

namespace PmeMemberApi
{
    public interface IJwtHandler
    {
        JsonWebToken Create(long userId, string username, string fullName, List<Claim> roleClaims, string email);

    }
}
