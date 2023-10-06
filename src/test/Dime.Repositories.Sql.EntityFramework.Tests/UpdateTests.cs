using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.Tests
{
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Update_ByEntity_ShouldRemoveOne()
        {
            using TestDatabase testDb = new();

            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            repo.Update(new Blog { BlogId = 1, Url = "http://sample.com/zebras" });

            // Use a separate instance of the context to verify correct data was saved to database
            using BloggingContext context = new(testDb.Options);
            Blog blog = context.Blogs.Find(1);
            Assert.IsTrue(blog.Url == "http://sample.com/zebras");
        }

        [TestMethod]
        public async Task UpdateAsync_ByEntity_ShouldRemoveOne()
        {
            using TestDatabase testDb = new();

            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            await repo.UpdateAsync(new Blog { BlogId = 1, Url = "http://sample.com/zebras" });

            // Use a separate instance of the context to verify correct data was saved to database
            await using BloggingContext context = new(testDb.Options);
            Blog blog = await context.Blogs.FindAsync(1);
            Assert.IsTrue(blog.Url == "http://sample.com/zebras");
        }
    }
}