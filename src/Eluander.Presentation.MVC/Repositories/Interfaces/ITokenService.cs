using System;
using System.Security.Claims;

namespace Eluander.Presentation.MVC.Repositories.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gerador de token.
        /// </summary>
        /// <param name="claimsIdentity"></param>
        /// <param name="criation"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        string GenerateToken(ClaimsIdentity claimsIdentity, DateTime? criation, DateTime? expiration);
    }
}