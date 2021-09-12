using System;
using System.Threading.Tasks;

namespace Timebox.Schedule.Application.Interfaces.Services
{
    public interface IScheduleService
    {
        Task<Domain.Entities.ISchedule> GetSchedule(string id);
        Task<Domain.Entities.ISchedule[]> GetSchedules();
        Task<Domain.Entities.ISchedule> CreateSchedule(string name, DateTime dateTime);
    }
}