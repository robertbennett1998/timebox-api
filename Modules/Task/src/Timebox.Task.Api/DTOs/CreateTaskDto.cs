namespace Timebox.Task.Api.DTOs
{
    public class CreateTaskDto
    {
        public CreateTaskDto(string name, string description)
        {
            Name = name;
            Description = description;
        }
        
        public string Name { get; }
        public string Description { get; }
    }
}