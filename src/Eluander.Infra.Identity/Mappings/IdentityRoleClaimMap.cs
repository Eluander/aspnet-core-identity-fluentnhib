using Eluander.Domain.Identity.Entities;
using FluentNHibernate.Mapping;

namespace Eluander.Infra.Identity.Mappings
{
    public sealed class IdentityRoleClaimMap : ClassMap<IdentityRoleClaim>
    {
        public IdentityRoleClaimMap()
        {
            Table("aspnet_role_claims");
            Schema("public");

            Id(x => x.Id).Column("id").Length(32).GeneratedBy.Sequence("snow_flake_id_seq");
            Map(x => x.ClaimType).Column("claim_type").Length(1024).Not.Nullable();
            Map(x => x.ClaimValue).Column("claim_value").Length(1024).Not.Nullable();
            Map(x => x.RoleId).Column("role_id").Length(32).Not.Nullable();
        }
    }
}
