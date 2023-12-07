using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.Tests
{
    [TestClass]
    public partial class GetTests
    {
        [TestMethod]
        public void FindAll_Contains_ShouldFindMatches()
        {
            using TestDatabase testDb = new();
            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            IEnumerable<Blog> result = repo.FindAll(x => x.Url.Contains("cat"));
            Assert.AreEqual(2, result.Count());
        }
    }
}