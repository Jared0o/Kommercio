using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Kommercio.Modules.Users.Core.Entities;
using Kommercio.Modules.Users.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Kommercio.Modules.Users.Infrastructure.Services;

public class TokenGenerator : ITokenGenerator
{
    private readonly IConfiguration _configuration;

    public TokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateToken(User user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"], null, claims, expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}