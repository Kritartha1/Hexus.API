using Hexus.Data;
using Hexus.Models.Domain;
using Hexus.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Hexus.Repositories.Implementation
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration configuration;
        private readonly HexusApiAuthDbContext dbContext;

        public TokenRepository(IConfiguration configuration,HexusApiAuthDbContext dbContext)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
        }
        public string CreateJWTToken(User user, List<string> roles)
        {
            var claims = new List<Claim>
            {
               // new Claim(JwtRegisteredClaimNames.NameId,user.Email)
               new Claim(ClaimTypes.Email, user.Email)
            };
            claims.Add(new Claim(JwtRegisteredClaimNames.NameId,user.UserName));
            
            

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials

            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

       /* public string CreateJWTToken(User user, List<string> roles)
        {
            throw new NotImplementedException();
        }*/
    }
}
