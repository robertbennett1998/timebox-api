using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Timebox.Schedule.Application.Exceptions;
using Timebox.Schedule.Application.Interfaces.Services;

namespace Timebox.Schedule.Application.Services
{
    public class ScheduleService : IScheduleService
    {
        public async Task<Domain.Entities.Schedule> CreateSchedule(DateTime dateTime)
        {
            throw new NotImplementedException();
        }
        
        public async Task<Domain.Entities.Schedule> GetSchedule(string id)
        {
            throw new NotImplementedException();
        }
    }
}