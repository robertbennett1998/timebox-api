namespace Timebox.Task.Application.Interfaces.Services
{
    public interface ITaskService
    {
        System.Threading.Tasks.Task<Domain.Entities.Task> CreateTask(string name, string description);
        System.Threading.Tasks.Task EditTask(string taskId, string name, string description);
        System.Threading.Tasks.Task DeleteTask(string taskId);
    }
}