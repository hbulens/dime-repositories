using System;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Dime.Repositories.Sql.EntityFramework.Tests
{
    internal class TestDatabase : IDisposable
    {        
        internal TestDatabase()
        {
            // In-memory database only exists while the connection is open
            Connection = new("DataSource=:memory:");
            Connection.Open();

            Options = new DbContextOptionsBuilder<BloggingContext>().UseSqlite(Connection).Options;

            CreateDatabase();
        }

        internal SqliteConnection Connection { get; private set; }

        internal DbContextOptions<BloggingContext> Options { get; private set; }   
        
        internal void CreateDatabase()
        {
            // Create the schema in the database
            using (BloggingContext context = new(Options))
                context.Database.EnsureCreated();

            // Insert seed data into the database using one instance of the context
            using (BloggingContext context = new(Options))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}