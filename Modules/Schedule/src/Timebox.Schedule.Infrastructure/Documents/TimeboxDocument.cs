using System;
using Convey.Types;
using Timebox.Schedule.Domain.Entities;

namespace Timebox.Schedule.Infrastructure.Documents
{
    public class TimeboxDocument : IIdentifiable<Guid>
    { private TimeboxDocument(Guid timeboxId, Guid scheduleId, DateTime timeboxFromDateTime, int timeboxDurationInMinutes, TaskDocument task=null)
        {
            Id = timeboxId;
            ScheduleId = scheduleId;
            Task = task;
            TimeboxFromDateTime = timeboxFromDateTime;
            TimeboxDurationInMinutes = timeboxDurationInMinutes;
        }

        public Guid Id { get; set; }
        public Guid ScheduleId { get; set; }
        public TaskDocument Task { get; set; }
        public DateTime TimeboxFromDateTime { get; set; }
        public int TimeboxDurationInMinutes { get; set; }


        public static TimeboxDocument FromEntity(ITimebox timebox)
        {
            if (timebox == null)
                return null;
            
            return new TimeboxDocument(timebox.Id, timebox.ScheduleId, timebox.FromDateTime,
                timebox.DurationInMinutes, TaskDocument.FromEntity(timebox.Task));
        }

        public static ITimebox ToEntity(TimeboxDocument timeboxDocument)
        {
            if (timeboxDocument == null)
                return null;
            
            return new Domain.Entities.Timebox(timeboxDocument.ScheduleId, timeboxDocument.TimeboxDurationInMinutes,
                timeboxDocument.TimeboxFromDateTime, TaskDocument.ToEntity(timeboxDocument.Task));
        }
    }
}