using System.Security.Claims;
using System.Text;
using Conduit.Application.Abstractions.Authentication;
using Conduit.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Infrastructure.Authentication;

public sealed class TokenService(IConfiguration configuration) : ITokenService
{
    public string Create(User user)
    {
        var secretKey = configuration.GetValue<string>("Jwt:Secret")!;

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);

        var claims = new Dictionary<string, object>
        {
            { ClaimTypes.Email, user.Email },
            { ClaimTypes.Name, user.Username },
        };

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Audience           = configuration.GetValue<string>("Jwt:Audience"),
            Claims             = claims,
            Expires            = DateTime.UtcNow.AddMinutes(5),
            Issuer             = configuration.GetValue<string>("Jwt:Issuer"),
            SigningCredentials = signingCredentials
        };

        var token = new JsonWebTokenHandler().CreateToken(securityTokenDescriptor);

        return token;
    }
}
