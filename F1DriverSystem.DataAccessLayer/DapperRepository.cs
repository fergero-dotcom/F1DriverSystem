using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using F1DriverSystem.Models;

namespace F1DriverSystem.DataAccessLayer
{
    /// <summary>
    /// Реализует репозиторий сущностей с использованием Dapper.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string connectionString;
        private readonly EntityMap map;

        /// <summary>
        /// Создаёт новый экземпляр класса DapperRepository.
        /// </summary>
        /// <param name="connectionString">Строка подключения к базе данных.</param>
        public DapperRepository(string connectionString)
        {
            this.connectionString = connectionString;
            map = EntityMap.For<T>();
        }

        /// <summary>
        /// Добавляет сущность в базу данных.
        /// </summary>
        /// <param name="entity">Добавляемая сущность.</param>
        public void Add(T entity)
        {
            Validate(entity);
            using (var connection = new SqlConnection(connectionString))
                connection.Execute("INSERT INTO " + map.Table + " (" + map.Columns +
                    ") VALUES (" + map.Parameters + ")", entity);
        }

        /// <summary>
        /// Удаляет сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>true, если сущность удалена; иначе false.</returns>
        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
                return connection.Execute("DELETE FROM " + map.Table + " WHERE [Id]=@Id",
                    new { Id = id }) > 0;
        }

        /// <summary>
        /// Возвращает все сущности из базы данных.
        /// </summary>
        /// <returns>Список сущностей.</returns>
        public List<T> ReadAll()
        {
            using (var connection = new SqlConnection(connectionString))
                return connection.Query<T>("SELECT " + map.Columns + " FROM " + map.Table +
                    " ORDER BY [Id]").ToList();
        }

        /// <summary>
        /// Находит сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Найденная сущность или null.</returns>
        public T ReadById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
                return connection.QuerySingleOrDefault<T>("SELECT " + map.Columns + " FROM " +
                    map.Table + " WHERE [Id]=@Id", new { Id = id });
        }

        /// <summary>
        /// Обновляет сущность в базе данных.
        /// </summary>
        /// <param name="entity">Обновляемая сущность.</param>
        /// <returns>true, если сущность обновлена; иначе false.</returns>
        public bool Update(T entity)
        {
            Validate(entity);
            using (var connection = new SqlConnection(connectionString))
                return connection.Execute("UPDATE " + map.Table + " SET " + map.Assignments +
                    " WHERE [Id]=@Id", entity) > 0;
        }

        /// <summary>
        /// Проверяет корректность сущности.
        /// </summary>
        /// <param name="entity">Проверяемая сущность.</param>
        private static void Validate(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.ID <= 0) throw new ArgumentOutOfRangeException(nameof(entity), "ID должен быть положительным.");
        }
    }
}
