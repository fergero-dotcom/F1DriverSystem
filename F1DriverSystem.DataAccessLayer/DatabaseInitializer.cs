using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace F1DriverSystem.DataAccessLayer
{
    /// <summary>
    /// 
    /// </summary>
    public static class DatabaseInitializer
    {
        private static readonly object Sync = new object();
        private static readonly HashSet<string> Initialized = new HashSet<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public static void EnsureCreated(string connectionString)
        {
            lock (Sync)
            {
                if (Initialized.Contains(connectionString)) return;
                var builder = new SqlConnectionStringBuilder(connectionString);
                if (string.IsNullOrWhiteSpace(builder.InitialCatalog) ||
                    !string.IsNullOrWhiteSpace(builder.AttachDBFilename))
                    throw new ArgumentException("Укажите Initial Catalog без AttachDBFilename.");

                using (var connection = new SqlConnection(connectionString))
                {
                    try { connection.Open(); }
                    catch (SqlException ex) when (ex.Number == 4060)
                    {
                        CreateDatabase(builder);
                        SqlConnection.ClearPool(connection);
                        connection.Open();
                    }

                    using (var transaction = connection.BeginTransaction())
                    using (var command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = @"
                         DECLARE @result int;
                         EXEC @result = sys.sp_getapplock @Resource=N'F1DriverSystemLab2.Schema',
                         @LockMode=N'Exclusive', @LockOwner=N'Transaction', @LockTimeout=30000;
                         IF @result < 0 THROW 50001, N'Не удалось получить блокировку схемы.', 1;";
                        command.ExecuteNonQuery();
                        using (var stream = typeof(DatabaseInitializer).Assembly.GetManifestResourceStream(
                            "F1DriverSystem.DataAccessLayer.Database.sql"))
                        using (var reader = new StreamReader(stream))
                            command.CommandText = reader.ReadToEnd();
                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                }
                Initialized.Add(connectionString);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        private static void CreateDatabase(SqlConnectionStringBuilder builder)
        {
            string databaseName = builder.InitialCatalog;
            builder.InitialCatalog = "master";
            using (var connection = new SqlConnection(builder.ConnectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = @"
                 IF DB_ID(@name) IS NULL
                 BEGIN
                 DECLARE @sql nvarchar(max) = N'CREATE DATABASE ' + QUOTENAME(@name);
                 EXEC (@sql);
                 END";
                command.Parameters.Add("@name", SqlDbType.NVarChar, 128).Value = databaseName;
                try { command.ExecuteNonQuery(); }
                catch (SqlException ex) when (ex.Number == 1801)
                {
                }
            }
        }
    }
}
