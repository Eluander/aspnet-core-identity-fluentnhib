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
    /// <summary>
    /// 
    /// </summary>
    public class TokenService : ITokenService
    {
        #region Repositories and Constructors
        public IConfiguration Configuration { get; }
        public TokenService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        /// <summary>
        /// Gerador de token.
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="criation"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        public string GenerateToken(ClaimsIdentity claimsIdentity, DateTime? criation, DateTime? expiration)
        {
            var jwtBearerSecret = Configuration.GetSection("Authentication:JwtBearer");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtBearerSecret["Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "https://eluander.com.br",
                Subject = claimsIdentity,
                NotBefore = criation ?? DateTime.UtcNow,
                Expires = expiration ?? DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
