using System;
using System.Threading.Tasks;
using Timebox.Task.Application.Interfaces.Services;

namespace Timebox.Task.Application.Services
{
    public class TaskService : ITaskService
    {
        public Task<Domain.Entities.Task> CreateTask(string name, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task<Domain.Entities.Task> EditTask(Guid taskId, string name, string description)
        {
            throw new System.NotImplementedException();
        }

        public System.Threading.Tasks.Task DeleteTask(Guid taskId)
        {
            throw new System.NotImplementedException();
        }

        public Task<Domain.Entities.Task> GetTask(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}