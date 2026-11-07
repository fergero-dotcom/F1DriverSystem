using System;
using System.Collections.Generic;
using System.Linq;
using F1DriverSystem.DataAccessLayer;
using F1DriverSystem.Models;

namespace F1DriverSystem.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class RaceLogic
    {
        private readonly IRepository<Race> repository;

        /// <summary>
        /// 
        /// </summary>
        public RaceLogic() : this(RepositoryFactory.Create<Race>()) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public RaceLogic(IRepository<Race> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Race> GetAllRaces()
        {
            return repository.ReadAll().OrderBy(x => x.Id).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Race FindById(int id)
        {
            return repository.ReadById(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <param name="laps"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <param name="laps"></param>
        /// <returns></returns>
        public bool UpdateRace(int id, string name, string country, int laps)
        {
            if (!IsValid(name, country, laps) || id <= 0) return false;
            return repository.Update(new Race(id, name.Trim(), country.Trim(), laps));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteRace(int id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <param name="laps"></param>
        /// <returns></returns>
        private static bool IsValid(string name, string country, int laps)
        {
            return !string.IsNullOrWhiteSpace(name) &&
                   !string.IsNullOrWhiteSpace(country) && laps > 0;
        }
    }
}
