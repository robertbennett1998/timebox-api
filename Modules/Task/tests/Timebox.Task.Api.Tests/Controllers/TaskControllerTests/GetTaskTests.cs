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
using Timebox.Task.Application.Exceptions;
using Timebox.Task.Application.Interfaces.Services;

namespace Timebox.Task.Api.Tests.Controllers.TaskControllerTests
{
    [TestFixture]
    class GetTaskTests
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
        public async System.Threading.Tasks.Task GetTask_Success_ReturnsTaskDto()
        {
            // Arrange
            var id = Guid.NewGuid();
            var taskName = "name";
            var taskDescription = "description";
            var task = new Domain.Entities.Task(id, taskName, taskDescription);
            _mocker.GetMock<ITaskService>().Setup(x => x.GetTask(It.Is<Guid>(y => y == id))).ReturnsAsync(task);

            // Act
            var result = await _sut.GetTask(id);

            // Assert
            result.ShouldBeAssignableTo<OkObjectResult>()
                .Value.ShouldBeAssignableTo<Domain.Entities.ITask>()
                .ShouldBe(task);
        }

        [Test]
        public async System.Threading.Tasks.Task GetTask_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var taskName = "name";
            var taskDescription = "description";
            var task = new Domain.Entities.Task(id, taskName, taskDescription);
            _mocker.GetMock<ITaskService>()
                .Setup(x => x.GetTask(It.Is<Guid>(y => y == id)))
                .ThrowsAsync(new NotFoundException(taskName, taskName));

            // Act
            var result = await _sut.GetTask(id);

            // Assert
            result.ShouldBeAssignableTo<NotFoundResult>();
        }

        [Test]
        public async System.Threading.Tasks.Task GetTask_Exception_ReturnsInternalServerError()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mocker.GetMock<ITaskService>().Setup(x => x.GetTask(It.Is<Guid>(y => y == id))).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.GetTask(id);

            // Assert
            result.ShouldBeAssignableTo<StatusCodeResult>();
            (result as StatusCodeResult)?.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        }
    }
}
