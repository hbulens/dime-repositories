using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Effort.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.NetFramework.Tests
{
    [TestClass]
    public partial class RepositoryTests
    {
        [TestMethod]
        public void Repository_FindAll_Contains_ShouldFindMatches()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                context.SaveChanges();
            }

            IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection), new RepositoryConfiguration());
            IEnumerable<Blog> result = repo.FindAll(x => x.Url.Contains("cat"));
            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task Repository_FindAllAsync_Contains_ShouldFindMatches()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish" });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs" });
                await context.SaveChangesAsync();
            }

            IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection), new RepositoryConfiguration());
            IEnumerable<Blog> result = await repo.FindAllAsync(x => x.Url.Contains("cat"));
            Assert.AreEqual(2, result.Count());
        }


        [TestMethod]
        public async Task Repository_FindAllAsync_ShouldFindMatches()
        {
            EffortConnection connection = Effort.DbConnectionFactory.CreateTransient();

            using (BloggingContext context = new(connection))
            {
                context.Blogs.Add(new Blog { Url = "http://sample.com/cats", Category = new() { Name = "Pets" } });
                context.Blogs.Add(new Blog { Url = "http://sample.com/catfish", Category = new() { Name = "Pets" } });
                context.Blogs.Add(new Blog { Url = "http://sample.com/dogs", Category = new() { Name = "Pets" } });
                await context.SaveChangesAsync();
            }

            IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(connection), new RepositoryConfiguration());

            Expression<Func<Blog, object>>[] includes = { x => x.Category, x => x.Category.Name };
            IEnumerable<Blog> result = await repo.FindAllAsync(
                where: x => x.Url.Contains("cat"),
                orderBy: null,
                ascending: false,
                includes: includes.ToPropertyExpression());
            Assert.AreEqual(2, result.Count());
        }
    }

    internal static class EfIncludesExtensions
    {
        public static string[] ToPropertyExpression<T>(this Expression<Func<T, object>>[] items)
            => items != null && items.Any() ? items.Where(x => x != null).Select(x => x.GetPropertyName()).ToArray() : null;

        public static string[] ToPropertyExpression<T, TEntity>(this Expression<Func<T, object>>[] includes, Expression<Func<T, TEntity>> select = null)
            => includes != null && select == null && includes.Any() ? includes.Select(x => x.GetPropertyName()).ToArray() : null;

        public static string[] ToPropertyExpression(this string[] includes)
            => includes != null && includes.Any() ? includes.ToArray() : null;
    }
}