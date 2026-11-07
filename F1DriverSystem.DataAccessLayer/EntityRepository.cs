using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using F1DriverSystem.Models;

namespace F1DriverSystem.DataAccessLayer
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject
    {
        private readonly string connectionString;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public EntityRepository(string connectionString)
        {
            EntityMap.For<T>();
            this.connectionString = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Add(T entity)
        {
            Validate(entity);
            using (var context = new DBContext(connectionString))
            {
                context.Set<T>().Add(entity);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            using (var context = new DBContext(connectionString))
            {
                T entity = context.Set<T>().Find(id);
                if (entity == null) return false;
                context.Set<T>().Remove(entity);
                context.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<T> ReadAll()
        {
            using (var context = new DBContext(connectionString))
                return context.Set<T>().AsNoTracking().ToList().OrderBy(x => x.ID).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T ReadById(int id)
        {
            using (var context = new DBContext(connectionString))
                return context.Set<T>().Find(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            Validate(entity);
            using (var context = new DBContext(connectionString))
            {
                T stored = context.Set<T>().Find(entity.ID);
                if (stored == null) return false;
                context.Entry(stored).CurrentValues.SetValues(entity);
                context.SaveChanges();
                return true;
            }
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
