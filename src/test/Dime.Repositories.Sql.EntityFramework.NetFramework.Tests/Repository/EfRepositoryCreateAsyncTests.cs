using System.Collections.Generic;
using System.Linq;
using Dime.Scheduler.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using t = System.Threading.Tasks;

namespace Dime.Repositories.EF.Tests
{
    [TestClass]
    public class EfRepositoryCreateAsyncTests : RepositoryTests<SchedulerContext>
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
            repoMock.Setup(x => x.FindByIdAsync(1)).Returns(t.Task.FromResult(data.ElementAt(0)));
            repoMock.Setup(x => x.FindByIdAsync(3)).Returns(t.Task.FromResult(data.ElementAt(1)));
            repoMock.Setup(x => x.FindByIdAsync(2)).Returns(t.Task.FromResult(data.ElementAt(2)));

            repoMock.Setup(x => x.CreateAsync(_newAppointment)).Returns(t.Task.FromResult(_newAppointment));
            repoMock.Setup(x => x.FindByIdAsync(5)).Returns(t.Task.FromResult(_newAppointment));

            repoMock.CallBase = true;
            _repository = repoMock.Object;
        }

        [TestMethod]
        [TestCategory("Repository")]
        public async t.Task EfRepository_CreateAsync_AddsOne()
        {
            Appointment item = await _repository.CreateAsync(_newAppointment);
            Appointment newAppointment = await _repository.FindByIdAsync(5);
            Assert.AreEqual(newAppointment.Id, item.Id);
        }
    }
}