using API.Application.Settings;
using API.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Infrastructure.Auth
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

       public string GenerateToken(ApplicationUser user)
        {
            var claims = new[]
            {
                //new Claim(ClaimTypes.NameIdentifier, user.Id),
                //new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                //new Claim(ClaimTypes.Name, user.UserName ?? string.Empty)
                new Claim("id", user.Id),
                new Claim("email", user.Email ?? string.Empty),
                new Claim("username", user.UserName ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id)

            };

            //var key = new SymmetricSecurityKey(Convert.FromBase64String(_jwtSettings.Key));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            //var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Convert.FromBase64String(_jwtSettings.Key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwtSettings.ExpiresInHours),
                signingCredentials: credentials);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine($"Token String: {tokenString}");

            return tokenString;
        }
    }
}
