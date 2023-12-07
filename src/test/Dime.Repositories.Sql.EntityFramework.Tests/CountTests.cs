using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.Tests
{
    [TestClass]
    public partial class CountTests
    {
        [TestMethod]
        public void Count_NoPredicate_ShouldCountAll()
        {
            using TestDatabase testDb = new();

            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            long result = repo.Count();
            Assert.AreEqual(3, result);
        }

        [TestMethod]
        public void Count_Predicate_ShouldCountCorrectly()
        {
            using TestDatabase testDb = new();

            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            long result = repo.Count(x => x.Url.Contains("cat"));
            Assert.AreEqual(2, result);
        }
    }
}