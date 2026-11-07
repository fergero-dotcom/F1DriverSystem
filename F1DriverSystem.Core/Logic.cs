using System;
using System.Collections.Generic;
using System.Linq;
using F1DriverSystem.DataAccessLayer;

namespace F1DriverSystem.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Logic
    {
        private readonly IRepository<Driver> repository;

        /// <summary>
        /// 
        /// </summary>
        public Logic() : this(RepositoryFactory.Create<Driver>()) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public Logic(IRepository<Driver> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int GetNextAvailableId()
        {
            var used = new HashSet<int>(repository.ReadAll().Select(x => x.Id));
            int id = 1;
            while (used.Contains(id)) id++;
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="team"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        private bool IsValid(string name, string team, int points)
        {
            return !string.IsNullOrWhiteSpace(name) &&
                   !string.IsNullOrWhiteSpace(team) && points >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="team"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        public bool AddDriver(string name, string team, int points)
        {
            if (!IsValid(name, team, points)) return false;
            repository.Add(new Driver(GetNextAvailableId(), name.Trim(), team.Trim(), points));
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Driver> GetAllDrivers()
        {
            return repository.ReadAll().OrderBy(x => x.Id).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Driver FindById(int id)
        {
            return repository.ReadById(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteDriver(int id)
        {
            return repository.Delete(id);
        }

        /// <summary>
        /// mbcx
        /// </summary>
        /// <param name="id">ghfhng</param>
        /// <param name="name"></param>
        /// <param name="team"></param>
        /// <param name="points"></param>
        /// <returns>cb xvgf</returns>
        public bool UpdateDriver(int id, string name, string team, int points)
        {
            if (!IsValid(name, team, points) || id <= 0) return false;
            return repository.Update(new Driver(id, name.Trim(), team.Trim(), points));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        public List<Driver> GetDriversByTeam(string team)
        {
            if (string.IsNullOrWhiteSpace(team)) return new List<Driver>();
            return repository.ReadAll().Where(x =>
                string.Equals(x.Team, team.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public double CalculateAveragePoints()
        {
            var drivers = repository.ReadAll();
            return drivers.Count == 0 ? 0 : drivers.Average(x => (double)x.Points);
        }
    }
}
