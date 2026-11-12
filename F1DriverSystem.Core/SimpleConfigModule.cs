using System;
using System.Configuration;
using F1DriverSystem.DataAccessLayer;
using F1DriverSystem.Models;
using Ninject.Modules;

namespace F1DriverSystem.Core
{
    /// <summary>
    /// Модуль конфигурации Ninject: сопоставляет абстракции IRepository
    /// конкретным реализациям (EntityRepository или DapperRepository)
    /// в соответствии с настройкой RepositoryProvider в App.config.
    /// Репозитории регистрируются как Singleton.
    /// </summary>
    public class SimpleConfigModule : NinjectModule
    {
        /// <summary>
        /// Регистрирует зависимости для всех репозиториев.
        /// </summary>
        public override void Load()
        {
            string provider = ConfigurationManager.AppSettings["RepositoryProvider"] ?? "EF";
            string connectionString =
                ConfigurationManager.ConnectionStrings["F1Database"].ConnectionString;

            DatabaseInitializer.EnsureCreated(connectionString);

            if (string.Equals(provider, "Dapper", StringComparison.OrdinalIgnoreCase))
            {
                Bind<IRepository<Driver>>().To<DapperRepository<Driver>>()
                    .InSingletonScope()
                    .WithConstructorArgument("connectionString", connectionString);
                Bind<IRepository<Race>>().To<DapperRepository<Race>>()
                    .InSingletonScope()
                    .WithConstructorArgument("connectionString", connectionString);
            }
            else
            {
                Bind<IRepository<Driver>>().To<EntityRepository<Driver>>()
                    .InSingletonScope()
                    .WithConstructorArgument("connectionString", connectionString);
                Bind<IRepository<Race>>().To<EntityRepository<Race>>()
                    .InSingletonScope()
                    .WithConstructorArgument("connectionString", connectionString);
            }
        }
    }
}
