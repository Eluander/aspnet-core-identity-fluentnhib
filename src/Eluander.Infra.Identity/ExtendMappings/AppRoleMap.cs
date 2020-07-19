using Eluander.Domain.Identity.Extends;
using FluentNHibernate.Mapping;

namespace Eluander.Infra.Identity.ExtendMappings
{
    public sealed class AppRoleMap : SubclassMap<AppRole>
    {
        public AppRoleMap()
        {
            Table("app_roles");
            Schema("public");

            KeyColumn("id");

            Map(x => x.Description).Column("description").Length(256);
        }
    }
}
