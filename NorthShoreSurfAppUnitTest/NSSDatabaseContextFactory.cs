using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NorthShoreSurfApp;
using System;
using System.Collections.Generic;
using System.IO;
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
                var path = $@"DataSource={Utilities.GetProjectFolder()}\NSS.db";
                var source = new SqliteConnection(path);
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
