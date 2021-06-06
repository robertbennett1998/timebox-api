using System;
using System.Threading.Tasks;

namespace Timebox.Schedule.Application.Interfaces.Services
{
    public interface IScheduleService
    {
        Task<Domain.Entities.Schedule> GetSchedule(string id);
        Task<Domain.Entities.Schedule> CreateSchedule(DateTime dateTime);
    }
}