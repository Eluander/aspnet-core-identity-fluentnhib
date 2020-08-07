using Eluander.Domain.Identity.Commands;
using Eluander.Domain.Identity.Extends;
using Eluander.Presentation.MVC.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Eluander.Presentation.MVC.Areas.Api.Controllers
{
    /// <summary>
    /// Autenticação de usuários.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Repositories and constructors
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="tokenService"></param>
        /// <param name="logger"></param>
        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        #endregion

        /// <summary>
        /// Faça o login para obter o token.
        /// </summary>
        /// <param name="model"></param>
        /// <response code="200">Sucesso.</response>
        /// <response code="401">Usuário bloqueado.</response>
        /// <response code="404">Usuário ou senha inválido.</response>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authentication([FromBody] LoginRequest model)
        {
            var token = "";
            var userIdentity = await _userManager.FindByNameAsync(model.Usuario);

            if (userIdentity != null)
            {
                // Efetuar login com base no ID do usuário e senha.
                var response = await _signInManager.CheckPasswordSignInAsync(userIdentity, model.Senha, true);

                if (response.Succeeded)
                {
                    _logger.LogInformation("Usuário conectado.");

                    // Limpar senha do usuário.
                    userIdentity.PasswordHash = null;

                }
                else if (response.IsLockedOut)
                {
                    _logger.LogWarning("Conta de usuário bloqueada.");
                    return Unauthorized(new
                    {
                        isAuthenticated = false,
                        message = "Este usuário esta bloqueado."
                    });
                }

            }
            else
            {
                return NotFound(new
                {
                    isAuthenticated = false,
                    message = "Usuário ou senha incorreto."
                });
            }

            // Obter token.
            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(userIdentity.UserName, "Login"),
                new[]
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                    new Claim(JwtRegisteredClaimNames.UniqueName, userIdentity.UserName)
                });

            var dtCriation = DateTime.UtcNow;
            token = _tokenService.GenerateToken(identity, dtCriation, dtCriation.AddMinutes(2));

            return Ok(new
            {
                isAuthenticated = true,
                token,
                user = userIdentity,
                created = dtCriation,
                expiration = dtCriation.AddMinutes(2),
                message = "OK"
            });

        }
    }
}
