using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using Timebox.Schedule.Api.Controllers;
using Timebox.Schedule.Api.DTOs;
using Timebox.Schedule.Application.Exceptions;
using Timebox.Schedule.Application.Interfaces.Services;
using Timebox.Schedule.Domain.Entities;

namespace Timebox.Schedule.Api.Tests.ScheduleControllerTests
{
    [TestFixture]
    public class ScheduleTaskTests
    {
        private AutoMocker _mocker;
        private ScheduleController _sut;
        
        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker(MockBehavior.Loose);
            _sut = _mocker.CreateInstance<ScheduleController>();
        }

        #region NewTimebox
        [Test]
        public async Task ScheduleTask_NewTimebox_Success_ReturnsOk()
        {
            // Arrange
            Guid scheduleId = Guid.NewGuid();
            Guid timeboxId = Guid.NewGuid();
            Guid taskId = Guid.NewGuid();
            const int durationInMinutes = 15;
            DateTime scheduledDateTime = DateTime.Now;
            ScheduleTaskDto scheduleTaskDto = new ScheduleTaskDto
            {
                TaskId = taskId.ToString(),
                DurationInMinutes = durationInMinutes,
                ScheduledDateTime = scheduledDateTime
            };

            _mocker.GetMock<ISchedulerService>().Setup(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId.ToString()),
                    It.Is<int>(y => y == durationInMinutes),
                    It.Is<DateTime>(y => y == scheduledDateTime)))
                .ReturnsAsync(new Domain.Entities.Timebox(timeboxId, scheduleId, durationInMinutes, scheduledDateTime));
            
            _mocker.GetMock<ISchedulerService>().Setup(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId.ToString()))).ReturnsAsync(new Domain.Entities.Timebox(timeboxId, scheduleId, durationInMinutes, scheduledDateTime, new TaskBase(taskId, "task-name")));

            // Act
            var result = await _sut.ScheduleTask(scheduleId.ToString(), scheduleTaskDto);

            // Assert
            result.ShouldBeAssignableTo<OkObjectResult>();
            (result as OkObjectResult)?.Value.ShouldBeEquivalentTo(new TaskScheduledDto
            {
                ScheduleId = scheduleId.ToString(),
                TimeboxId = timeboxId.ToString(),
                Timebox = new TimeboxDto
                {
                    TimeboxId = timeboxId.ToString(),
                    TaskId = taskId.ToString(),
                    DurationInMinutes = durationInMinutes,
                    FromDateTime = scheduledDateTime
                },
                TaskId = taskId.ToString()
            });
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<int>(y => y == durationInMinutes),
                It.Is<DateTime>(y => y == scheduledDateTime)), Times.Once);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId.ToString())), Times.Once);
        }
        
        [Test]
        public async Task ScheduleTask_NewTimebox_TimeslotOverlaps_ReturnsConflict()
        {
            // Arrange
            const string scheduleId = "schedule-id";
            const string taskId = "task-id";
            const int durationInMinutes = 15;
            DateTime scheduledDateTime = DateTime.Now;
            const string existingTimeboxId = "existing-timebox-id";
            ScheduleTaskDto scheduleTaskDto = new ScheduleTaskDto
            {
                TaskId = taskId,
                DurationInMinutes = durationInMinutes,
                ScheduledDateTime = scheduledDateTime
            };

            _mocker.GetMock<ISchedulerService>().Setup(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId),
                    It.Is<int>(y => y == durationInMinutes),
                    It.Is<DateTime>(y => y == scheduledDateTime)))
                .ThrowsAsync(new TimeboxWouldOverlapException(existingTimeboxId));
            
            // Act
            var result = await _sut.ScheduleTask(scheduleId, scheduleTaskDto);

            // Assert
            result.ShouldBeOfType<ConflictObjectResult>();
            (result as ConflictObjectResult)?.Value.ShouldBe(existingTimeboxId);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId),
                It.Is<int>(y => y == durationInMinutes),
                It.Is<DateTime>(y => y == scheduledDateTime)), Times.Once);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.IsAny<string>(),
                It.IsAny<string>(), 
                It.IsAny<string>()), Times.Never);
        }
        
        [Test]
        public async Task ScheduleTask_NewTimebox_NotFound_ReturnsNotFound()
        {
            // Arrange
            Guid scheduleId = Guid.NewGuid();
            Guid timeboxId = Guid.NewGuid();
            const string taskId = "task-id";
            const int durationInMinutes = 15;
            DateTime scheduledDateTime = DateTime.Now;
            ScheduleTaskDto scheduleTaskDto = new ScheduleTaskDto
            {
                TaskId = taskId,
                DurationInMinutes = durationInMinutes,
                ScheduledDateTime = scheduledDateTime
            };
            
            _mocker.GetMock<ISchedulerService>().Setup(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId.ToString()),
                    It.Is<int>(y => y == durationInMinutes),
                    It.Is<DateTime>(y => y == scheduledDateTime)))
                .ReturnsAsync(new Domain.Entities.Timebox(timeboxId, scheduleId, durationInMinutes, scheduledDateTime));

            var resourceName = "resource-name";
            var resourceIdentifier = "resource-identifier";
           _mocker.GetMock<ISchedulerService>().Setup(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId))).ThrowsAsync(new NotFoundException(resourceName, resourceIdentifier));

            // Act
            var result = await _sut.ScheduleTask(scheduleId.ToString(), scheduleTaskDto);

            // Assert
            result.ShouldBeAssignableTo<NotFoundObjectResult>();
            (result as NotFoundObjectResult)?.Value.ShouldBeEquivalentTo(new [] {resourceName, resourceIdentifier});
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<int>(y => y == durationInMinutes),
                It.Is<DateTime>(y => y == scheduledDateTime)), Times.Once);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId)), Times.Once);
        }
        
        [Test]
        public async Task ScheduleTask_NewTimebox_InvalidGuid_ReturnsBadRequest()
        {
            // Arrange
            Guid scheduleId = Guid.NewGuid();
            Guid timeboxId = Guid.NewGuid();
            const string taskId = "task-id";
            const int durationInMinutes = 15;
            DateTime scheduledDateTime = DateTime.Now;
            ScheduleTaskDto scheduleTaskDto = new ScheduleTaskDto
            {
                TaskId = taskId,
                DurationInMinutes = durationInMinutes,
                ScheduledDateTime = scheduledDateTime
            };
            
            _mocker.GetMock<ISchedulerService>().Setup(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId.ToString()),
                    It.Is<int>(y => y == durationInMinutes),
                    It.Is<DateTime>(y => y == scheduledDateTime)))
                .ReturnsAsync(new Domain.Entities.Timebox(timeboxId, scheduleId, durationInMinutes, scheduledDateTime));
            
            var exceptionValue = new Dictionary<string, string>
            {
                {"test", "test"}
            };
            
            _mocker.GetMock<ISchedulerService>().Setup(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId))).ThrowsAsync(new InvalidParametersException(exceptionValue));

            // Act
            var result = await _sut.ScheduleTask(scheduleId.ToString(), scheduleTaskDto);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult)?.Value.ShouldBe(exceptionValue);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<int>(y => y == durationInMinutes),
                It.Is<DateTime>(y => y == scheduledDateTime)), Times.Once);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId)), Times.Once);
        }
        
        [Test]
        public async Task ScheduleTask_NewTimebox_UnknownExceptionAllocateTimebox_ReturnsInternalServerError()
        {
            // Arrange
            const string scheduleId = "schedule-id";
            Guid timeboxId = Guid.NewGuid();
            const string taskId = "task-id";
            const int durationInMinutes = 15;
            DateTime scheduledDateTime = DateTime.Now;
            ScheduleTaskDto scheduleTaskDto = new ScheduleTaskDto
            {
                TaskId = taskId,
                DurationInMinutes = durationInMinutes,
                ScheduledDateTime = scheduledDateTime
            };

            _mocker.GetMock<ISchedulerService>().Setup(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId),
                    It.Is<int>(y => y == durationInMinutes),
                    It.Is<DateTime>(y => y == scheduledDateTime)))
                .ThrowsAsync(new Exception());

            // Act
            var result = await _sut.ScheduleTask(scheduleId, scheduleTaskDto);

            // Assert
            result.ShouldBeAssignableTo<StatusCodeResult>();
            (result as StatusCodeResult)?.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId),
                It.Is<int>(y => y == durationInMinutes),
                It.Is<DateTime>(y => y == scheduledDateTime)), Times.Once);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.IsAny<string>(),
                It.IsAny<string>(), 
                It.IsAny<string>()), Times.Never);
        }
        
        [Test]
        public async Task ScheduleTask_NewTimebox_UnknownExceptionScheduleTask_ReturnsInternalServerError()
        {
            // Arrange
            Guid scheduleId = Guid.NewGuid();
            Guid timeboxId = Guid.NewGuid();
            const string taskId = "task-id";
            const int durationInMinutes = 15;
            DateTime scheduledDateTime = DateTime.Now;
            ScheduleTaskDto scheduleTaskDto = new ScheduleTaskDto
            {
                TaskId = taskId,
                DurationInMinutes = durationInMinutes,
                ScheduledDateTime = scheduledDateTime
            };

            _mocker.GetMock<ISchedulerService>().Setup(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId.ToString()),
                    It.Is<int>(y => y == durationInMinutes),
                    It.Is<DateTime>(y => y == scheduledDateTime)))
                .ReturnsAsync(new Domain.Entities.Timebox(timeboxId, scheduleId, durationInMinutes, scheduledDateTime));
            
            _mocker.GetMock<ISchedulerService>().Setup(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId))).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.ScheduleTask(scheduleId.ToString(), scheduleTaskDto);

            // Assert
            result.ShouldBeAssignableTo<StatusCodeResult>();
            (result as StatusCodeResult)?.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<int>(y => y == durationInMinutes),
                It.Is<DateTime>(y => y == scheduledDateTime)), Times.Once);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId)), Times.Once);
        }
        #endregion
        
        #region ExistingTimebox
        [Test]
        public async Task ScheduleTask_ExistingTimebox_Success_ReturnsOk()
        {
            // Arrange
            Guid scheduleId = Guid.NewGuid();
            Guid timeboxId = Guid.NewGuid();
            Guid taskId = Guid.NewGuid();
            const int durationInMinutes = 15;
            DateTime scheduledDateTime = DateTime.Now;
            
            _mocker.GetMock<ISchedulerService>().Setup(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId.ToString()))).ReturnsAsync(new Domain.Entities.Timebox(timeboxId, scheduleId, durationInMinutes, scheduledDateTime, new TaskBase(taskId, "task-name")));

            // Act
            var result = await _sut.ScheduleTask(scheduleId.ToString(), timeboxId.ToString(), taskId.ToString());

            // Assert
            result.ShouldBeAssignableTo<OkObjectResult>();
            (result as OkObjectResult)?.Value.ShouldBeEquivalentTo(new TaskScheduledDto
            {
                ScheduleId = scheduleId.ToString(),
                TimeboxId = timeboxId.ToString(),
                Timebox = new TimeboxDto
                {
                    TimeboxId = timeboxId.ToString(),
                    TaskId = taskId.ToString(),
                    DurationInMinutes = durationInMinutes,
                    FromDateTime = scheduledDateTime
                },
                TaskId = taskId.ToString()
            });
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>()), Times.Never);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId.ToString()),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId.ToString())), Times.Once);
        }
        
        [Test]
        public async Task ScheduleTask_ExistingTimebox_NotFound_ReturnsNotFound()
        {
            // Arrange
            const string scheduleId = "schedule-id";
            const string timeboxId = "timebox-id";
            const string taskId = "task-id";

            var resourceName = "resource-name";
            var resourceIdentifier = "resource-identifier";
           _mocker.GetMock<ISchedulerService>().Setup(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId),
                It.Is<string>(y => y == timeboxId), 
                It.Is<string>(y => y == taskId))).ThrowsAsync(new NotFoundException(resourceName, resourceIdentifier));

            // Act
            var result = await _sut.ScheduleTask(scheduleId, timeboxId, taskId);

            // Assert
            result.ShouldBeAssignableTo<NotFoundObjectResult>();
            (result as NotFoundObjectResult)?.Value.ShouldBe(new [] {resourceName, resourceIdentifier});
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>()), Times.Never);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId)), Times.Once);
        }
        
        [Test]
        public async Task ScheduleTask_ExistingTimebox_InvalidGuid_ReturnsBadRequest()
        {
            // Arrange
            const string scheduleId = "schedule-id";
            const string timeboxId = "timebox-id";
            const string taskId = "task-id";

            var exceptionValue = new Dictionary<string, string>
            {
                {"test", "test"}
            };
            _mocker.GetMock<ISchedulerService>().Setup(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId))).ThrowsAsync(new InvalidParametersException(exceptionValue));

            // Act
            var result = await _sut.ScheduleTask(scheduleId, timeboxId, taskId);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult)?.Value.ShouldBe(exceptionValue);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>()), Times.Never);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId)), Times.Once);
        }
        
        [Test]
        public async Task ScheduleTask_ExistingTimebox_UnknownException_ReturnsInternalServerError()
        {
            // Arrange
            const string scheduleId = "schedule-id";
            const string timeboxId = "timebox-id";
            const string taskId = "task-id";

            var exceptionValue = new Dictionary<string, string>
            {
                {"test", "test"}
            };
            _mocker.GetMock<ISchedulerService>().Setup(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId))).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.ScheduleTask(scheduleId, timeboxId, taskId);

            // Assert
            result.ShouldBeAssignableTo<StatusCodeResult>();
            (result as StatusCodeResult)?.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.AllocateTimebox(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<DateTime>()), Times.Never);
            
            _mocker.GetMock<ISchedulerService>().Verify(x => x.ScheduleTask(
                It.Is<string>(y => y == scheduleId),
                It.Is<string>(y => y == timeboxId.ToString()), 
                It.Is<string>(y => y == taskId)), Times.Once);
        }
        #endregion
    }
}