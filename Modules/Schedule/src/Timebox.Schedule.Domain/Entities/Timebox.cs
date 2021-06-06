using System;
using System.Collections.Generic;

namespace Timebox.Schedule.Domain.Entities
{
    public class Timebox : ITimebox
    {
        public Timebox(Guid scheduleId, int durationInMinutes, DateTime fromDateTime, TaskBase task=null)
        {
            Id = Guid.NewGuid();
            ScheduleId = scheduleId;
            DurationInMinutes = durationInMinutes;
            FromDateTime = fromDateTime;
            Task = task;
        }
        
        public Timebox(Guid id, Guid scheduleId, int durationInMinutes, DateTime fromDateTime, TaskBase task=null)
        {
            Id = id;
            ScheduleId = scheduleId;
            DurationInMinutes = durationInMinutes;
            FromDateTime = fromDateTime;
            Task = task;
        }
        
        public Guid Id { get; private set; }
        public Guid ScheduleId { get; private set; }
        public int DurationInMinutes { get; private set; }
        public TaskBase Task { get; private set; }
        public DateTime FromDateTime { get; private set; }
    }
}