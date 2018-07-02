using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetCore.Tests
{
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_Create_ShouldAddOne()
        {
            // In-memory database only exists while the connection is open
            using (SqliteConnection connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                try
                {
                    DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                        .UseSqlite(connection)
                        .Options;

                    // Create the schema in the database
                    using (BloggingContext context = new BloggingContext(options))
                        context.Database.EnsureCreated();

                    // Run the test against one instance of the context
                    using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options)))
                        repo.Create(new Blog() { Url = "http://sample.com" });

                    // Use a separate instance of the context to verify correct data was saved to database
                    using (BloggingContext context = new BloggingContext(options))
                    {
                        Assert.AreEqual(1, context.Blogs.Count());
                        Assert.AreEqual("http://sample.com", context.Blogs.Single().Url);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        [TestMethod]
        public async Task Repository_CreateAsync_ShouldAddOne()
        {
            // In-memory database only exists while the connection is open
            using (SqliteConnection connection = new SqliteConnection("DataSource=:memory:"))
            {
                connection.Open();

                try
                {
                    DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                        .UseSqlite(connection)
                        .Options;

                    // Create the schema in the database
                    using (BloggingContext context = new BloggingContext(options))
                    {
                        context.Database.EnsureCreated();
                    }

                    using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options)))
                        await repo.CreateAsync(new Blog() { Url = "http://sample.com" });

                    // Use a separate instance of the context to verify correct data was saved to database
                    using (BloggingContext context = new BloggingContext(options))
                    {
                        Assert.AreEqual(1, context.Blogs.Count());
                        Assert.AreEqual("http://sample.com", context.Blogs.Single().Url);
                    }
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}