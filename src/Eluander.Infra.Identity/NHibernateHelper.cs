using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;

namespace Eluander.Infra.Identity
{
    public sealed class NHibernateHelper
    {
        private static NHibernate.Cfg.Configuration Config()
        {
            var config = Fluently.Configure()
                            .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString("User ID=postgres;Password=123;Host=127.0.0.1;Port=5432;Database=NHibernate;"))
                            .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                            .ExposeConfiguration(ExecuteMigration)
                            .BuildConfiguration();

            return config;
        }

        private static void ExecuteMigration(NHibernate.Cfg.Configuration cfg)
        {
            new SchemaUpdate(cfg).Execute(true, true);
            cfg.SetProperty("use_proxy_validator", "false");
        }

        public static ISessionFactory SessionFactory()
        {
            var config = Config();

            ISessionFactory sessionFactory = config.BuildSessionFactory();
            return sessionFactory;
        }
    }
}
