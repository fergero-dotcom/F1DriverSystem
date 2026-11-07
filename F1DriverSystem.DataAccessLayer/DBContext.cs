using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using F1DriverSystem.Core;
using F1DriverSystem.Models;

namespace F1DriverSystem.DataAccessLayer
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlServerConfiguration : DbConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public SqlServerConfiguration()
        {
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [DbConfigurationType(typeof(SqlServerConfiguration))]
    public class DBContext : DbContext
    {
        static DBContext()
        {
            Database.SetInitializer<DBContext>(null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public DBContext(string connectionString) : base(connectionString) { }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Driver> Drivers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Race> Races { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Driver>().ToTable("Drivers", "dbo").HasKey(x => x.Id);
            modelBuilder.Entity<Driver>().Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Driver>().Property(x => x.Name).IsRequired().IsMaxLength();
            modelBuilder.Entity<Driver>().Property(x => x.Team).IsRequired().IsMaxLength();

            modelBuilder.Entity<Race>().ToTable("Races", "dbo").HasKey(x => x.Id);
            modelBuilder.Entity<Race>().Property(x => x.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Race>().Property(x => x.Name).IsRequired().IsMaxLength();
            modelBuilder.Entity<Race>().Property(x => x.Country).IsRequired().IsMaxLength();
            base.OnModelCreating(modelBuilder);
        }
    }
}
