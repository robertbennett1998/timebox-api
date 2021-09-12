using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timebox.Schedule.Application.Exceptions;
using Timebox.Schedule.Application.Interfaces.Repositories;
using Timebox.Schedule.Application.Interfaces.Services;

namespace Timebox.Schedule.Application.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }
        
        public async Task<Domain.Entities.ISchedule> CreateSchedule(string name, DateTime dateTime)
        {
            var schedule = new Domain.Entities.Schedule(name, dateTime);
            
            await _scheduleRepository.Add(schedule);

            return schedule;
        }
        
        public async Task<Domain.Entities.ISchedule> GetSchedule(string id)
        {
            if (!Guid.TryParse(id, out var scheduleId))
            {
                throw new InvalidParametersException(new Dictionary<string, string> {{nameof(id), $"Cannot parse string ({id}) into GUID"}});
            }
            
            var schedule = await _scheduleRepository.Get(scheduleId);

            if (schedule == null)
                throw new NotFoundException(nameof(schedule), nameof(id));
                
            return await _scheduleRepository.Get(Guid.Parse(id));
        }
        
        public async Task<Domain.Entities.ISchedule[]> GetSchedules()
        {
            var schedules = await _scheduleRepository.GetAll();
            return schedules.ToArray();
        }
    }
}