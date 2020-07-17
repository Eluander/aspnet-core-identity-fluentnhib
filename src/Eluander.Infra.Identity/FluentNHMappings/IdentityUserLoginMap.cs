using Eluander.Domain.Identity.Entities;
using FluentNHibernate.Mapping;

namespace Eluander.Infra.Identity.FluentNHMappings
{
    public sealed class IdentityUserLoginMap : ClassMap<IdentityUserLogin>
    {
        public IdentityUserLoginMap()
        {
            Table("aspnet_user_logins");
            Schema("public");

            CompositeId()
                .KeyProperty(x => x.LoginProvider, "login_provider")
                .KeyProperty(x => x.ProviderKey, "provider_key");

            Map(x => x.ProviderDisplayName).Column("provider_display_name").Length(32).Not.Nullable();
            Map(x => x.UserId).Column("user_id").Length(32).Not.Nullable();
        }
    }
}
