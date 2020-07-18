using Eluander.Domain.Identity.Entities;
using FluentNHibernate.Mapping;

namespace Eluander.Infra.Identity.Mappings
{
    public sealed class IdentityUserClaimMap : ClassMap<IdentityUserClaim>
    {
        public IdentityUserClaimMap()
        {
            Table("aspnet_user_claims");
            Schema("public");

            Id(x => x.Id).Column("id").GeneratedBy.Sequence("snow_flake_id_seq");
            Map(x => x.ClaimType).Column("claim_type").Length(1024).Not.Nullable();
            Map(x => x.ClaimValue).Column("claim_value").Length(1024).Not.Nullable();
            Map(x => x.UserId).Column("user_id").Length(32).Not.Nullable();
        }
    }
}
