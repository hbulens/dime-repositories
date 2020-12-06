using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetCore.Tests
{
    [TestClass]
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_FindAll_Contains_ShouldFindMatches()
        {
            // In-memory database only exists while the connection is open
            using SqliteConnection connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                using (BloggingContext context = new BloggingContext(options))
                    context.Database.EnsureCreated();

                // Insert seed data into the database using one instance of the context
                using (BloggingContext context = new BloggingContext(options))
                {
                    context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                    context.SaveChanges();
                }

                using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options));
                IEnumerable<Blog> result = repo.FindAll(x => x.Url.Contains("cat"));
                Assert.AreEqual(2, result.Count());
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task Repository_FindAllAsync_Contains_ShouldFindMatches()
        {
            // In-memory database only exists while the connection is open
            await using SqliteConnection connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                // Create the schema in the database
                await using (BloggingContext context = new BloggingContext(options))
                    context.Database.EnsureCreated();

                // Insert seed data into the database using one instance of the context
                await using (BloggingContext context = new BloggingContext(options))
                {
                    context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                    context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                    context.SaveChanges();
                }

                using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options));
                IEnumerable<Blog> result = await repo.FindAllAsync(x => x.Url.Contains("cat"));
                Assert.AreEqual(2, result.Count());
            }
            finally
            {
                connection.Close();
            }
        }
    }
}