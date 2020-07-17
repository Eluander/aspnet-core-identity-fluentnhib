using Eluander.Domain.Identity.Entities;
using FluentNHibernate.Mapping;

namespace Eluander.Infra.Identity.FluentNHMappings
{
    public sealed class IdentityUserRoleMap : ClassMap<IdentityUserRole>
    {
        public IdentityUserRoleMap()
        {
            Table("aspnet_user_roles");
            Schema("public");

            CompositeId()
                .KeyProperty(x => x.UserId, "user_id")
                .KeyProperty(x => x.RoleId, "role_id");
        }
    }
}
