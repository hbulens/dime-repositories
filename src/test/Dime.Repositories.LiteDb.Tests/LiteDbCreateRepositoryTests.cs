using System;
using System.Threading.Tasks;
using Xunit;

namespace Dime.Repositories.LiteDb.Tests
{
    /// <summary>
    ///
    /// </summary>
    public class LiteDbCreateRepositoryTests
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public LiteDbCreateRepositoryTests()
        {
            this.Factory = new LiteDbRepositoryFactory(@"C:\Temp\MyData.db");
        }

        #endregion Constructor

        #region Properties

        private readonly IRepositoryFactory Factory;

        #endregion Properties

        #region Methods

        [Fact]
        public async Task CreateAsyncReturnsTrue()
        {
            using (IRepository<TestModel> repository = this.Factory.Create<TestModel>())
            {
                TestModel item = await repository.CreateAsync(new TestModel() { Id = new Random().Next(0, 10000000) });
                Assert.NotNull(item);
            }
        }

        #endregion Methods
    }
}