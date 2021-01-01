using System.Linq;
using System.Threading.Tasks;
using Effort.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetFramework.Tests
{
    [TestClass]
    public class PagedQueryRepositoryTests
    {
        [TestMethod]
        public async Task Repository_FindPagedAsync_Contains_ShouldFindMatches()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new BloggingContext(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/2" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/3" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/4" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/5" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/6" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/7" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/8" });

                await context.SaveChangesAsync();
            }

            IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection), new RepositoryConfiguration());
            Page<Blog> result = await repo.FindAllPagedAsync(
                where: x => x.Url.Contains("cat"),
                orderBy: null,
                count: null,
                page: 1,
                pageSize: 5,
                includes: null);

            Assert.AreEqual(2, result.Data.Count());
            Assert.AreEqual(10, result.Total);
        }
    }
}