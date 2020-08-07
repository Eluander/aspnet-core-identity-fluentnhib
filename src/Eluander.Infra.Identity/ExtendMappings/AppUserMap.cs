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

        }
    }
}
