using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Timebox.Schedule.Api.DTOs;
using Timebox.Schedule.Application.Exceptions;
using Timebox.Schedule.Application.Interfaces.Services;

namespace Timebox.Schedule.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleService _scheduleService;
        private readonly ISchedulerService _schedulerService;

        public ScheduleController(IScheduleService scheduleService, ISchedulerService schedulerService)
        {
            _scheduleService = scheduleService;
            _schedulerService = schedulerService;
        }

        [HttpPost("createSchedule")]
        [ProducesResponseType(typeof(ScheduleCreatedDto), (int)HttpStatusCode.Created)]
        public async Task<IActionResult> CreateSchedule([FromBody]CreateScheduleDto createScheduleDto)
        {
            try
            {
                Domain.Entities.ISchedule entity = await _scheduleService.CreateSchedule(createScheduleDto.Name, createScheduleDto.Date);
                return Created(entity.Id.ToString(), ScheduleCreatedDto.FromEntity(entity));
            }
            catch (Exception e)
            {
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("getSchedules")]
        [ProducesResponseType(typeof(ScheduleDto[]), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSchedules()
        {
            try
            {
                var schedules = await _scheduleService.GetSchedules();
                return Ok(schedules.Select(ScheduleDto.FromEntity));
            }
            catch (Exception e)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("getSchedule/{scheduleId}")]
        [ProducesResponseType(typeof(ScheduleDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Dictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetSchedule([FromRoute]string scheduleId)
        {
            try
            {
                var entity = await _scheduleService.GetSchedule(scheduleId);
                return Ok(ScheduleDto.FromEntity(entity));
            }
            catch (NotFoundException exception)
            {
                return NotFound(new [] {exception.ResourceName, exception.ResourceIdentifier});
            }
            catch (InvalidParametersException exception)
            {
                return BadRequest(exception.InvalidParameters);
            }
            catch (Exception e)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPatch("scheduleTask/{scheduleId}")]
        [ProducesResponseType(typeof(TaskScheduledDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Dictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> ScheduleTask([FromRoute]string scheduleId, [FromBody]ScheduleTaskDto scheduleTaskDto)
        {
            try
            {
                var timebox = await _schedulerService.AllocateTimebox(scheduleId, scheduleTaskDto.DurationInMinutes,
                    scheduleTaskDto.ScheduledDateTime);
                
                timebox = await _schedulerService.ScheduleTask(scheduleId, timebox.Id.ToString(), scheduleTaskDto.TaskId);
                
                return Ok(TaskScheduledDto.FromEntity(timebox));
            }
            catch (NotFoundException exception)
            {
                return NotFound(new [] {exception.ResourceName, exception.ResourceIdentifier});
            }
            catch (InvalidParametersException exception)
            {
                return BadRequest(exception.InvalidParameters);
            }
            catch (TimeboxWouldOverlapException exception)
            {
                return Conflict(exception.ExistingTimeboxId);
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
        
        [HttpPut("scheduleTask/{scheduleId}/{timeboxId}")]
        [ProducesResponseType(typeof(TaskScheduledDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string[]), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Dictionary<string, string>), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ScheduleTask([FromRoute]string scheduleId, [FromRoute]string timeboxId, [FromBody]string taskId)
        {
            try
            {
                var timebox = await _schedulerService.ScheduleTask(scheduleId, timeboxId, taskId);
                return Ok(TaskScheduledDto.FromEntity(timebox));
            }
            catch (NotFoundException exception)
            {
                return NotFound(new [] {exception.ResourceName, exception.ResourceIdentifier});
            }
            catch (InvalidParametersException exception)
            {
                return BadRequest(exception.InvalidParameters);
            }
            catch (Exception)
            {
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}