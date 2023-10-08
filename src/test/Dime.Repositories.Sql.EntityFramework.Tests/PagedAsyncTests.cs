using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.Tests
{
    [TestClass]
    public partial class PagedAsyncTests
    {
        [TestMethod]
        public async Task FindAllPagedAsync_All_ShouldFindMatches()
        {
            using TestDatabase testDb = new();
            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            IPage<Blog> result = await repo.FindAllPagedAsync(null, null, 1, 2);
            Assert.AreEqual(3, result.Total);
            Assert.AreEqual(2, result.Data.Count());
        }

        [TestMethod]
        public async Task FindAllPagedAsync_Contains_ShouldFindMatches()
        {
            using TestDatabase testDb = new();
            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            IPage<Blog> result = await repo.FindAllPagedAsync(x => x.Url.Contains("cat"), null, 1, 1);
            Assert.AreEqual(2, result.Total);
            Assert.AreEqual(1, result.Data.Count());
        }
    }
}