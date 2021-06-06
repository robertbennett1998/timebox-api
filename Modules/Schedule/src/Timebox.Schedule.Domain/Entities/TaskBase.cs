using System;

namespace Timebox.Schedule.Domain.Entities
{
    public class TaskBase : ITaskBase
    {
        public TaskBase(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
        
        public TaskBase(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
    }
}