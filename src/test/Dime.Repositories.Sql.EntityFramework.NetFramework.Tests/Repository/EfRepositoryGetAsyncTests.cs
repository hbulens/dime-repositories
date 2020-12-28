using System.Collections.Generic;
using System.Linq;
using Dime.Repositories;
using Dime.Scheduler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using t = System.Threading.Tasks;

namespace Dime.Repositories.EF.Tests
{
    [TestClass]
    public class EfRepositoryGetAsyncTests : RepositoryTests<SchedulerContext>
    {
        private EfRepository<Appointment, SchedulerContext> _repository;

        [TestInitialize]
        public void Initialize()
        {
            IQueryable<Appointment> data = new List<Appointment>
            {
                new Appointment { Id = 1, Subject = "Appointment 1", Task = new Task { Id = 1 } },
                new Appointment { Id = 3, Subject = "Appointment 2", Task = new Task { Id = 1 } },
                new Appointment { Id = 2, Subject = "Appointment 3", Task = new Task { Id = 2 } },
            }.AsQueryable();

            Appointment noAppointment = null;
            Mock<EfRepository<Appointment, SchedulerContext>> repoMock = new Mock<EfRepository<Appointment, SchedulerContext>>(Setup(data));
            repoMock.Setup(x => x.FindByIdAsync(1)).Returns(t.Task.FromResult(data.ElementAt(0)));
            repoMock.Setup(x => x.FindByIdAsync(3)).Returns(t.Task.FromResult(data.ElementAt(1)));
            repoMock.Setup(x => x.FindByIdAsync(2)).Returns(t.Task.FromResult(data.ElementAt(2)));
            repoMock.Setup(x => x.FindByIdAsync(5)).Returns(t.Task.FromResult(noAppointment));

            repoMock.Setup(x => x.FindByIdAsync(1, null)).Returns(t.Task.FromResult(data.ElementAt(0)));
            repoMock.Setup(x => x.FindByIdAsync(3, null)).Returns(t.Task.FromResult(data.ElementAt(1)));
            repoMock.Setup(x => x.FindByIdAsync(2, null)).Returns(t.Task.FromResult(data.ElementAt(2)));
            repoMock.Setup(x => x.FindByIdAsync(5, null)).Returns(t.Task.FromResult(noAppointment));

            repoMock.Setup(x => x.ExistsAsync(y => y.Subject == "Appointment 1")).Returns(t.Task.FromResult(true));
            repoMock.Setup(x => x.ExistsAsync(y => y.Subject == "Appointment Null")).Returns(t.Task.FromResult(false));

            repoMock.CallBase = true;
            _repository = repoMock.Object;
        }

        [TestMethod]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindAllAsync_ReturnsAll()
        {
            IEnumerable<Appointment> items = await _repository.FindAllAsync(includes: null);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual("Appointment 1", items.ElementAt(0).Subject);
            Assert.AreEqual("Appointment 2", items.ElementAt(1).Subject);
            Assert.AreEqual("Appointment 3", items.ElementAt(2).Subject);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public async t.Task EfRepository_ExistsAsync_ContainsItem_ReturnsTrue()
            => Assert.IsTrue(await _repository.ExistsAsync(x => x.Subject == "Appointment 1"));

        [TestMethod]
        [TestCategory("Repository")]
        public async t.Task EfRepository_ExistsAsync_DoesNotContainItem_ReturnsFalse()
            => Assert.IsFalse(await _repository.ExistsAsync(x => x.Subject == "Appointment Null"));

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindById(long id, bool exists)
        {
            Appointment item = await _repository.FindByIdAsync(id);
            Assert.IsTrue(item != null == exists);
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindById_Includes(long id, bool exists)
        {
            Appointment item = await _repository.FindByIdAsync(id, null);
            Assert.IsTrue(item != null == exists);
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindOne(long id, bool exists)
        {
            Appointment item = await _repository.FindOneAsync(x => x.Id == id);
            Assert.IsTrue(item != null == exists);
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindOne_Includes(long id, bool exists)
        {
            Appointment item = await _repository.FindOneAsync(x => x.Id == id, includes: null);
            Assert.IsTrue(item != null == exists);
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindOne_Full(long id, bool exists)
        {
            Category item = await _repository.FindOneAsync(
                x => x.Id == id, x => new Category { Id = (int)x.Id }, null, false, null, null, null);

            Assert.IsTrue(exists ? item.Id == id : item == null);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindAllAsync_Where()
        {
            IEnumerable<Appointment> items = await _repository.FindAllAsync(x => x.Task.Id == 1, null);
            Assert.IsTrue(items.Count() == 2);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindAllAsync_Where_Includes()
        {
            IEnumerable<Appointment> items = await _repository.FindAllAsync(x => x.Task.Id == 1, includes: null);
            Assert.IsTrue(items != null);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindAllAsync_Where_IncludeAll_Includes()
        {
            IEnumerable<Appointment> items = await _repository.FindAllAsync(x => x.Task.Id == 1, includes: null);
            Assert.IsTrue(items != null);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public async t.Task EfRepository_FindAllAsync_Where_IncludeAll_Includes_Paged()
        {
            IEnumerable<Appointment> items = await _repository.FindAllAsync(x => x.Task.Id == 1, page: 1, pageSize: 2, includes: null);
            Assert.IsTrue(items != null);
        }
    }
}