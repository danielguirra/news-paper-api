using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Modules.User;

namespace Modules.Auth
{
    public static class TokenService
    {
        public static string GetToken(UserModel user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
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

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static AuthModel? ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AuthSettings.Secret);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);

                var idString = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var name = principal.FindFirst(ClaimTypes.Name)?.Value;
                var role = principal.FindFirst(ClaimTypes.Role)?.Value;

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
}
