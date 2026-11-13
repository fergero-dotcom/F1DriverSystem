using F1DriverSystem.Models;
namespace F1DriverSystem.Core
{
    /// <summary>
    /// Представляет гонщика.
    /// </summary>
    public class Driver : IDomainObject
    {
        /// <summary>
        /// Идентификатор гонщика.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Идентификатор доменного объекта.
        /// </summary>
        int IDomainObject.ID { get { return Id; } set { Id = value; } }
        /// <summary>
        /// Имя гонщика.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Команда гонщика.
        /// </summary>
        public string Team { get; set; }
        /// <summary>
        /// Количество очков гонщика.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Создаёт новый экземпляр класса Driver.
        /// </summary>
        public Driver() { }

        /// <summary>
        /// Создаёт новый экземпляр класса Driver с указанными данными.
        /// </summary>
        /// <param name="id">Идентификатор гонщика.</param>
        /// <param name="name">Имя гонщика.</param>
        /// <param name="team">Команда гонщика.</param>
        /// <param name="points">Количество очков.</param>
        public Driver(int id, string name, string team, int points)
        {
            Id = id;
            Name = name;
            Team = team;
            Points = points;
        }
    }
}
