using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetCore.Tests
{
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_Update_ByEntity_ShouldRemoveOne()
        {
            // In-memory database only exists while the connection is open
            using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            try
            {
                DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                using (BloggingContext context = new(options))
                {
                    context.Database.EnsureCreated();

                    context.Blogs.Add(new Blog { BlogId = 1, Url = "http://sample.com/cats" });
                    context.Blogs.Add(new Blog { BlogId = 2, Url = "http://sample.com/catfish" });
                    context.Blogs.Add(new Blog { BlogId = 3, Url = "http://sample.com/dogs" });
                    context.SaveChanges();
                }

                using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options)))
                    repo.Update(new Blog { BlogId = 1, Url = "http://sample.com/zebras" });

                // Use a separate instance of the context to verify correct data was saved to database
                using (BloggingContext context = new(options))
                {
                    Blog blog = context.Blogs.Find(1);
                    Assert.IsTrue(blog.Url == "http://sample.com/zebras");
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [TestMethod]
        public async Task Repository_UpdateAsync_ByEntity_ShouldRemoveOne()
        {
            // In-memory database only exists while the connection is open
            await using SqliteConnection connection = new("DataSource=:memory:");
            connection.Open();

            try
            {
                DbContextOptions<BloggingContext> options = new DbContextOptionsBuilder<BloggingContext>()
                    .UseSqlite(connection)
                    .Options;

                await using (BloggingContext context = new(options))
                {
                    context.Database.EnsureCreated();

                    context.Blogs.Add(new Blog { BlogId = 1, Url = "http://sample.com/cats" });
                    context.Blogs.Add(new Blog { BlogId = 2, Url = "http://sample.com/catfish" });
                    context.Blogs.Add(new Blog { BlogId = 3, Url = "http://sample.com/dogs" });
                    context.SaveChanges();
                }

                using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(options)))
                    await repo.UpdateAsync(new Blog { BlogId = 1, Url = "http://sample.com/zebras" });

                // Use a separate instance of the context to verify correct data was saved to database
                await using (BloggingContext context = new(options))
                {
                    Blog blog = await context.Blogs.FindAsync(1);
                    Assert.IsTrue(blog.Url == "http://sample.com/zebras");
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}