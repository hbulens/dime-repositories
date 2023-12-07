using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.Sql.EntityFramework.Tests
{
    [TestClass]
    public partial class DeleteTests
    {
        [TestMethod]
        public void Delete_ByEntity_ShouldRemoveOne()
        {
            using TestDatabase testDb = new();

            using IRepository<Blog> repo = new EfRepository<Blog, BloggingContext>(new BloggingContext(testDb.Options));
            repo.Delete(new Blog { BlogId = 1 });

            using BloggingContext context = new(testDb.Options);
            Assert.AreEqual(2, context.Blogs.Count());
        }
    }
}