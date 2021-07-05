namespace Timebox.Task.Api.DTOs
{
    public class TaskDto
    {
        public TaskDto(string id,string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }
        
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public static TaskDto FromEntity(Domain.Entities.Task task)
        {
            return new TaskDto(task.Id.ToString(), task.Name, task.Description);
        }
    }
}