using System.Data.Entity;
using System.Linq;
using Moq;

namespace Dime.Repositories.EF.Tests
{
    public abstract class RepositoryTests<TContext> where TContext : DbContext
    {
        public TContext Setup<T>(IQueryable<T> data) where T : class
        {
            Mock<DbSet<T>> mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(data.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(data.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mockSet.Setup(x => x.AsNoTracking()).Returns(mockSet.Object);

            Mock<TContext> mockContext = new Mock<TContext>();
            mockContext.Setup(c => c.Set<T>()).Returns(mockSet.Object);
            return mockContext.Object;
        }
    }
}