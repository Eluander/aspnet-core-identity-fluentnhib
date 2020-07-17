using Eluander.Domain.Identity.Entities;
using FluentNHibernate.Mapping;

namespace Eluander.Infra.Identity.FluentNHMappings
{
    public sealed class IdentityRoleMap : ClassMap<IdentityRole>
    {
        public IdentityRoleMap()
        {
            Table("aspnet_roles");
            Schema("public");

            Id(x => x.Id).Column("id").Length(32).GeneratedBy.TriggerIdentity();
            Map(x => x.Name).Column("name").Length(64).Not.Nullable().Unique();
            Map(x => x.NormalizedName).Column("normalized_name").Length(64).Not.Nullable().Unique();
            Map(x => x.ConcurrencyStamp).Column("concurrency_stamp").Length(36).Nullable();
        }
    }
}
