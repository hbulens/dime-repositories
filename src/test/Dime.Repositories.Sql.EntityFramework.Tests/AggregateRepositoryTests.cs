using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetCore.Tests
{
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_Count_NoPredicate_ShouldCountAll()
        {
            // In-memory database only exists while the connection is open
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            try
            {
                DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (BloggingContext context = new(options))
                    context.Database.EnsureCreated();

                // Insert seed data into the database using one instance of the context
                using (BloggingContext context = new(options))
                {
                    context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                    context.SaveChanges();
                }

                using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options));
                long result = repo.Count();
                Assert.AreEqual(3, result);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task Repository_CountAsync_NoPredicate_ShouldCountAll()
        {
            // In-memory database only exists while the connection is open
            await using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            try
            {
                DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                await using (BloggingContext context = new(options))
                    context.Database.EnsureCreated();

                // Insert seed data into the database using one instance of the context
                await using (BloggingContext context = new(options))
                {
                    context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                    context.SaveChanges();
                }

                using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options));
                long result = await repo.CountAsync();
                Assert.AreEqual(3, result);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task Repository_CountAsync_Predicate_ShouldCountCorrectly()
        {
            // In-memory database only exists while the connection is open
            await using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            try
            {
                DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                await using (BloggingContext context = new(options))
                {
                    context.Database.EnsureCreated();
                }

                // Insert seed data into the database using one instance of the context
                await using (BloggingContext context = new(options))
                {
                    context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                    context.SaveChanges();
                }

                using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options));
                long result = await repo.CountAsync(x => x.Url.Contains("cat"));
                Assert.AreEqual(2, result);
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public void Repository_Count_Predicate_ShouldCountCorrectly()
        {
            // In-memory database only exists while the connection is open
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            try
            {
                DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (BloggingContext context = new(options))
                {
                    context.Database.EnsureCreated();
                }

                // Insert seed data into the database using one instance of the context
                using (BloggingContext context = new(options))
                {
                    context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                    context.SaveChanges();
                }

                using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options));
                long result = repo.Count(x => x.Url.Contains("cat"));
                Assert.AreEqual(2, result);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}