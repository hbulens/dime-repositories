using System.Threading.Tasks;
using Effort.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetFramework.Tests
{
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_Count_NoPredicate_ShouldCountAll()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new BloggingContext(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection));
            long result = repo.Count();
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public async Task Repository_CountAsync_NoPredicate_ShouldCountAll()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new BloggingContext(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection));
            long result = await repo.CountAsync();
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public async Task Repository_CountAsync_Predicate_ShouldCountCorrectly()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new BloggingContext(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection));
            long result = await repo.CountAsync(x => x.Url.Contains("cat"));
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void Repository_Count_Predicate_ShouldCountCorrectly()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();
            
            using (BloggingContext context = new BloggingContext(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            using (IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection)))
            {
                long result = repo.Count(x => x.Url.Contains("cat"));
                Assert.AreEqual(2, result);
            }
        }
    }
}