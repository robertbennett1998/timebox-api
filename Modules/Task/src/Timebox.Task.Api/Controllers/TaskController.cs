using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Timebox.Task.Api.DTOs;
using Timebox.Task.Application.Exceptions;
using Timebox.Task.Application.Interfaces.Services;

namespace Timebox.Task.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    { 
        private readonly ITaskService _taskService;
        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        
        [HttpPost("createTask")]
        [ProducesResponseType(typeof(TaskDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async System.Threading.Tasks.Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            try
            {
                var task = await _taskService.CreateTask(createTaskDto.Name, createTaskDto.Description);
                return Ok(TaskDto.FromEntity(task));
            }
            catch (InvalidParametersException exception)
            {
                return BadRequest(exception.InvalidParameters);
            }
            catch (Exception)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpGet("getTask/{taskId:guid}")]
        [ProducesResponseType(typeof(TaskDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async System.Threading.Tasks.Task<IActionResult> GetTask([FromRoute] Guid taskId)
        {
            try
            {
                var task = await _taskService.GetTask(taskId);
                return Ok(task);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpPut("editTask/{taskId:guid}")]
        [ProducesResponseType(typeof(TaskDto), (int) HttpStatusCode.Accepted)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async System.Threading.Tasks.Task<IActionResult> EditTask([FromRoute] Guid taskId, [FromBody] EditTaskDto editTaskDto)
        {
            try
            {
                var task = await _taskService.EditTask(taskId, editTaskDto.Name, editTaskDto.Description);
                return Ok(TaskDto.FromEntity(task));
            }
            catch (InvalidParametersException exception)
            {
                return BadRequest(exception.InvalidParameters);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpDelete("deleteTask/{taskId:guid}")]
        [ProducesResponseType((int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async System.Threading.Tasks.Task<IActionResult> DeleteTask([FromRoute] Guid taskId)
        {
            try
            {
                await _taskService.DeleteTask(taskId);
                return Ok();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}