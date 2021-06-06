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
using Timebox.Schedule.Application.Exceptions;
using Timebox.Schedule.Application.Interfaces.Services;

namespace Timebox.Schedule.Api.Tests.ScheduleControllerTests
{
    [TestFixture]
    public class GetScheduleTests
    {
        private AutoMocker _mocker;
        private ScheduleController _sut;
        
        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker(MockBehavior.Loose);
            _sut = _mocker.CreateInstance<ScheduleController>();
        }

        [Test]
        public async Task GetSchedule_ScheduleExists_ReturnsOk()
        {
            // Arrange
            const string scheduleId = "schedule-id";
            var expectedId = Guid.NewGuid();
            _mocker.GetMock<IScheduleService>().Setup(scheduleService => 
                scheduleService.GetSchedule(It.Is<string>(id => id == scheduleId))).ReturnsAsync(new Domain.Entities.Schedule(expectedId, DateTime.Now));

            // Act
            var result = await _sut.GetSchedule(scheduleId);

            // Assert
            result.ShouldBeOfType<OkObjectResult>();
        }
        [Test]
        public async Task GetSchedule_InvalidGuid_ReturnsBadRequest()
        {
            // Arrange
            const string scheduleId = "schedule-id";
            
            var exceptionValue = new Dictionary<string, string>
            {
                {"test", "test"}
            };
            
            _mocker.GetMock<IScheduleService>().Setup(scheduleService => 
                scheduleService.GetSchedule(It.Is<string>(id => id == scheduleId))).Throws(new InvalidParametersException(exceptionValue));

            // Act
            var result = await _sut.GetSchedule(scheduleId);

            // Assert
            result.ShouldBeOfType<BadRequestObjectResult>();
            (result as BadRequestObjectResult)?.Value.ShouldBe(exceptionValue);
        }
        
        [Test]
        public async Task GetSchedule_ScheduleNotFound_ReturnsNotFound()
        {
            // Arrange
            const string scheduleId = "schedule-id";
            
            var resourceName = "resource-name";
            var resourceIdentifier = "resource-identifier";
            _mocker.GetMock<IScheduleService>().Setup(scheduleService => 
                scheduleService.GetSchedule(It.Is<string>(id => id == scheduleId))).Throws(new NotFoundException(resourceName, resourceIdentifier));

            // Act
            var result = await _sut.GetSchedule(scheduleId);

            // Assert
            result.ShouldBeAssignableTo<NotFoundObjectResult>();
            (result as NotFoundObjectResult)?.Value.ShouldBeEquivalentTo(new [] {resourceName, resourceIdentifier});
        }
        
        [Test]
        public async Task GetSchedule_UnknownException_ReturnsInternalServerError()
        {
            // Arrange
            const string scheduleId = "schedule-id";
            _mocker.GetMock<IScheduleService>().Setup(scheduleService => 
                scheduleService.GetSchedule(It.Is<string>(id => id == scheduleId))).Throws(new Exception());

            // Act
            var result = await _sut.GetSchedule(scheduleId);

            // Assert
            result.ShouldBeAssignableTo<StatusCodeResult>();
            (result as StatusCodeResult)?.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        }
    }
}