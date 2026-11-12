using System;
using System.Collections.Generic;
using System.Linq;
using F1DriverSystem.DataAccessLayer;
using F1DriverSystem.Models;

namespace F1DriverSystem.Core
{
    /// <summary>
    /// Бизнес-логика каталога этапов (Гран-при).
    /// Класс зависит только от абстракции IRepository, конкретная реализация
    /// внедряется IoC-контейнером (Ninject) через конструктор.
    /// </summary>
    public class RaceLogic
    {
        private readonly IRepository<Race> repository;

        /// <summary>
        /// Создаёт объект бизнес-логики с внедрённым репозиторием.
        /// </summary>
        /// <param name="repository">Репозиторий этапов, внедряемый DI-контейнером.</param>
        public RaceLogic(IRepository<Race> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Возвращает список всех этапов, отсортированный по идентификатору.
        /// </summary>
        /// <returns>Список этапов.</returns>
        public List<Race> GetAllRaces()
        {
            return repository.ReadAll().OrderBy(x => x.Id).ToList();
        }

        /// <summary>
        /// Находит этап по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор этапа.</param>
        /// <returns>Этап или null, если он не найден.</returns>
        public Race FindById(int id)
        {
            return repository.ReadById(id);
        }

        /// <summary>
        /// Добавляет новый этап.
        /// </summary>
        /// <param name="name">Название этапа.</param>
        /// <param name="country">Страна проведения.</param>
        /// <param name="laps">Количество кругов.</param>
        /// <returns>true, если этап успешно добавлен.</returns>
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
        /// Обновляет данные этапа.
        /// </summary>
        /// <param name="id">Идентификатор этапа.</param>
        /// <param name="name">Новое название.</param>
        /// <param name="country">Новая страна проведения.</param>
        /// <param name="laps">Новое количество кругов.</param>
        /// <returns>true, если данные обновлены.</returns>
        public bool UpdateRace(int id, string name, string country, int laps)
        {
            if (!IsValid(name, country, laps) || id <= 0) return false;
            return repository.Update(new Race(id, name.Trim(), country.Trim(), laps));
        }

        /// <summary>
        /// Удаляет этап по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор этапа.</param>
        /// <returns>true, если этап удалён.</returns>
        public bool DeleteRace(int id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Проверяет корректность данных этапа.
        /// </summary>
        /// <param name="name">Название этапа.</param>
        /// <param name="country">Страна проведения.</param>
        /// <param name="laps">Количество кругов.</param>
        /// <returns>true, если данные корректны.</returns>
        private static bool IsValid(string name, string country, int laps)
        {
            return !string.IsNullOrWhiteSpace(name) &&
                   !string.IsNullOrWhiteSpace(country) && laps > 0;
        }
    }
}
