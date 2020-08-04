using System.Threading.Tasks;

namespace Eluander.Presentation.MVC.Repositories.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
