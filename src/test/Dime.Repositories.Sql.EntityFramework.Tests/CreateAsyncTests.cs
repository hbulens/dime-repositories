using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.Tests
{
    [TestClass]
    public partial class CreateAsyncTests
    {
        [TestMethod]
        public void Create_ShouldAddOne()
        {
            using TestDatabase testDb = new();

            // Run the test against one instance of the context
            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            repo.Create(new Blog { Url = "http://sample.com" });

            // Use a separate instance of the context to verify correct data was saved to database
            using BloggingContext context = new(testDb.Options);
            Assert.AreEqual(4, context.Blogs.Count());
            Assert.AreEqual("http://sample.com", context.Blogs.OrderByDescending(x => x.BlogId).First().Url);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldAddOne()
        {
            using TestDatabase testDb = new();

            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            await repo.CreateAsync(new Blog { Url = "http://sample.com" });

            // Use a separate instance of the context to verify correct data was saved to database
            await using BloggingContext context = new(testDb.Options);
            Assert.AreEqual(4, context.Blogs.Count());
            Assert.AreEqual("http://sample.com", context.Blogs.OrderByDescending(x => x.BlogId).First().Url);
        }
    }
}