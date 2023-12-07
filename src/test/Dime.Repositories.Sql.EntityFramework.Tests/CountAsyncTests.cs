using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.Tests
{
    [TestClass]
    public partial class CountAsyncTests
    {
        [TestMethod]
        public async Task CountAsync_NoPredicate_ShouldCountAll()
        {
            using TestDatabase testDb = new();

            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            long result = await repo.CountAsync();
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public async Task CountAsync_Predicate_ShouldCountCorrectly()
        {
            using TestDatabase testDb = new();

            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            long result = await repo.CountAsync(x => x.Url.Contains("cat"));
            Assert.AreEqual(2, result);
        }
    }
}