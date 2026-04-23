using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class JwtService(IConfiguration config) : IJwtService
    {
        public string GenerateToken(User user)
        {
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));

            var Clims= new[]
            {

            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role,  user.Role.ToString()),
            new Claim("employeeId", user.Employee?.Id.ToString() ?? "")

            };

            var token = new JwtSecurityToken(

                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: Clims,
                expires : GetExpiration(),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime GetExpiration() =>
       DateTime.UtcNow.AddHours(
           double.Parse(config["Jwt:ExpiryHours"] ?? "24"));
    }
}
