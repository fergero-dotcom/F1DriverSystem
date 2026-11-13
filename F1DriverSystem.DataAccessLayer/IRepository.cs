using System.Collections.Generic;
using F1DriverSystem.Models;

namespace F1DriverSystem.DataAccessLayer
{
    /// <summary>
    /// Определяет операции работы с сущностями в базе данных.
    /// </summary>
    /// <typeparam name="T">Тип сущности.</typeparam>
    public interface IRepository<T> where T : class, IDomainObject
    {
        /// <summary>
        /// Добавляет сущность.
        /// </summary>
        /// <param name="entity">Добавляемая сущность.</param>
        void Add(T entity);
        /// <summary>
        /// Удаляет сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>true, если сущность удалена; иначе false.</returns>
        bool Delete(int id);
        /// <summary>
        /// Возвращает все сущности.
        /// </summary>
        /// <returns>Список сущностей.</returns>
        List<T> ReadAll();
        /// <summary>
        /// Находит сущность по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор сущности.</param>
        /// <returns>Найденная сущность или null.</returns>
        T ReadById(int id);
        /// <summary>
        /// Обновляет сущность.
        /// </summary>
        /// <param name="entity">Обновляемая сущность.</param>
        /// <returns>true, если сущность обновлена; иначе false.</returns>
        bool Update(T entity);
    }
}
