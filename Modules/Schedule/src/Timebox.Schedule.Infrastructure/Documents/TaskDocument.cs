using System;
using Convey.Types;
using Timebox.Schedule.Domain.Entities;

namespace Timebox.Schedule.Infrastructure.Documents
{
    public class TaskDocument : IIdentifiable<Guid>
    {
        private TaskDocument(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public static TaskDocument FromEntity(ITaskBase task)
        {
            if (task == null)
                return null;
            
            return new TaskDocument(task.Id, task.Name);
        }
        
        public static ITaskBase ToEntity(TaskDocument taskDocument)
        {
            if (taskDocument == null)
                return null;
            
            return new TaskBase(taskDocument.Id, taskDocument.Name);
        }
    }
}