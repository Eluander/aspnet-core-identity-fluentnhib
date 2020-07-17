using Eluander.Infra.Identity.Migrations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using SimpleMigrations;
using SimpleMigrations.DatabaseProvider;
using System.Reflection;

namespace Eluander.Teste.Identity.Migration
{
    [TestClass]
    public class Migration_Go
    {
        private const string cnn = "User ID=postgres;Host=127.0.0.1;Port=5432;Database=fluentnhib;Password=123;";
        private Assembly assemblyTo = typeof(_0001_aspnet_core_Identity).Assembly;

        [TestMethod]
        public void UpgradeDataBase()
        {
            // start conecxão da db.
            using (var connection = new NpgsqlConnection(cnn))
            {
                var databaseProvider = new PostgresqlDatabaseProvider(connection);
                var migrator = new SimpleMigrator(assemblyTo, databaseProvider);

                // carregar todos os estados de versões da base de dados.
                migrator.Load();

                // quando existir uma versão já ativa, inicie as migrações apratir de uma versão especifica.
                //migrator.Baseline(10);

                // verificar se a versão da db atual é diferente da ultima versão dos arquivos de migrações.
                if (migrator.CurrentMigration.Version != migrator.LatestMigration.Version)
                {
                    // atualizar db para ultima versão.
                    migrator.MigrateToLatest();

                    // atualizar db até o arquivo de migração numero 11.
                    //migrator.MigrateTo(11);
                }

                Assert.IsTrue(migrator.CurrentMigration.Version == migrator.LatestMigration.Version);
            }
        }

        [TestMethod]
        public void DowngradeDataBase()
        {
            int versionTo = 0;

            using (var connection = new NpgsqlConnection(cnn))
            {
                var databaseProvider = new PostgresqlDatabaseProvider(connection);
                var migrator = new SimpleMigrator(assemblyTo, databaseProvider);

                // carregar todos os estados de versões da base de dados.
                migrator.Load();

                // a versão da db atual deve ser maior que a versão versionTo.
                if (migrator.CurrentMigration.Version > versionTo)
                {
                    // versão da db para qual deseja voltar.
                    migrator.MigrateTo(versionTo);
                }

                Assert.IsTrue(migrator.CurrentMigration.Version == migrator.LatestMigration.Version);
            }
        }
    }
}
