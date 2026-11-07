using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using F1DriverSystem.Models;

namespace F1DriverSystem.DataAccessLayer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DapperRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string connectionString;
        private readonly EntityMap map;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public DapperRepository(string connectionString)
        {
            this.connectionString = connectionString;
            map = EntityMap.For<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            Validate(entity);
            using (var connection = new SqlConnection(connectionString))
                connection.Execute("INSERT INTO " + map.Table + " (" + map.Columns +
                    ") VALUES (" + map.Parameters + ")", entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(connectionString))
                return connection.Execute("DELETE FROM " + map.Table + " WHERE [Id]=@Id",
                    new { Id = id }) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<T> ReadAll()
        {
            using (var connection = new SqlConnection(connectionString))
                return connection.Query<T>("SELECT " + map.Columns + " FROM " + map.Table +
                    " ORDER BY [Id]").ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T ReadById(int id)
        {
            using (var connection = new SqlConnection(connectionString))
                return connection.QuerySingleOrDefault<T>("SELECT " + map.Columns + " FROM " +
                    map.Table + " WHERE [Id]=@Id", new { Id = id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            Validate(entity);
            using (var connection = new SqlConnection(connectionString))
                return connection.Execute("UPDATE " + map.Table + " SET " + map.Assignments +
                    " WHERE [Id]=@Id", entity) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        private static void Validate(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.ID <= 0) throw new ArgumentOutOfRangeException(nameof(entity), "ID должен быть положительным.");
        }
    }
}
