using System;
using System.Threading.Tasks;
using Timebox.Schedule.Application.Interfaces.Services;

namespace Timebox.Schedule.Application.Services
{
    public class SchedulerService : ISchedulerService
    {
        public async Task<Domain.Entities.Timebox> ScheduleTask(string scheduleId, string timeboxId, string taskId)
        {
            throw new NotImplementedException();
        }

        public async Task<Domain.Entities.Timebox> AllocateTimebox(string scheduleId, int durationInMinutes, DateTime fromDateTime)
        {
            throw new NotImplementedException();
        }
    }
}