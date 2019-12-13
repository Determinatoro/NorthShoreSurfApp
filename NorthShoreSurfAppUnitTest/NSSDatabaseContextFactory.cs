using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NorthShoreSurfApp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NorthShoreSurfAppUnitTest
{
    public class NSSDatabaseContextFactory : IDisposable
    {
        private SqliteConnection connection;

        private DbContextOptions<NSSDatabaseContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<NSSDatabaseContext>()
                .UseSqlite(connection).Options;
        }

        public NSSDatabaseContext CreateContext()
        {
            if (connection == null)
            {
                var source = new SqliteConnection(@"DataSource=D:\OneDrive\Projects\C Sharp projects\NorthShoreSurfApp\NorthShoreSurfApp\NorthShoreSurfApp.Android\Assets\NSS.db");
                source.Open();

                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                source.BackupDatabase(connection);
                source.Close();

                var options = CreateOptions();
                using (var context = new NSSDatabaseContext(options))
                {
                    context.Database.EnsureCreated();
                    context.Database.Migrate();
                }
            }

            return new NSSDatabaseContext(CreateOptions());
        }

        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection = null;
            }
        }
    }
}
