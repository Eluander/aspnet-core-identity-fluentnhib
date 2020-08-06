using Eluander.Domain.Identity.Extends;
using Eluander.Presentation.MVC.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eluander.Presentation.MVC.Repositories
{
    public class TokenService : ITokenService
    {
        #region Repositories and Constructors
        public IConfiguration Configuration { get; }
        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        public string GenerateToken(AppUser appUser)
        {
            var jwtBearerSecret = Configuration.GetSection("Authentication:JwtBearer");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerSecret["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, appUser.Email),
                    //new Claim(ClaimTypes.Role, appUser.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
