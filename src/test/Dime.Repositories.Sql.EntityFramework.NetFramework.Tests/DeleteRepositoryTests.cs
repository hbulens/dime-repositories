using System.Linq;
using System.Threading.Tasks;
using Effort.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetFramework.Tests
{
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_Delete_ByEntity_ShouldRemoveOne()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new(connection))
            {
                context.Blogs.Add(new Blog { BlogId = 1, Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { BlogId = 2, Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { BlogId = 3, Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection)))
                repo.Delete(new Blog { BlogId = 1 });

            // Use a separate instance of the context to verify correct data was saved to database
            using (BloggingContext context = new(connection))
            {
                Assert.AreEqual(2, context.Blogs.Count());
            }
        }

        [TestMethod]
        public async Task Repository_DeleteAsync_ByEntity_ShouldRemoveOne()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new(connection))
            {
                context.Blogs.Add(new Blog { BlogId = 1, Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { BlogId = 2, Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { BlogId = 3, Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection)))
                await repo.DeleteAsync(new Blog { BlogId = 1 });

            // Use a separate instance of the context to verify correct data was saved to database
            using (BloggingContext context = new(connection))
            {
                Assert.AreEqual(2, context.Blogs.Count());
            }
        }
    }
}