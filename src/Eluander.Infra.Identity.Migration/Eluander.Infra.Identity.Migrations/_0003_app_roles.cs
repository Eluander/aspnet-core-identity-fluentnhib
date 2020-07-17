using SimpleMigrations;

namespace Eluander.Infra.Identity.Migrations
{
    [Migration(3, "create app roles")]
    public class _0003_app_roles : Migration
    {
        protected override void Down()
        {
            Execute(@"
                -- table: public.app_roles
                drop table public.app_roles;
            ");
        }

        protected override void Up()
        {
            Execute(@"
                -- table: public.app_roles
                create table public.app_roles
                (
                    id character varying(32) collate pg_catalog.default not null,
                    description character varying(256) collate pg_catalog.default,
                    constraint pk_app_roles primary key(id),
                    constraint fk_aspnet_roles_id foreign key(id)
                        references public.aspnet_roles(id) match simple
                        on update cascade
                        on delete cascade
                )
                with(
                    oids = false
                )
                tablespace pg_default;

                        alter table public.app_roles
                            owner to postgres;
                        comment on table public.app_roles
                    is 'application roles table.';

                comment on column public.app_roles.description
                    is 'roles description';

            ");
        }
    }
}
