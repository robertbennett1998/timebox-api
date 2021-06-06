using System;
using System.Collections.Generic;

namespace Timebox.Schedule.Domain.Entities
{
    public class Timebox : ITimebox
    {
        public Timebox(int durationInMinutes, TaskBase task=null)
        {
            Id = Guid.NewGuid();
            DurationInMinutes = durationInMinutes;
            Task = task;
        }
        
        public Timebox(Guid id, int durationInMinutes, TaskBase task=null)
        {
            Id = id;
            DurationInMinutes = durationInMinutes;
        }
        
        public Guid Id { get; private set; }
        public int DurationInMinutes { get; private set; }
        public TaskBase Task { get; private set; }
    }
}