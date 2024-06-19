using Bss.Component.Identity.Configuration;
using Bss.Component.Identity.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bss.Component.Identity.Services;

public interface IJwtTokenService
{
    (string? tokenId, string token, DateTime expires) GenerateToken(ApplicationUser user, IEnumerable<string> roles, DateTime now);
}

public class JwtTokenService : IJwtTokenService
{
    private readonly SigningCredentials _signingCredentials;
    private readonly BssIdentityConfiguration.JwtConfiguration _config;

    public JwtTokenService(IOptions<BssIdentityConfiguration> config)
    {
        _config = config.Value.Jwt;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.SigningKey));
        _signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
    }

    public (string? tokenId, string token, DateTime expires) GenerateToken(ApplicationUser user, IEnumerable<string> roles, DateTime now)
    {
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        if (user.UserName is not null)
        {
            claims.Add(new(ClaimTypes.Name, user.UserName));
        }

        if (user.Email is not null)
        {
            claims.Add(new(ClaimTypes.Email, user.Email));
        }

        foreach (var role in roles)
        {
            claims.Add(new (ClaimTypes.Role, role));
        }

        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var expires = now.AddHours(_config.ExpireHours);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _config.Issuer,
            Audience = _config.Audience,
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            SigningCredentials = _signingCredentials
        };

        var securityToken = jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        return (securityToken.Id, jwtSecurityTokenHandler.WriteToken(securityToken), expires);
    }
}