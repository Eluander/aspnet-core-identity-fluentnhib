using Eluander.Domain.Identity.Extends;

namespace Eluander.Presentation.MVC.Repositories.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(AppUser appUser);
    }
}