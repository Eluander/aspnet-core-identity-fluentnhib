using Eluander.Domain.Identity.Entities;
using FluentNHibernate.Mapping;

namespace Eluander.Infra.Identity.Mappings
{
    public sealed class IdentityUserTokenMap : ClassMap<IdentityUserToken>
    {
        public IdentityUserTokenMap()
        {
            Table("aspnet_user_tokens");
            Schema("public");

            CompositeId()
                .KeyProperty(x => x.UserId, "user_id")
                .KeyProperty(x => x.LoginProvider, "login_provider")
                .KeyProperty(x => x.Name, "name");

            Map(x => x.Value).Column("value").Length(256).Not.Nullable();
        }
    }
}
