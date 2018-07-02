using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.LiteDb.Tests
{
    [TestClass]
    public class LiteDbReadRepositoryTests
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public LiteDbReadRepositoryTests()
        {
            Factory = new LiteDbRepositoryFactory(@"C:\Temp\MyData.db");
        }

        #endregion Constructor

        #region Properties

        private readonly IRepositoryFactory Factory;

        #endregion Properties

        #region Methods

        [TestMethod]
        public async Task FindAllPagedAsyncReturnNotNull()
        {
            using (IRepository<TestModel> repository = Factory.Create<TestModel>())
            {
                IPage<TestModel> pagedItems = await repository.FindAllPagedAsync((x) => x.Id > 0, null, 1, 10);
                Assert.IsNotNull(pagedItems);
            }
        }

        [TestMethod]
        public async Task FindAllPagedAsyncReturnNotEmpty()
        {
            using (IRepository<TestModel> repository = Factory.Create<TestModel>())
            {
                TestModel item = await repository.CreateAsync(new TestModel() { Id = new Random().Next(0, 10000000) });
                IPage<TestModel> pagedItems = await repository.FindAllPagedAsync((x) => x.Id > 0, null, 1, 10);
                Assert.IsNotNull(pagedItems.Data);
            }
        }

        #endregion Methods
    }
}