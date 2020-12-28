using System.Collections.Generic;
using System.Linq;
using Dime.Repositories;
using Dime.Scheduler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Dime.Repositories.EF.Tests
{
    [TestClass]
    public class EfRepositoryGetSyncTests : RepositoryTests<SchedulerContext>
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
            repoMock.Setup(x => x.FindById(1)).Returns(data.ElementAt(0));
            repoMock.Setup(x => x.FindById(3)).Returns(data.ElementAt(1));
            repoMock.Setup(x => x.FindById(2)).Returns(data.ElementAt(2));
            repoMock.Setup(x => x.FindById(5)).Returns(noAppointment);

            repoMock.Setup(x => x.FindById(1, null)).Returns(data.ElementAt(0));
            repoMock.Setup(x => x.FindById(3, null)).Returns(data.ElementAt(1));
            repoMock.Setup(x => x.FindById(2, null)).Returns(data.ElementAt(2));
            repoMock.Setup(x => x.FindById(5, null)).Returns(noAppointment);

            repoMock.CallBase = true;
            _repository = repoMock.Object;
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void EfRepository_FindAll_ReturnsAll()
        {
            IEnumerable<Appointment> items = _repository.FindAll(includes: null);
            Assert.AreEqual(3, items.Count());
            Assert.AreEqual("Appointment 1", items.ElementAt(0).Subject);
            Assert.AreEqual("Appointment 2", items.ElementAt(1).Subject);
            Assert.AreEqual("Appointment 3", items.ElementAt(2).Subject);
        }

        [DataTestMethod]
        [DataRow("Appointment 1", true)]
        [DataRow("Appointment Null", false)]
        [TestCategory("Repository")]
        public void EfRepository_Exists(string subject, bool exists)
        {
            bool containsItem = _repository.Exists(x => x.Subject == subject);
            Assert.IsTrue(containsItem == exists);
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public void EfRepository_FindById(long id, bool exists)
        {
            Appointment item = _repository.FindById(id);
            Assert.IsTrue(item != null == exists);
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public void EfRepository_FindById_Includes(long id, bool exists)
        {
            Appointment item = _repository.FindById(id, null);
            Assert.IsTrue(item != null == exists);
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public void EfRepository_FindOne(long id, bool exists)
        {
            Appointment item = _repository.FindOne(x => x.Id == id);
            Assert.IsTrue(item != null == exists);
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public void EfRepository_FindOne_Includes(long id, bool exists)
        {
            Appointment item = _repository.FindOne(x => x.Id == id, includes: null);
            Assert.IsTrue(item != null == exists);
        }

        [DataTestMethod]
        [DataRow(1, true)]
        [DataRow(2, true)]
        [DataRow(5, false)]
        [TestCategory("Repository")]
        public void EfRepository_FindOne_Full(long id, bool exists)
        {
            Category item = _repository.FindOne(x => x.Id == id, x => new Category { Id = (int)x.Id }, null, false, null, null, null);
            Assert.IsTrue(exists ? item.Id == id : item == null);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void EfRepository_FindAll_Where()
        {
            IEnumerable<Appointment> items = _repository.FindAll(x => x.Task.Id == 1);
            Assert.IsTrue(items.Count() == 2);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void EfRepository_FindAll_Where_Includes()
        {
            IEnumerable<Appointment> items = _repository.FindAll(x => x.Task.Id == 1, includes: null);
            Assert.IsTrue(items != null);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void EfRepository_FindAll_Where_IncludeAll_Includes()
        {
            IEnumerable<Appointment> items = _repository.FindAll(x => x.Task.Id == 1, true, includes: null);
            Assert.IsTrue(items != null);
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void EfRepository_FindAll_Where_IncludeAll_Includes_Paged()
        {
            IEnumerable<Appointment> items = _repository.FindAll(x => x.Task.Id == 1, page: 1, pageSize: 2, includes: null);
            Assert.IsTrue(items != null);
        }
    }
}