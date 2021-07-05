using System;

namespace Timebox.Task.Domain.Entities
{
    public class Task : ITask
    {
        public Task(string name, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
        }
        
        public Task(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; }
    }
}