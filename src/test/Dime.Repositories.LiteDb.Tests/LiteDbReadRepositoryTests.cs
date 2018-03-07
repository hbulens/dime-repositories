using System;
using System.Threading.Tasks;
using Xunit;

namespace Dime.Repositories.LiteDb.Tests
{
    /// <summary>
    ///
    /// </summary>
    public class LiteDbReadRepositoryTests
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public LiteDbReadRepositoryTests()
        {
            this.Factory = new LiteDbRepositoryFactory(@"C:\Temp\MyData.db");
        }

        #endregion Constructor

        #region Properties

        private readonly IRepositoryFactory Factory;

        #endregion Properties

        #region Methods

        [Fact]
        public async Task FindAllPagedAsyncReturnNotNull()
        {
            using (IRepository<TestModel> repository = this.Factory.Create<TestModel>())
            {
                IPage<TestModel> pagedItems = await repository.FindAllPagedAsync((x) => x.Id > 0, null, 1, 10);
                Assert.NotNull(pagedItems);
            }
        }

        [Fact]
        public async Task FindAllPagedAsyncReturnNotEmpty()
        {
            using (IRepository<TestModel> repository = this.Factory.Create<TestModel>())
            {
                TestModel item = await repository.CreateAsync(new TestModel() { Id = new Random().Next(0, 10000000) });
                IPage<TestModel> pagedItems = await repository.FindAllPagedAsync((x) => x.Id > 0, null, 1, 10);
                Assert.NotEmpty(pagedItems.Data);
            }
        }

        #endregion Methods
    }
}