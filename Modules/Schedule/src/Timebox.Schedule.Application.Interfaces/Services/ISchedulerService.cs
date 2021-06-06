using System;
using System.Threading.Tasks;

namespace Timebox.Schedule.Application.Interfaces.Services
{
    public interface ISchedulerService
    {
        Task<Domain.Entities.Timebox> ScheduleTask(string scheduleId, string timeboxId, string taskId);
        Task<Domain.Entities.Timebox> AllocateTimebox(string scheduleId, int durationInMinutes, DateTime fromDateTime);
    }
}