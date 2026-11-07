namespace F1DriverSystem.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Race : IDomainObject
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Laps { get; set; }

        /// <summary>
        /// 
        /// </summary>
        int IDomainObject.ID { get { return Id; } set { Id = value; } }

        /// <summary>
        /// 
        /// </summary>
        public Race() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <param name="laps"></param>
        public Race(int id, string name, string country, int laps)
        {
            Id = id;
            Name = name;
            Country = country;
            Laps = laps;
        }
    }
}
