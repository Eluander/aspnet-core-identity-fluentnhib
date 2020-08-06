using Eluander.Domain.Identity.Commands;
using Eluander.Domain.Identity.Extends;
using Eluander.Presentation.MVC.Areas.Identity.Pages.Account;
using Eluander.Presentation.MVC.Repositories;
using Eluander.Presentation.MVC.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Eluander.Presentation.MVC.Areas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Repositories and constructors
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _logger = logger;
        }

        #endregion

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authentication([FromBody] LoginRequest model)
        {
            var response = await _signInManager.PasswordSignInAsync(model.Usuario, model.Senha, false, false);
            AppUser user = null;
            if (response.Succeeded)
            {
                _logger.LogInformation("Usuário conectado.");
                user = await _userManager.FindByNameAsync(model.Usuario);
                user.PasswordHash = null;
            }
            if (response.IsLockedOut)
            {
                _logger.LogWarning("Conta de usuário bloqueada.");
                return Unauthorized(new { message = "Este usuário esta bloqueado." });
            }
            //if (response.RequiresTwoFactor)
            //{
            //    return NotFound(new { message = "Atenção, autenticação 2 fatores." });
            //}

            if (user == null)
                return NotFound(new { message = "Usuário ou senha inválido." });

            // gerar token
            var token = _tokenService.GenerateToken(user);

            return Ok(new
            {
                token,
                user,
                expiration = DateTime.UtcNow.AddHours(2)
            });
        }
    }
}
