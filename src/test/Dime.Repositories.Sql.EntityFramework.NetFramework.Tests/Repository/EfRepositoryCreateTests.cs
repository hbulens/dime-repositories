using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dime.Scheduler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Task = Dime.Scheduler.Models.Task;

namespace Dime.Repositories.EF.Tests
{
    [TestClass]
    public class EfRepositoryCreateTests : RepositoryTests<SchedulerContext>
    {
        private EfRepository<Appointment, SchedulerContext> _repository;

        private readonly Appointment _newAppointment = new Appointment { Id = 5, Subject = "Appointment 5", Task = new Task { Id = 2 } };

        [TestInitialize]
        public void Initialize()
        {
            IQueryable<Appointment> data = new List<Appointment>
            {
                new Appointment {Id = 1, Subject = "Appointment 1", Task = new Task {Id = 1}},
                new Appointment {Id = 3, Subject = "Appointment 2", Task = new Task {Id = 1}},
                new Appointment {Id = 2, Subject = "Appointment 3", Task = new Task {Id = 2}},
            }.AsQueryable();

            Mock<EfRepository<Appointment, SchedulerContext>> repoMock = new Mock<EfRepository<Appointment, SchedulerContext>>(Setup(data));
            repoMock.Setup(x => x.FindById(1)).Returns(data.ElementAt(0));
            repoMock.Setup(x => x.FindById(3)).Returns(data.ElementAt(1));
            repoMock.Setup(x => x.FindById(2)).Returns(data.ElementAt(2));

            repoMock.Setup(x => x.Create(_newAppointment)).Returns(_newAppointment);
            repoMock.Setup(x => x.FindById(5)).Returns(_newAppointment);

            repoMock.CallBase = true;
            _repository = repoMock.Object;
        }

        [TestMethod]
        [TestCategory("Repository")]
        public void EfRepository_Create_AddsOne()
        {
            Appointment item = _repository.Create(_newAppointment);
            Appointment newAppointment = _repository.FindById(5);
            Assert.AreEqual(newAppointment.Id, item.Id);
        }
    }
}