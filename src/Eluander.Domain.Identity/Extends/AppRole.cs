using Eluander.Domain.Identity.Entities;

namespace Eluander.Domain.Identity.Extends
{
    public class AppRole : IdentityRole
    {
        public virtual string Description { get; set; }
    }
}
