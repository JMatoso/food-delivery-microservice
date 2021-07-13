using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Account.Data;
using Account.Helpers;
using Account.Models;
using Microsoft.IdentityModel.Tokens;

namespace Account.Services
{
    public static class TokenService
    {
        public static TokenReturned GenerateToken(ApplicationUser user) 
        { 
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(JwtSettings.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor 
            {
                Audience = JwtSettings.AudienceToken,
                Issuer = JwtSettings.IssuerToken,
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(JwtSettings.ExpireMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new TokenReturned
            {
                Id = Guid.Parse(user.Id),
                Name = $"{user.FirstName} {user.LastName}",
                Token = tokenHandler.WriteToken(token),
                Role = user.Role,
                ExpireTime = token.ValidTo.AddMinutes(JwtSettings.ExpireMinutes)
            };
        }
    }
}