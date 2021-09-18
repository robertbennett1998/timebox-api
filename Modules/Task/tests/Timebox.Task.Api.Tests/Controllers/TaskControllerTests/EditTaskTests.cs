using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Timebox.Task.Api.Controllers;
using Timebox.Task.Api.DTOs;
using Timebox.Task.Application.Exceptions;
using Timebox.Task.Application.Interfaces.Services;

namespace Timebox.Task.Api.Tests.Controllers.TaskControllerTests
{
    [TestFixture]
    class EditTaskTests
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
        public async System.Threading.Tasks.Task EditTask_Success_ReturnsEditedTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updatedTaskName = "original-name";
            var updatedTaskDescription = "original-description";
            var updatedTask = new Domain.Entities.Task(taskId, updatedTaskName, updatedTaskDescription);

            _mocker.GetMock<ITaskService>()
                .Setup(x => x.EditTask(
                    It.Is<Guid>(y => y == taskId),
                    It.Is<string>(y => y == updatedTaskName),
                    It.Is<string>(y => y == updatedTaskDescription)))
                    .ReturnsAsync(updatedTask);


            // Act
            var result = await _sut.EditTask(taskId, new DTOs.EditTaskDto()
            {
                Name = updatedTaskName,
                Description = updatedTaskDescription
            });

            // Assert
            var taskDto = result
                .ShouldBeAssignableTo<OkObjectResult>()
                .Value.ShouldBeAssignableTo<TaskDto>();
            taskDto.Name.ShouldBe(updatedTaskName);
            taskDto.Description.ShouldBe(updatedTaskDescription);
        }

        [Test]
        public async System.Threading.Tasks.Task EditTask_NotFound_ReturnsNotFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updatedTaskName = "original-name";
            var updatedTaskDescription = "original-description";
            var updatedTask = new Domain.Entities.Task(taskId, updatedTaskName, updatedTaskDescription);

            _mocker.GetMock<ITaskService>()
                .Setup(x => x.EditTask(
                    It.Is<Guid>(y => y == taskId),
                    It.Is<string>(y => y == updatedTaskName),
                    It.Is<string>(y => y == updatedTaskDescription)))
                    .ThrowsAsync(new NotFoundException("task", taskId.ToString()));


            // Act
            var result = await _sut.EditTask(taskId, new DTOs.EditTaskDto()
            {
                Name = updatedTaskName,
                Description = updatedTaskDescription
            });

            // Assert
            result.ShouldBeAssignableTo<NotFoundResult>();
        }

        [Test]
        public async System.Threading.Tasks.Task EditTask_InvalidParameters_ReturnsBadRequest()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updatedTaskName = "original-name";
            var updatedTaskDescription = "original-description";
            var updatedTask = new Domain.Entities.Task(taskId, updatedTaskName, updatedTaskDescription);

            var errorDictionary = new Dictionary<string, string>
            {
                {"taskName", updatedTaskName}
            };

            _mocker.GetMock<ITaskService>()
                .Setup(x => x.EditTask(
                    It.Is<Guid>(y => y == taskId),
                    It.Is<string>(y => y == updatedTaskName),
                    It.Is<string>(y => y == updatedTaskDescription)))
                    .ThrowsAsync(new InvalidParametersException(errorDictionary));


            // Act
            var result = await _sut.EditTask(taskId, new DTOs.EditTaskDto()
            {
                Name = updatedTaskName,
                Description = updatedTaskDescription
            });

            // Assert
            var badRequestResult = result.ShouldBeAssignableTo<BadRequestObjectResult>()?.Value;
            badRequestResult.ShouldNotBeNull();
            badRequestResult.ShouldBeEquivalentTo(errorDictionary);
        }

        [Test]
        public async System.Threading.Tasks.Task EditTask_Exception_ReturnsInternalServerError()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var updatedTaskName = "original-name";
            var updatedTaskDescription = "original-description";
            var updatedTask = new Domain.Entities.Task(taskId, updatedTaskName, updatedTaskDescription);

            var errorDictionary = new Dictionary<string, string>
            {
                {"taskName", updatedTaskName}
            };

            _mocker.GetMock<ITaskService>()
                .Setup(x => x.EditTask(
                    It.Is<Guid>(y => y == taskId),
                    It.Is<string>(y => y == updatedTaskName),
                    It.Is<string>(y => y == updatedTaskDescription)))
                    .ThrowsAsync(new Exception());


            // Act
            var result = await _sut.EditTask(taskId, new DTOs.EditTaskDto()
            {
                Name = updatedTaskName,
                Description = updatedTaskDescription
            });

            // Assert
            result.ShouldBeAssignableTo<StatusCodeResult>();
            (result as StatusCodeResult)?.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        }
    }
}
