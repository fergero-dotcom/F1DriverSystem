using System;
using System.Configuration;
using F1DriverSystem.Models;

namespace F1DriverSystem.DataAccessLayer
{
    /// <summary>
    /// Создаёт репозитории для работы с базой данных.
    /// </summary>
    public static class RepositoryFactory
    {
        /// <summary>
        /// Создаёт репозиторий на основе настроек приложения.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <returns>Репозиторий сущностей.</returns>
        public static IRepository<T> Create<T>() where T : class, IDomainObject
        {
            string provider = ConfigurationManager.AppSettings["RepositoryProvider"] ?? "EF";
            var setting = ConfigurationManager.ConnectionStrings["F1Database"];
            if (setting == null)
                throw new ConfigurationErrorsException("В App.config отсутствует строка подключения F1Database.");
            return Create<T>(provider, setting.ConnectionString);
        }

        /// <summary>
        /// Создаёт репозиторий с указанным провайдером и строкой подключения.
        /// </summary>
        /// <typeparam name="T">Тип сущности.</typeparam>
        /// <param name="provider">Провайдер доступа к данным.</param>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        /// <returns>Репозиторий сущностей.</returns>
        public static IRepository<T> Create<T>(string provider, string connectionString)
            where T : class, IDomainObject
        {
            IRepository<T> repository;
            if (string.Equals(provider, "EF", StringComparison.OrdinalIgnoreCase))
                repository = new EntityRepository<T>(connectionString);
            else if (string.Equals(provider, "Dapper", StringComparison.OrdinalIgnoreCase))
                repository = new DapperRepository<T>(connectionString);
            else
                throw new ConfigurationErrorsException("RepositoryProvider должен быть EF или Dapper.");

            DatabaseInitializer.EnsureCreated(connectionString);
            return repository;
        }
    }
}
