using F1DriverSystem.Models;
namespace F1DriverSystem.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Driver : IDomainObject
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int IDomainObject.ID { get { return Id; } set { Id = value; } }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Team { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Driver() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="team"></param>
        /// <param name="points"></param>
        public Driver(int id, string name, string team, int points)
        {
            Id = id;
            Name = name;
            Team = team;
            Points = points;
        }
    }
}
