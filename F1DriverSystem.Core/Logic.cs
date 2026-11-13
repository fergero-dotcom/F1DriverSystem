using System;
using System.Collections.Generic;
using System.Linq;
using F1DriverSystem.DataAccessLayer;

namespace F1DriverSystem.Core
{
    /// <summary>
    /// Управляет данными гонщиков.
    /// </summary>
    public class Logic
    {
        private readonly IRepository<Driver> repository;

        /// <summary>
        /// Создаёт новый экземпляр класса Logic.
        /// </summary>
        public Logic() : this(RepositoryFactory.Create<Driver>()) { }

        /// <summary>
        /// Создаёт новый экземпляр класса Logic с указанным репозиторием.
        /// </summary>
        /// <param name="repository">Репозиторий гонщиков.</param>
        public Logic(IRepository<Driver> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Возвращает следующий доступный идентификатор гонщика.
        /// </summary>
        /// <returns>Следующий доступный идентификатор.</returns>
        private int GetNextAvailableId()
        {
            var used = new HashSet<int>(repository.ReadAll().Select(x => x.Id));
            int id = 1;
            while (used.Contains(id)) id++;
            return id;
        }

        /// <summary>
        /// Проверяет корректность данных гонщика.
        /// </summary>
        /// <param name="name">Имя гонщика.</param>
        /// <param name="team">Команда гонщика.</param>
        /// <param name="points">Количество очков.</param>
        /// <returns>true, если данные корректны; иначе false.</returns>
        private bool IsValid(string name, string team, int points)
        {
            return !string.IsNullOrWhiteSpace(name) &&
                   !string.IsNullOrWhiteSpace(team) && points >= 0;
        }

        /// <summary>
        /// Добавляет нового гонщика.
        /// </summary>
        /// <param name="name">Имя гонщика.</param>
        /// <param name="team">Команда гонщика.</param>
        /// <param name="points">Количество очков.</param>
        /// <returns>true, если гонщик добавлен; иначе false.</returns>
        public bool AddDriver(string name, string team, int points)
        {
            if (!IsValid(name, team, points)) return false;
            repository.Add(new Driver(GetNextAvailableId(), name.Trim(), team.Trim(), points));
            return true;
        }

        /// <summary>
        /// Возвращает всех гонщиков, отсортированных по идентификатору.
        /// </summary>
        /// <returns>Список гонщиков.</returns>
        public List<Driver> GetAllDrivers()
        {
            return repository.ReadAll().OrderBy(x => x.Id).ToList();
        }

        /// <summary>
        /// Находит гонщика по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор гонщика.</param>
        /// <returns>Найденный гонщик или null.</returns>
        public Driver FindById(int id)
        {
            return repository.ReadById(id);
        }

        /// <summary>
        /// Удаляет гонщика по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор гонщика.</param>
        /// <returns>true, если гонщик удалён; иначе false.</returns>
        public bool DeleteDriver(int id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Обновляет данные существующего гонщика.
        /// </summary>
        /// <param name="id">Идентификатор гонщика.</param>
        /// <param name="name">Новое имя гонщика.</param>
        /// <param name="team">Новая команда гонщика.</param>
        /// <param name="points">Новое количество очков.</param>
        /// <returns>true, если гонщик обновлён; иначе false.</returns>
        public bool UpdateDriver(int id, string name, string team, int points)
        {
            if (!IsValid(name, team, points) || id <= 0) return false;
            return repository.Update(new Driver(id, name.Trim(), team.Trim(), points));
        }

        /// <summary>
        /// Возвращает гонщиков указанной команды.
        /// </summary>
        /// <param name="team">Название команды.</param>
        /// <returns>Список гонщиков.</returns>
        public List<Driver> GetDriversByTeam(string team)
        {
            if (string.IsNullOrWhiteSpace(team)) return new List<Driver>();
            return repository.ReadAll().Where(x =>
                string.Equals(x.Team, team.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Вычисляет среднее количество очков гонщиков.
        /// </summary>
        /// <returns>Среднее количество очков.</returns>
        public double CalculateAveragePoints()
        {
            var drivers = repository.ReadAll();
            return drivers.Count == 0 ? 0 : drivers.Average(x => (double)x.Points);
        }
    }
}
