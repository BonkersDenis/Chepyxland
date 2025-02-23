using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Chepyxland.Services;

public class JwtTokenService
{
    private readonly string _secretKey = "my_super_secret_key_1234567890!@#$%^&*()";
    private readonly string _issuer = "asdf";
    private readonly string _audience = "asdfa";
    
    public string GenerateToken(string login)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
                     {
                         new Claim(ClaimTypes.Name, login)
                     };

        var token = new JwtSecurityToken(
                                         issuer: _issuer,
                                         audience: _audience,
                                         claims: claims,
                                         expires: DateTime.Now.AddMinutes(30), // Срок действия токена
                                         signingCredentials: credentials
                                        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var validationParameters = new TokenValidationParameters
                                   {
                                       ValidateIssuer = true,
                                       ValidateAudience = true,
                                       ValidateLifetime = true,
                                       ValidateIssuerSigningKey = true,
                                       ValidIssuer = _issuer,
                                       ValidAudience = _audience,
                                       IssuerSigningKey = new SymmetricSecurityKey(key)
                                   };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            return principal;
        }
        catch
        {
            return null; // Токен невалиден
        }
    }
}