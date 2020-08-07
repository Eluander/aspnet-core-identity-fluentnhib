using SimpleMigrations;

namespace Eluander.Infra.Identity.Migrations
{
    [Migration(2, "create app users")]
    public class _0002_app_users : Migration
    {
        protected override void Down()
        {
            Execute(@"
                -- table: public.app_users
                drop table public.app_users;
            ");
        }

        protected override void Up()
        {
            Execute(@"
                -- table: public.app_users
                create table public.app_users
                (
                    id character varying(32) collate pg_catalog.default not null,
                    constraint pk_app_users primary key(id),
                    constraint fk_aspnet_users_id foreign key(id)
                        references public.aspnet_users(id) match simple
                        on update cascade
                        on delete cascade
                )
                with(
                    oids = false
                )
                tablespace pg_default;

                        alter table public.app_users
                            owner to postgres;
                        comment on table public.app_users
                    is 'application users table.';

            ");
        }
    }
}
