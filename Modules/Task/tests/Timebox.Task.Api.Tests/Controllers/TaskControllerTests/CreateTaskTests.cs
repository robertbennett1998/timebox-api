using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using Timebox.Task.Api.Controllers;
using Timebox.Task.Api.DTOs;
using Timebox.Task.Application;
using Timebox.Task.Application.Exceptions;
using Timebox.Task.Application.Interfaces.Services;
using Timebox.Task.Application.Services;

namespace Timebox.Task.Api.Tests.Controllers.TaskControllerTests
{
    [TestFixture]
    public class CreateTaskTests
    {
        private AutoMocker _mocker;
        private TaskController _sut;
        
        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker(MockBehavior.Loose);
            _sut = _mocker.CreateInstance<TaskController>();
        }

        [Test]
        public async System.Threading.Tasks.Task CreateTask_Success_ReturnCreated()
        {
            // Arrange
            Guid taskId = Guid.NewGuid();
            string taskName = "task-name";
            string taskDescription = "task-description";
            CreateTaskDto createTaskDto = new CreateTaskDto(taskName, taskDescription);
            _mocker.GetMock<ITaskService>().Setup(x => x.CreateTask(
                It.Is<string>(y => y == createTaskDto.Name), 
                It.Is<string>(y => y == createTaskDto.Description))).ReturnsAsync(new Domain.Entities.Task(taskId, taskName, taskDescription));

            // Act
            var result = await _sut.CreateTask(createTaskDto);

            // Assert
            var taskDtoResult = result.ShouldBeAssignableTo<OkObjectResult>()?.Value.ShouldBeAssignableTo<TaskDto>();
            taskDtoResult.ShouldNotBeNull();
            taskDtoResult.Id.ShouldBe(taskId.ToString());
            taskDtoResult.Name.ShouldBe(taskName);
            taskDtoResult.Description.ShouldBe(taskDescription);
        }
        
        [Test]
        public async System.Threading.Tasks.Task CreateTask_InvalidParametersException_ReturnsBadRequest()
        {
            // Arrange
            string taskName = "task-name";
            string taskDescription = "task-description";
            var errorDictionary = new Dictionary<string, string>
            {
                {"taskName", taskName}
            };
            CreateTaskDto createTaskDto = new CreateTaskDto(taskName, taskDescription);
            _mocker.GetMock<ITaskService>().Setup(x => x.CreateTask(
                It.Is<string>(y => y == createTaskDto.Name),
                It.Is<string>(y => y == createTaskDto.Description)))
                .ThrowsAsync(new InvalidParametersException(errorDictionary));

            // Act
            var result = await _sut.CreateTask(createTaskDto);

            // Assert
            var badRequestResult = result.ShouldBeAssignableTo<BadRequestObjectResult>()?.Value;
            badRequestResult.ShouldNotBeNull();
            badRequestResult.ShouldBeEquivalentTo(errorDictionary);
        }
        
        [Test]
        public async System.Threading.Tasks.Task CreateTask_Exception_ReturnsInternalServerError()
        {
            // Arrange
            string taskName = "task-name";
            string taskDescription = "task-description";
            CreateTaskDto createTaskDto = new CreateTaskDto(taskName, taskDescription);
            _mocker.GetMock<ITaskService>().Setup(x => x.CreateTask(
                It.Is<string>(y => y == createTaskDto.Name), 
                It.Is<string>(y => y == createTaskDto.Description))).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.CreateTask(createTaskDto);

            // Assert
            result.ShouldBeAssignableTo<StatusCodeResult>();
            (result as StatusCodeResult)?.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        }
    }
}