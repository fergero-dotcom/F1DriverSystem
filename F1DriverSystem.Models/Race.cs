namespace F1DriverSystem.Models
{
    /// <summary>
    /// Представляет гонку.
    /// </summary>
    public class Race : IDomainObject
    {
        /// <summary>
        /// Идентификатор гонки.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Название гонки.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Страна проведения гонки.
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Количество кругов.
        /// </summary>
        public int Laps { get; set; }

        /// <summary>
        /// Идентификатор доменного объекта.
        /// </summary>
        int IDomainObject.ID { get { return Id; } set { Id = value; } }

        /// <summary>
        /// Создаёт новый экземпляр класса Race.
        /// </summary>
        public Race() { }

        /// <summary>
        /// Создаёт новый экземпляр класса Race с указанными данными.
        /// </summary>
        /// <param name="id">Идентификатор гонки.</param>
        /// <param name="name">Название гонки.</param>
        /// <param name="country">Страна проведения гонки.</param>
        /// <param name="laps">Количество кругов.</param>
        public Race(int id, string name, string country, int laps)
        {
            Id = id;
            Name = name;
            Country = country;
            Laps = laps;
        }
    }
}
