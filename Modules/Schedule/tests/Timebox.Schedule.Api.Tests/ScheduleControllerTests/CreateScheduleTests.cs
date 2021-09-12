using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using Timebox.Schedule.Api.Controllers;
using Timebox.Schedule.Api.DTOs;
using Timebox.Schedule.Application.Interfaces.Services;

namespace Timebox.Schedule.Api.Tests.ScheduleControllerTests
{
    [TestFixture]
    public class CreateScheduleTests
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
        public async Task CreateSchedule_Success_ReturnsCreated()
        {
            // Arrange
            Guid expectedGuid = Guid.NewGuid();
            string expectedName = "schedule-name";
            DateTime expectedDate = DateTime.Now;
            var expectedResult = new Domain.Entities.Schedule(expectedGuid, expectedName, expectedDate);
            _mocker.GetMock<IScheduleService>()
                    .Setup(x => x.CreateSchedule(It.Is<string>(y => y == expectedName), It.Is<DateTime>(y => y == expectedDate)))
                    .ReturnsAsync(expectedResult);
            
            // Act
            var result = await _sut.CreateSchedule(new CreateScheduleDto(expectedName, expectedDate));

            // Assert
            result.ShouldBeAssignableTo<CreatedResult>();
            var createdResult = result as CreatedResult;
            var createdResultValue = createdResult?.Value as ScheduleCreatedDto;
            createdResultValue?.Id.ShouldBe(expectedGuid);        
            createdResultValue?.Date.ShouldBe(expectedDate); 
            createdResult?.Location.ShouldBe(expectedGuid.ToString());
        }
        
        [Test]
        public async Task CreateSchedule_Exception_ReturnsInternalServerError()
        {
            // Arrange
            Guid expectedGuid = Guid.NewGuid();
            string expectedName = "schedule-name";
            DateTime expectedDate = DateTime.Now;
            var expectedResult = new Domain.Entities.Schedule(expectedGuid, expectedName, expectedDate);
            _mocker.GetMock<IScheduleService>()
                .Setup(x => x.CreateSchedule(It.Is<string>(y => y == expectedName), It.Is<DateTime>(y => y == expectedDate)))
                .ThrowsAsync(new Exception());
            
            // Act
            var result = await _sut.CreateSchedule(new CreateScheduleDto(expectedName, expectedDate));

            // Assert
            result.ShouldBeAssignableTo<StatusCodeResult>();
            (result as StatusCodeResult)?.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        }
    }
}