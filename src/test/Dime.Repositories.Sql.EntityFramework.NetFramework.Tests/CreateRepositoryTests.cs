using System.Linq;
using System.Threading.Tasks;
using Effort.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetFramework.Tests
{
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_Create_ShouldAddOne()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection)))
                repo.Create(new Blog { Url = "http://sample.com" });

            using BloggingContext context = new(connection);
            Assert.AreEqual(1, context.Blogs.Count());
            Assert.AreEqual("http://sample.com", context.Blogs.Single().Url);
        }

        [TestMethod]
        public async Task Repository_CreateAsync_ShouldAddOne()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection)))
                await repo.CreateAsync(new Blog { Url = "http://sample.com" });

            // Use a separate instance of the context to verify correct data was saved to database
            using BloggingContext context = new(connection);
            Assert.AreEqual(1, context.Blogs.Count());
            Assert.AreEqual("http://sample.com", context.Blogs.Single().Url);
        }
    }
}