using System.Threading.Tasks;
using Effort.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetFramework.Tests
{
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_Update_ByEntity_ShouldRemoveOne()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new BloggingContext(connection))
            {
                context.Blogs.Add(new Blog { BlogId = 1, Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { BlogId = 2, Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { BlogId = 3, Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection)))
                repo.Update(new Blog { BlogId = 1, Url = "http://sample.com/zebras" });

            // Use a separate instance of the context to verify correct data was saved to database
            using (BloggingContext context = new BloggingContext(connection))
            {
                Blog blog = context.Blogs.Find(1);
                Assert.IsTrue(blog.Url == "http://sample.com/zebras");
            }
        }

        [TestMethod]
        public async Task Repository_UpdateAsync_ByEntity_ShouldRemoveOne()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new BloggingContext(connection))
            {
                context.Blogs.Add(new Blog { BlogId = 1, Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { BlogId = 2, Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { BlogId = 3, Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection)))
                await repo.UpdateAsync(new Blog { BlogId = 1, Url = "http://sample.com/zebras" });

            // Use a separate instance of the context to verify correct data was saved to database
            using (BloggingContext context = new BloggingContext(connection))
            {
                Blog blog = await context.Blogs.FindAsync(1);
                Assert.IsTrue(blog.Url == "http://sample.com/zebras");
            }
        }
    }
}