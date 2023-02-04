using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace PmeMemberApi
{
    public class JwtHandler : IJwtHandler
    {
        private readonly JwtSettings _options;
        private readonly SymmetricSecurityKey _issuerSigningKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtHeader _jwtHeader;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();


        public JwtHandler(IOptions<JwtSettings> options)
        {
            _options = options.Value;
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret));
            _signingCredentials = new SigningCredentials(_issuerSigningKey, SecurityAlgorithms.HmacSha256);
            _jwtHeader = new JwtHeader(_signingCredentials);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret))
            };
        }
        public JsonWebToken Create(long userId, string username, string fullName, List<Claim> roleClaims, string email)
        {
            var nowUtc = DateTime.UtcNow;
            var expires = nowUtc.AddMinutes(_options.ExpiryMinutes);
            var centuryBegins = new DateTime(1970, 1, 1).ToUniversalTime();
            var exp = (long)new TimeSpan(expires.Ticks - centuryBegins.Ticks).TotalSeconds;
            var now = (long)new TimeSpan(nowUtc.Ticks - centuryBegins.Ticks).TotalSeconds;

            var payload = new JwtPayload
            {
                {JwtRegisteredClaimNames.Sub, userId},
                {JwtRegisteredClaimNames.Iss, _options.Issuer},
                {JwtRegisteredClaimNames.Iat, now},
                {JwtRegisteredClaimNames.Exp, exp},
                {JwtRegisteredClaimNames.UniqueName, username},
                {ClaimTypes.GivenName, fullName},
                {ClaimTypes.Email, email}
            };

            if (roleClaims != null && roleClaims.Count > 0) 
                payload.AddClaims(roleClaims);

            var jwt = new JwtSecurityToken(_jwtHeader, payload);
            var token = _jwtSecurityTokenHandler.WriteToken(jwt);

            return new JsonWebToken
            {
                Token = token,
                Email = email,
                Expires = exp,
                FullName = fullName,
                UserName = username
            };
        }
    }
}
