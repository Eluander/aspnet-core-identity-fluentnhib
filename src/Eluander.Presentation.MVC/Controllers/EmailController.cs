using Eluander.Presentation.MVC.Models;
using Eluander.Presentation.MVC.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Eluander.Presentation.MVC.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class EmailController : Controller
    {
        #region Repositorios e Construtores
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender,
            IHostingEnvironment env)
        {
            _emailSender = emailSender;
        }
        #endregion

        #region Views
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult EmailResponse()
        {
            TempData.Peek("Status");
            return View();
        }
        #endregion

        #region Actions
        [HttpPost]
        public IActionResult Index(EmailModel email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmailSender(email.Destino, email.Assunto, email.Mensagem).GetAwaiter();

                    TempData["Status"] = true;
                    return RedirectToAction("EmailResponse");
                }
                catch (Exception e)
                {
                    TempData["Status"] = false;
                    return RedirectToAction("EmailResponse");
                }
            }
            return View(email);
        }
        public async Task EmailSender(string email, string assunto, string mensagem)
        {
            await _emailSender.SendEmailAsync(email, assunto, mensagem);
        }
        #endregion
    }
}
