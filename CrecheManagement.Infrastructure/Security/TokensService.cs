using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CrecheManagement.Domain.Interfaces.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CrecheManagement.Infrastructure.Security;

public class TokensService : ITokensService
{
    private readonly string _signingKey;
    private readonly int _expirationInMinutes;

    public TokensService(IConfiguration configuration)
    {
        _signingKey = configuration["Security:JWT:SigningKey"]!;
        _expirationInMinutes = configuration.GetValue<int>("Security:JWT:ExpirationTime");
    }

    public string GenerateAccessToken(string userIdentifier)
    {
        var claims = new List<Claim> { new (ClaimTypes.Sid, userIdentifier) };
        var descriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationInMinutes),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
        };

        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    public string ValidateTokenAndGetUserIdentifier(string token)
    {
        var validationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(),
        };

        var handler = new JwtSecurityTokenHandler();
        var principal = handler.ValidateToken(token, validationParameters, out _);

        var identifier = principal.Claims.First(x => x.Type == ClaimTypes.Sid).Value;

        if (string.IsNullOrEmpty(identifier))
            throw new SecurityTokenValidationException("Unauthorized. COD: 002");

        return identifier;
    }

    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString("N");
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var key = Encoding.UTF8.GetBytes(_signingKey);
        return new SymmetricSecurityKey(key);
    }
}