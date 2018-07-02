using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Dime.Repositories.LiteDb.Tests
{
    [TestClass]
    public class LiteDbCreateRepositoryTests
    {
        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public LiteDbCreateRepositoryTests()
        {
            Factory = new LiteDbRepositoryFactory(@"C:\Temp\MyData.db");
        }

        #endregion Constructor

        #region Properties

        private readonly IRepositoryFactory Factory;

        #endregion Properties

        #region Methods

        [TestMethod]
        public async Task CreateAsyncReturnsTrue()
        {
            using (IRepository<TestModel> repository = Factory.Create<TestModel>())
            {
                TestModel item = await repository.CreateAsync(new TestModel() { Id = new Random().Next(0, 10000000) });
                Assert.IsNotNull(item);
            }
        }

        #endregion Methods
    }
}