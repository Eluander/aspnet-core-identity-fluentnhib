using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eluander.Teste.Identity.Migration
{
    [TestClass]
    public class Migration_Go
    {
        [TestMethod]
        public void UpgradeDataBase()
        {
            var migrationsAssembly = typeof(_0001_Criar_Tabelas_Versao_1).Assembly;

            foreach (var cnn in lstCasa)
            {
                using (var connection = new NpgsqlConnection(cnn))
                {
                    var databaseProvider = new PostgresqlDatabaseProvider(connection);
                    var migrator = new SimpleMigrator(migrationsAssembly, databaseProvider);
                    migrator.Load();

                    //Quando ja existir um banco ativo, pode setar a versão da base de dados apartir de:
                    //migrator.Baseline(10);

                    if (migrator.CurrentMigration.Version != migrator.LatestMigration.Version)
                    {
                        //USE ESTE PARA: subir a versão até a versão final.
                        migrator.MigrateToLatest();

                        //USE ESTE PARA: delimitar a versão.
                        //migrator.MigrateTo(43);
                    }

                    Assert.IsTrue(migrator.CurrentMigration.Version == migrator.LatestMigration.Version);
                }
            }
        }

        [TestMethod]
        public void DowngradeDataBase()
        {
            var migrationsAssembly = typeof(_0001_Criar_Tabelas_Versao_1).Assembly;

            foreach (var cnn in lstTeste)
            {
                using (var connection = new NpgsqlConnection(cnn))
                {
                    var databaseProvider = new PostgresqlDatabaseProvider(connection);
                    var migrator = new SimpleMigrator(migrationsAssembly, databaseProvider);
                    migrator.Load();

                    // Adicione aqui a versão para qual deseja voltar a base de dados.
                    migrator.MigrateTo(32);
                }
            }
        }
    }
}
