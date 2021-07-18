using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ContactBook.Controllers
{
    internal class UtilityClass
    {
        public static object GenerateToken(string id, string userName, string firstName, string lastName, string email, IConfiguration config, ICollection<string> roles)
        {
            // Create and setup claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, id),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Surname, lastName),
                new Claim(ClaimTypes.GivenName, firstName)
            };

            // Create and add roles
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
            }

            // Create SecurityTokenDescriptor
            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:JWTSigninKey").Value)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            // Create a token Handler
            var tokenhandler = new JwtSecurityTokenHandler();

            var tokenCreated = tokenhandler.CreateToken(securityTokenDescriptor);

            return new
            {
                token = tokenhandler.WriteToken(tokenCreated),
                id
            };
        }
    }
}