using System;

namespace Timebox.Task.Application.Interfaces.Services
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<Domain.Entities.Task> CreateTask(string name, string description);
        System.Threading.Tasks.Task<Domain.Entities.Task> EditTask(Guid taskId, string name, string description);
        System.Threading.Tasks.Task DeleteTask(Guid taskId);
        System.Threading.Tasks.Task<Domain.Entities.Task> GetTask(Guid guid);
    }
}