using System.Collections.Generic;
using System.Threading.Tasks;
using Timebox.Schedule.Domain.Entities;

namespace Timebox.Schedule.Application.Interfaces.Repositories
{
    public interface IScheduleRepository : IRepository<ISchedule>
    {
        Task<IEnumerable<ISchedule>> GetAll();
    }
}