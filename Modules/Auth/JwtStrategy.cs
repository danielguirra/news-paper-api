using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Modules.Auth.Model;
using Modules.Auth.Settings;
using Modules.User.Model;

namespace Modules.Auth.Token;

public static class TokenService
{
    public static string GetToken(UserModel user)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[]? key = Encoding.ASCII.GetBytes(AuthSettings.Secret);
        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                ]
            ),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public static AuthModel? ValidateToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return null;

        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(AuthSettings.Secret);

        try
        {
            TokenValidationParameters validationParameters = new()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
            };

            ClaimsPrincipal principal = tokenHandler.ValidateToken(
                token,
                validationParameters,
                out _
            );

            string? idString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            string? name = principal.FindFirst(ClaimTypes.Name)?.Value;
            string? role = principal.FindFirst(ClaimTypes.Role)?.Value;

            if (
                string.IsNullOrEmpty(idString)
                || string.IsNullOrEmpty(name)
                || string.IsNullOrEmpty(role)
            )
                return null;

            AuthModel auth = new()
            {
                Id = Guid.Parse(idString),
                Name = name,
                Role = role,
            };

            return auth;
        }
        catch
        {
            return null;
        }
    }
}
