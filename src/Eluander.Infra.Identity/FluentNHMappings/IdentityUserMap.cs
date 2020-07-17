using Eluander.Domain.Identity.Entities;
using FluentNHibernate.Mapping;

namespace Eluander.Infra.Identity.FluentNHMappings
{
    public sealed class IdentityUserMap : ClassMap<IdentityUser>
    {
        public IdentityUserMap()
        {
            Table("aspnet_users");
            Schema("public");

            Id(x => x.Id).Column("id").Length(32).GeneratedBy.TriggerIdentity();
            Map(x => x.UserName).Column("user_name").Length(64).Not.Nullable().Unique();
            Map(x => x.NormalizedUserName).Column("normalized_user_name").Length(64).Not.Nullable().Unique();
            Map(x => x.Email).Column("email").Length(256).Not.Nullable();
            Map(x => x.NormalizedEmail).Column("normalized_email").Length(256).Not.Nullable();
            Map(x => x.EmailConfirmed).Column("email_confirmed").Not.Nullable();
            Map(x => x.PhoneNumber).Column("phone_number").Length(128).Nullable();
            Map(x => x.PhoneNumberConfirmed).Column("phone_number_confirmed").Nullable();
            Map(x => x.LockoutEnabled).Column("lockout_enabled").Not.Nullable();
            Map(x => x.LockoutEndUnixTimeSeconds).Column("lockout_end_unix_time_seconds").Nullable();
            Map(x => x.AccessFailedCount).Column("access_failed_count").Not.Nullable();
            Map(x => x.ConcurrencyStamp).Column("concurrency_stamp").Length(36).Nullable();
            Map(x => x.PasswordHash).Column("password_hash").Length(256).Not.Nullable();
            Map(x => x.TwoFactorEnabled).Column("two_factor_enabled").Not.Nullable();
            Map(x => x.SecurityStamp).Column("security_stamp").Length(64).Nullable();
        }
    }
}
