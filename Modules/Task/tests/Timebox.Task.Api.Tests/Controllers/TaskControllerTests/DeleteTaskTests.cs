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
    class DeleteTaskTests
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
        public async System.Threading.Tasks.Task DeleteTask_Success_ReturnsOk()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mocker.GetMock<ITaskService>().Setup(x => x.DeleteTask(It.Is<Guid>(y => y == id)));

            // Act
            var result = await _sut.DeleteTask(id);

            // Assert
            result.ShouldBeAssignableTo<OkResult>();
        }

        [Test]
        public async System.Threading.Tasks.Task DeleteTask_NotFound_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mocker.GetMock<ITaskService>().Setup(x => x.DeleteTask(It.Is<Guid>(y => y == id))).ThrowsAsync(new NotFoundException("task", id.ToString()));

            // Act
            var result = await _sut.DeleteTask(id);

            // Assert
            result.ShouldBeAssignableTo<NotFoundResult>();
        }

        [Test]
        public async System.Threading.Tasks.Task DeleteTask_Exception_InternalServerError()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mocker.GetMock<ITaskService>().Setup(x => x.DeleteTask(It.Is<Guid>(y => y == id))).ThrowsAsync(new Exception());

            // Act
            var result = await _sut.DeleteTask(id);

            // Assert
            result.ShouldBeAssignableTo<StatusCodeResult>();
            (result as StatusCodeResult)?.StatusCode.ShouldBe((int)HttpStatusCode.InternalServerError);
        }
    }
}
