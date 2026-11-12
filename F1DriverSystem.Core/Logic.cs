using System;
using System.Collections.Generic;
using System.Linq;
using F1DriverSystem.DataAccessLayer;

namespace F1DriverSystem.Core
{
    /// <summary>
    /// Бизнес-логика системы учёта гонщиков Формулы-1.
    /// Класс зависит только от абстракции IRepository, конкретная реализация
    /// внедряется IoC-контейнером (Ninject) через конструктор.
    /// </summary>
    public class Logic
    {
        private readonly IRepository<Driver> repository;

        /// <summary>
        /// Создаёт объект бизнес-логики с внедрённым репозиторием.
        /// </summary>
        /// <param name="repository">Репозиторий гонщиков, внедряемый DI-контейнером.</param>
        public Logic(IRepository<Driver> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// Находит минимальный свободный идентификатор.
        /// </summary>
        /// <returns>Первый незанятый положительный идентификатор.</returns>
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
        /// <param name="team">Название команды.</param>
        /// <param name="points">Количество очков.</param>
        /// <returns>true, если данные корректны.</returns>
        private bool IsValid(string name, string team, int points)
        {
            return !string.IsNullOrWhiteSpace(name) &&
                   !string.IsNullOrWhiteSpace(team) && points >= 0;
        }

        /// <summary>
        /// Добавляет нового гонщика.
        /// </summary>
        /// <param name="name">Имя гонщика.</param>
        /// <param name="team">Название команды.</param>
        /// <param name="points">Количество очков.</param>
        /// <returns>true, если гонщик успешно добавлен.</returns>
        public bool AddDriver(string name, string team, int points)
        {
            if (!IsValid(name, team, points)) return false;
            repository.Add(new Driver(GetNextAvailableId(), name.Trim(), team.Trim(), points));
            return true;
        }

        /// <summary>
        /// Возвращает список всех гонщиков, отсортированный по идентификатору.
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
        /// <returns>Гонщик или null, если он не найден.</returns>
        public Driver FindById(int id)
        {
            return repository.ReadById(id);
        }

        /// <summary>
        /// Удаляет гонщика по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор гонщика.</param>
        /// <returns>true, если гонщик удалён.</returns>
        public bool DeleteDriver(int id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// Обновляет данные гонщика.
        /// </summary>
        /// <param name="id">Идентификатор гонщика.</param>
        /// <param name="name">Новое имя.</param>
        /// <param name="team">Новая команда.</param>
        /// <param name="points">Новое количество очков.</param>
        /// <returns>true, если данные обновлены.</returns>
        public bool UpdateDriver(int id, string name, string team, int points)
        {
            if (!IsValid(name, team, points) || id <= 0) return false;
            return repository.Update(new Driver(id, name.Trim(), team.Trim(), points));
        }

        /// <summary>
        /// Возвращает гонщиков указанной команды.
        /// </summary>
        /// <param name="team">Название команды.</param>
        /// <returns>Список гонщиков команды.</returns>
        public List<Driver> GetDriversByTeam(string team)
        {
            if (string.IsNullOrWhiteSpace(team)) return new List<Driver>();
            return repository.ReadAll().Where(x =>
                string.Equals(x.Team, team.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// Вычисляет среднее количество очков по всем гонщикам.
        /// </summary>
        /// <returns>Среднее количество очков (0, если гонщиков нет).</returns>
        public double CalculateAveragePoints()
        {
            var drivers = repository.ReadAll();
            return drivers.Count == 0 ? 0 : drivers.Average(x => (double)x.Points);
        }
    }
}
