namespace F1DriverSystem.Models
{
    /// <summary>
    /// Определяет общий идентификатор для доменных объектов.
    /// </summary>
    public interface IDomainObject
    {
        /// <summary>
        /// Идентификатор доменного объекта.
        /// </summary>
        int ID { get; set; }
    }
}
