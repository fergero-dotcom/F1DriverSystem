using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using F1DriverSystem.Core;
using F1DriverSystem.DataAccessLayer;
using F1DriverSystem.Models;

namespace F1DriverSystem.Tests
{
    internal static class Program
    {
        private static int passed;

        private static int Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    DriverScenarios(new MemoryRepository<Driver>(), "Unit");
                    RaceScenarios(new MemoryRepository<Race>(), "Unit");
                    ContractScenarios();
                    Console.WriteLine("Database integration tests NOT RUN. Use --database on Windows with SQL Server.");
                }
                else if (args.Length == 1 && args[0] == "--database")
                    DatabaseScenarios();
                else
                    throw new ArgumentException("Usage: F1DriverSystem.Tests.exe [--database]");

                Console.WriteLine("PASS: " + passed + " checks.");
                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("FAIL after " + passed + " checks: " + ex);
                return 1;
            }
        }

        private static void Check(bool condition, string name)
        {
            if (!condition) throw new Exception(name);
            passed++;
            Console.WriteLine("OK: " + name);
        }

        private static void Throws<TException>(Action action, string name) where TException : Exception
        {
            try { action(); }
            catch (TException) { Check(true, name); return; }
            throw new Exception("Expected " + typeof(TException).Name + ": " + name);
        }

        private static void DriverScenarios(IRepository<Driver> repo, string label)
        {
            var logic = new Logic(repo);
            Check(logic.GetAllDrivers().Count == 0, label + " Driver empty");
            Check(logic.CalculateAveragePoints() == 0, label + " Driver empty average");
            Check(!logic.AddDriver(" ", "A", 1), label + " Driver blank name");
            Check(!logic.AddDriver("A", null, 1), label + " Driver null team");
            Check(!logic.AddDriver("A", "B", -1), label + " Driver negative points");
            Check(logic.AddDriver(" Пилот O'Neil ", " Команда А ", 10), label + " Driver Add");
            Check(logic.FindById(1).Name == "Пилот O'Neil" &&
                logic.FindById(1).Team == "Команда А", label + " Driver ReadById / trim / Unicode / quote");
            Check(logic.AddDriver("Второй", "Команда А", 0), label + " Driver zero points");
            Check(logic.GetAllDrivers().Select(x => x.Id).SequenceEqual(new[] { 1, 2 }), label + " Driver ReadAll ordered");
            Check(logic.GetDriversByTeam(" команда а ").Count == 2, label + " Driver case-insensitive filter");
            Check(logic.GetDriversByTeam(null).Count == 0, label + " Driver null filter");
            Check(logic.CalculateAveragePoints() == 5, label + " Driver average");
            Check(!logic.UpdateDriver(1, "Bad", "A", -1), label + " Driver invalid Update");
            Check(logic.FindById(1).Points == 10, label + " Driver invalid Update leaves data intact");
            Check(logic.UpdateDriver(1, " Изменён ", " Другая ", 20), label + " Driver Update");
            Check(logic.FindById(1).Name == "Изменён" && logic.FindById(1).Points == 20,
                label + " Driver persisted Update");
            Check(!logic.UpdateDriver(999, "A", "B", 1), label + " Driver Update missing");
            Check(!logic.UpdateDriver(0, "A", "B", 1), label + " Driver Update invalid ID");
            Check(logic.FindById(999) == null, label + " Driver ReadById missing");
            Check(!logic.DeleteDriver(999), label + " Driver Delete missing");
            Check(logic.DeleteDriver(1), label + " Driver Delete");
            Check(logic.AddDriver("Новый", "A", 1) && logic.FindById(1).Name == "Новый",
                label + " Driver first free ID reused");
            var detached = logic.FindById(1);
            detached.Points = 777;
            Check(logic.FindById(1).Points == 1, label + " Driver no implicit write");
            foreach (var driver in logic.GetAllDrivers()) logic.DeleteDriver(driver.Id);
            Check(logic.GetAllDrivers().Count == 0, label + " Driver all deleted");
        }

        private static void RaceScenarios(IRepository<Race> repo, string label)
        {
            var logic = new RaceLogic(repo);
            Check(logic.GetAllRaces().Count == 0, label + " Race empty");
            Check(!logic.AddRace("", "A", 1), label + " Race blank name");
            Check(!logic.AddRace("A", " ", 1), label + " Race blank country");
            Check(!logic.AddRace("A", "B", 0), label + " Race zero laps");
            Check(!logic.AddRace("A", "B", -1), label + " Race negative laps");
            Check(logic.AddRace(" Учебный Гран-при O'Neil ", " Страна А ", 50), label + " Race Add");
            Check(logic.FindById(1).Name == "Учебный Гран-при O'Neil" &&
                logic.FindById(1).Country == "Страна А", label + " Race ReadById / trim / Unicode / quote");
            Check(logic.AddRace("Второй", "B", 1), label + " Race second Add");
            Check(logic.GetAllRaces().Select(x => x.Id).SequenceEqual(new[] { 1, 2 }), label + " Race ReadAll ordered");
            Check(!logic.UpdateRace(1, "A", "B", 0), label + " Race invalid Update");
            Check(logic.FindById(1).Laps == 50, label + " Race invalid Update leaves data intact");
            Check(logic.UpdateRace(1, " Новый ", " Новая ", 60), label + " Race Update");
            Check(logic.FindById(1).Laps == 60 && logic.FindById(1).Name == "Новый",
                label + " Race persisted Update");
            Check(!logic.UpdateRace(999, "A", "B", 1), label + " Race Update missing");
            Check(!logic.UpdateRace(-1, "A", "B", 1), label + " Race invalid ID");
            Check(logic.FindById(999) == null, label + " Race ReadById missing");
            Check(!logic.DeleteRace(999), label + " Race Delete missing");
            Check(logic.DeleteRace(1), label + " Race Delete");
            Check(logic.AddRace("Повторный", "A", 20) && logic.FindById(1).Name == "Повторный",
                label + " Race first free ID reused");
            var detached = logic.FindById(1);
            detached.Laps = 999;
            Check(logic.FindById(1).Laps == 20, label + " Race no implicit write");
            foreach (var race in logic.GetAllRaces()) logic.DeleteRace(race.Id);
            Check(logic.GetAllRaces().Count == 0, label + " Race all deleted");
        }

        private static void ContractScenarios()
        {
            var driver = new Driver();
            ((IDomainObject)driver).ID = 17;
            Check(driver.Id == 17, "Driver ID contract");
            var race = new Race();
            ((IDomainObject)race).ID = 18;
            Check(race.Id == 18, "Race ID contract");
            Check(typeof(Driver).GetProperties().Length == 4, "Driver View has no extra ID column");
            Throws<ArgumentNullException>(() => new Logic(null), "Driver null repository");
            Throws<ArgumentNullException>(() => new RaceLogic(null), "Race null repository");
            Throws<ConfigurationErrorsException>(() => RepositoryFactory.Create<Driver>("Unknown", ""),
                "Unknown provider rejected before connecting");
            foreach (IRepository<Driver> repo in new IRepository<Driver>[]
            {
                new EntityRepository<Driver>("unused"), new DapperRepository<Driver>("unused")
            })
            {
                string label = repo.GetType().Name;
                Throws<ArgumentNullException>(() => repo.Add(null), label + " Add null");
                Throws<ArgumentNullException>(() => repo.Update(null), label + " Update null");
                Throws<ArgumentOutOfRangeException>(() => repo.Add(new Driver(0, "A", "B", 0)), label + " Add invalid ID");
                Throws<ArgumentOutOfRangeException>(() => repo.Update(new Driver(-1, "A", "B", 0)), label + " Update invalid ID");
            }

            using (var context = new MappingContext())
            {
                var builder = new DbModelBuilder();
                context.Configure(builder);
                var model = builder.Build(new DbProviderInfo("System.Data.SqlClient", "2012"));
                Check(model.ConceptualModel.EntityTypes.Count() == 2, "EF model: two entities");
                Check(model.ConceptualModel.EntityTypes.All(x => x.KeyProperties.Single().Name == "Id"),
                    "EF model: Id primary keys");
                Check(model.StoreModel.EntityTypes.Count() == 2, "EF model: two tables");
                model.Compile();
                Check(true, "EF model compiles");
            }
        }

        private sealed class MappingContext : DBContext
        {
            public MappingContext() : base("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=Unused;Integrated Security=True") { }
            public void Configure(DbModelBuilder builder) { base.OnModelCreating(builder); }
        }

        private static void DatabaseScenarios()
        {
            var setting = ConfigurationManager.ConnectionStrings["F1Database"];
            string configured = Environment.GetEnvironmentVariable("F1_TEST_CONNECTION") ??
                (setting == null ? null : setting.ConnectionString);
            if (string.IsNullOrWhiteSpace(configured))
                throw new ConfigurationErrorsException("Missing F1Database.");
            var builder = new SqlConnectionStringBuilder(configured);
            string databaseName = "F1DriverSystem_Lab2_Tests_" + Guid.NewGuid().ToString("N");
            builder.InitialCatalog = databaseName;
            string connectionString = builder.ConnectionString;
            bool initialized = false;
            try
            {
                DatabaseInitializer.EnsureCreated(connectionString);
                initialized = true;
                var initial = new DapperRepository<Driver>(connectionString);
                Check(initial.ReadAll().Count == 23, "Database initial seed");
                foreach (var driver in initial.ReadAll()) initial.Delete(driver.Id);

                foreach (string provider in new[] {  "Dapper" })
                {
                    DriverScenarios(RepositoryFactory.Create<Driver>(provider, connectionString), provider);
                    RaceScenarios(RepositoryFactory.Create<Race>(provider, connectionString), provider);
                }

                var efDrivers = new EntityRepository<Driver>(connectionString);
                var dapperDrivers = new DapperRepository<Driver>(connectionString);
                efDrivers.Add(new Driver(1, "EF → Dapper", "Тест", 42));
                Check(dapperDrivers.ReadById(1).Points == 42, "Driver EF write / Dapper read");
                dapperDrivers.Update(new Driver(1, "Обновлён", "Тест", 84));
                Check(efDrivers.ReadById(1).Points == 84, "Driver Dapper write / EF read");
                Check(dapperDrivers.Delete(1) && efDrivers.ReadById(1) == null, "Driver shared Delete");

                var efRaces = new EntityRepository<Race>(connectionString);
                var dapperRaces = new DapperRepository<Race>(connectionString);
                dapperRaces.Add(new Race(1, "Тестовый этап", "Тест", 10));
                Check(efRaces.ReadById(1).Laps == 10, "Race Dapper write / EF read");
                efRaces.Update(new Race(1, "Изменённый этап", "Тест", 20));
                Check(dapperRaces.ReadById(1).Laps == 20, "Race EF write / Dapper read");
                Check(efRaces.Delete(1) && dapperRaces.ReadById(1) == null, "Race shared Delete");

                builder.ApplicationName = "Lab2.Reinitialize";
                DatabaseInitializer.EnsureCreated(builder.ConnectionString);
                Check(initial.ReadAll().Count == 0, "Empty table is not reseeded on restart");
                Check(new DapperRepository<Race>(builder.ConnectionString).ReadAll().Count == 0,
                    "Race deletion persists after reinitialize");
            }
            finally
            {
                if (initialized)
                {
                    SqlConnection.ClearAllPools();
                    builder.InitialCatalog = "master";
                    using (var connection = new SqlConnection(builder.ConnectionString))
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText = "ALTER DATABASE [" + databaseName +
                            "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [" + databaseName + "];";
                        command.ExecuteNonQuery();
                    }
                    Console.WriteLine("Temporary test database removed: " + databaseName);
                }
            }
        }
    }
}
