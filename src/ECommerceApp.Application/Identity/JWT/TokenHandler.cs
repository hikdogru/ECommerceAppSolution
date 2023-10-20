using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ECommerceApp.Application.Models.User;
using ECommerceApp.Core.Domain.Entities.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ECommerceApp.Application.Identity.JWT;
public class TokenHandler : ITokenHandler
{
    private readonly JwtConfig _jwtConfig;

    public TokenHandler(IOptions<JwtConfig> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public TokenModel GenerateToken(AppUser user, int minute, IEnumerable<string> roles)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfig.SecurityKey));
        var now = DateTime.UtcNow;
        var expires = now.Add(TimeSpan.FromDays(2));
        var claims = new List<Claim>
            {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            };

        claims.AddRange(roles.Select(role => new Claim("role", role)));
        var jwt = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            notBefore: now,
            expires: expires,
            signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
            );

        return new TokenModel
        {
            Token = new JwtSecurityTokenHandler().WriteToken(jwt),
            Expiration = expires
        };
    }
}
