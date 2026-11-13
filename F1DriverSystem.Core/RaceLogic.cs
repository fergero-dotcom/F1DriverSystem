using System;
using System.Collections.Generic;
using System.Linq;
using F1DriverSystem.DataAccessLayer;
using F1DriverSystem.Models;

namespace F1DriverSystem.Core
{
    /// <summary>
    /// Управляет данными гонок.
    /// </summary>
    public class RaceLogic
    {
        private readonly IRepository<Race> repository;

        /// <summary>
        /// Создаёт новый экземпляр класса RaceLogic.
        /// </summary>
        public RaceLogic() : this(RepositoryFactory.Create<Race>()) { }

        /// <summary>
        /// Создаёт новый экземпляр класса RaceLogic с указанным репозиторием.
        /// </summary>
        /// <param name="repository">Репозиторий гонок.</param>
        public RaceLogic(IRepository<Race> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Возвращает все гонки, отсортированные по идентификатору.
        /// </summary>
        /// <returns>Список гонок.</returns>
        public List<Race> GetAllRaces()
        {
            return repository.ReadAll().OrderBy(x => x.Id).ToList();
        }

        /// <summary>
        /// Находит гонку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор гонки.</param>
        /// <returns>Найденная гонка или null.</returns>
        public Race FindById(int id)
        {
            return repository.ReadById(id);
        }

        /// <summary>
        /// Добавляет новую гонку.
        /// </summary>
        /// <param name="name">Название гонки.</param>
        /// <param name="country">Страна проведения гонки.</param>
        /// <param name="laps">Количество кругов.</param>
        /// <returns>true, если гонка добавлена; иначе false.</returns>
        public bool AddRace(string name, string country, int laps)
        {
            if (!IsValid(name, country, laps)) return false;
            var used = new HashSet<int>(repository.ReadAll().Select(x => x.Id));
            int id = 1;
            while (used.Contains(id)) id++;
            repository.Add(new Race(id, name.Trim(), country.Trim(), laps));
            return true;
        }

        /// <summary>
        /// Обновляет данные существующей гонки.
        /// </summary>
        /// <param name="id">Идентификатор гонки.</param>
        /// <param name="name">Новое название гонки.</param>
        /// <param name="country">Новая страна проведения гонки.</param>
        /// <param name="laps">Новое количество кругов.</param>
        /// <returns>true, если гонка обновлена; иначе false.</returns>
        public bool UpdateRace(int id, string name, string country, int laps)
        {
            if (!IsValid(name, country, laps) || id <= 0) return false;
            return repository.Update(new Race(id, name.Trim(), country.Trim(), laps));
        }

        /// <summary>
        /// Удаляет гонку по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор гонки.</param>
        /// <returns>true, если гонка удалена; иначе false.</returns>
        public bool DeleteRace(int id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Проверяет корректность данных гонки.
        /// </summary>
        /// <param name="name">Название гонки.</param>
        /// <param name="country">Страна проведения гонки.</param>
        /// <param name="laps">Количество кругов.</param>
        /// <returns>true, если данные корректны; иначе false.</returns>
        private static bool IsValid(string name, string country, int laps)
        {
            return !string.IsNullOrWhiteSpace(name) &&
                   !string.IsNullOrWhiteSpace(country) && laps > 0;
        }
    }
}
