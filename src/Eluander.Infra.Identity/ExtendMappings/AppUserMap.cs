using Eluander.Domain.Identity.Extends;
using FluentNHibernate.Mapping;

namespace Eluander.Infra.Identity.ExtendMappings
{
    public sealed class AppUserMap : SubclassMap<AppUser>
    {
        public AppUserMap()
        {
            Table("app_users");
            Schema("public");

            KeyColumn("id");

            Map(x => x.DHCadastro).Column("dh_cadastro").Not.Nullable().Generated.Insert();
            Map(x => x.DHUltimoLogin).Column("dh_ultimologin");
            Map(x => x.LoginCount).Column("login_count").Not.Nullable();
        }
    }
}
