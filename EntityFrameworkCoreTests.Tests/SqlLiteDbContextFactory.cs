using System;
using System.Data.Common;
using EntityFrameworkCoreTests.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCoreTests.Tests
{
    /// <summary>
    /// Wrapper that handle the connection lifetime, the creation of the DbContext and the creation of the schema.
    /// </summary>
    public class SqlLiteDbContextFactory : IDisposable
    {
        private DbConnection _connection;

        private DbContextOptions<DataContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<DataContext>()
                .UseSqlite(_connection).Options;
        }

        public DataContext CreateContext()
        {
            if (_connection == null)
            {
                _connection = new SqliteConnection("DataSource=:memory:");    
                _connection.Open();

                var options = CreateOptions();
                using (var context = new DataContext(options))
                {
                    context.Database.EnsureCreated();
                }
            }

            return new DataContext(CreateOptions());
        }

        public void Dispose()
        {
            if (_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
        
    }
}