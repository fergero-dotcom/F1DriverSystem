using System;
using System.Collections.Generic;
using System.Linq;
using F1DriverSystem.Models;
using F1DriverSystem.DataAccessLayer;

namespace F1DriverSystem.Tests
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal sealed class MemoryRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        private readonly Dictionary<int, T> data = new Dictionary<int, T>();

        public void Add(T entity)
        {
            data.Add(entity.ID, Copy(entity));
        }

        public bool Delete(int id)
        {
            return data.Remove(id);
        }

        public List<T> ReadAll()
        {
            return data.Values.Select(Copy).OrderBy(x => x.ID).ToList();
        }

        public T ReadById(int id)
        {
            T entity;
            return data.TryGetValue(id, out entity) ? Copy(entity) : null;
        }

        public bool Update(T entity)
        {
            if (!data.ContainsKey(entity.ID)) return false;
            data[entity.ID] = Copy(entity);
            return true;
        }

        private static T Copy(T source)
        {
            var result = new T();
            foreach (var property in typeof(T).GetProperties())
                if (property.CanRead && property.CanWrite)
                    property.SetValue(result, property.GetValue(source));
            return result;
        }
    }
}
