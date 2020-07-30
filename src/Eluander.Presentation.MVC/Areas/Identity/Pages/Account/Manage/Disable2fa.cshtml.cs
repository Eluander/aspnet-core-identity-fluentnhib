using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eluander.Domain.Identity.Extends;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Eluander.Presentation.MVC.Areas.Identity.Pages.Account.Manage
{
    public class Disable2faModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<Disable2faModel> _logger;

        public Disable2faModel(
            UserManager<AppUser> userManager,
            ILogger<Disable2faModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            if (!await _userManager.GetTwoFactorEnabledAsync(user))
            {
                throw new InvalidOperationException($"Não é possível desativar o 2FA para o usuário com ID '{_userManager.GetUserId(User)}' pois não está ativado no momento.");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Não foi possível carregar o usuário com o ID '{_userManager.GetUserId(User)}'.");
            }

            var disable2faResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
            if (!disable2faResult.Succeeded)
            {
                throw new InvalidOperationException($"Ocorreu um erro inesperado ao desativar o 2FA para o usuário com ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("O usuário com o ID '{UserId}' desativou 2fa.", _userManager.GetUserId(User));
            StatusMessage = "2fa foi desativado. Você pode reativar o 2fa ao configurar um aplicativo autenticador";
            return RedirectToPage("./TwoFactorAuthentication");
        }
    }
}