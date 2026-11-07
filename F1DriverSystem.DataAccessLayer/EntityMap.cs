using System;
using F1DriverSystem.Core;
using F1DriverSystem.Models;

namespace F1DriverSystem.DataAccessLayer
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class EntityMap
    {
        /// <summary>
        /// 
        /// </summary>
        public string Table { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Columns { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Parameters { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string Assignments { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columns"></param>
        /// <param name="parameters"></param>
        /// <param name="assignments"></param>
        private EntityMap(string table, string columns, string parameters, string assignments)
        {
            Table = table;
            Columns = columns;
            Parameters = parameters;
            Assignments = assignments;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static EntityMap For<T>()
        {
            if (typeof(T) == typeof(Driver))
                return new EntityMap("[dbo].[Drivers]", "[Id], [Name], [Team], [Points]",
                    "@Id, @Name, @Team, @Points", "[Name]=@Name, [Team]=@Team, [Points]=@Points");
            if (typeof(T) == typeof(Race))
                return new EntityMap("[dbo].[Races]", "[Id], [Name], [Country], [Laps]",
                    "@Id, @Name, @Country, @Laps", "[Name]=@Name, [Country]=@Country, [Laps]=@Laps");
            throw new NotSupportedException("Не настроено отображение сущности " + typeof(T).Name);
        }
    }
}
