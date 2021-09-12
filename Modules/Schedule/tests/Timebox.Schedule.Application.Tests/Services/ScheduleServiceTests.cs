using System;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using Timebox.Schedule.Application.Exceptions;
using Timebox.Schedule.Application.Interfaces.Repositories;
using Timebox.Schedule.Application.Services;
using Timebox.Schedule.Domain.Entities;

namespace Timebox.Schedule.Application.Tests.Services
{
    [TestFixture]
    public class ScheduleServiceTests
    {
        private AutoMocker _mocker;
        private ScheduleService _sut;
        
        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker(MockBehavior.Loose);
            _sut = _mocker.CreateInstance<ScheduleService>();
        }

        [Test]
        public async Task CreateSchedule_Success_ReturnsCreated()
        {
            // Arrange
            var scheduleName = "schedule-name";
            var today = DateTime.Now;
            
            // Act
            var schedule = await _sut.CreateSchedule(scheduleName, today);

            // Assert
            schedule.ShouldNotBeNull();
            schedule.Date.ShouldBe(today);
            schedule.Timeboxes.ShouldBeEmpty();
            _mocker.GetMock<IScheduleRepository>().Verify(x => x.Add(It.Is<ISchedule>(y => y == schedule)));
        }
        
        [Test]
        public async Task GetSchedule_Success_ReturnsRepositoryValue()
        {
            // Arrange
            var scheduleId = Guid.NewGuid();
            var scheduleName = "schedule-name";
            var schedule = new Domain.Entities.Schedule(scheduleId, scheduleName, DateTime.Now);
            _mocker.GetMock<IScheduleRepository>().Setup(x => x.Get(It.Is<Guid>(y => y == scheduleId)))
                .ReturnsAsync(schedule);
            
            // Act
            var returnedValue = await _sut.GetSchedule(scheduleId.ToString());

            // Assert
            returnedValue.ShouldBe(schedule);
            schedule.Timeboxes.ShouldBeEmpty();
        }
        
        [Test]
        public async Task GetSchedule_NotFound_ThrowsNotFoundException()
        {
            // Arrange
            var scheduleId = Guid.NewGuid();
            _mocker.GetMock<IScheduleRepository>().Setup(x => x.Get(It.Is<Guid>(y => y == scheduleId)))
                .ReturnsAsync(null as ISchedule);
            
            // Act & Assert
            Func<Task<ISchedule>> testInvocation = async () => await _sut.GetSchedule(scheduleId.ToString());
            await testInvocation.ShouldThrowAsync<NotFoundException>();
        }
        
        [Test]
        public async Task GetSchedule_InvalidId_ThrowsInvalidParametersException()
        {
            // Arrange
            var scheduleId = "Guid.NewGuid()";
            
            // Act & Assert
            Func<Task<ISchedule>> testInvocation = async () => await _sut.GetSchedule(scheduleId);
            var exception = await testInvocation.ShouldThrowAsync<InvalidParametersException>();
            exception.InvalidParameters.ShouldContainKey("id");
        }
    }
}