using Eluander.Domain.Identity.Entities;
using System;

namespace Eluander.Domain.Identity.Extends
{
    public class AppUser : IdentityUser
    {
        public virtual DateTime DHCadastro { get; set; }
        public virtual DateTime? DHUltimoLogin { get; set; }
        public virtual int LoginCount { get; set; }
    }
}
