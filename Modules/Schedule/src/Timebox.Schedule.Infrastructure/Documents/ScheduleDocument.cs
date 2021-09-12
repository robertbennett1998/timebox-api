using System;
using System.Collections.Generic;
using System.Linq;
using Convey.Types;
using Timebox.Schedule.Domain.Entities;

namespace Timebox.Schedule.Infrastructure.Documents
{
    public class ScheduleDocument : IIdentifiable<Guid>
    {
        private ScheduleDocument(Guid scheduleId, string name, DateTime scheduleDate, IEnumerable<TimeboxDocument> timeboxes)
        {
            Id = scheduleId;
            Name = name;
            ScheduleDate = scheduleDate;
            Timeboxes = timeboxes;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime ScheduleDate { get; set; }
        public IEnumerable<TimeboxDocument> Timeboxes { get; set; }

        public static ScheduleDocument FromEntity(ISchedule schedule)
        {
            if (schedule == null)
                return null;
            
            return new ScheduleDocument(schedule.Id, schedule.Name, schedule.Date,
                schedule.Timeboxes?.Select(TimeboxDocument.FromEntity));
        }

        public static ISchedule ToEntity(ScheduleDocument scheduleDocument)
        {
            if (scheduleDocument == null)
                return null;
            
            return new Domain.Entities.Schedule(scheduleDocument.Id, scheduleDocument.Name, scheduleDocument.ScheduleDate,
                scheduleDocument.Timeboxes?.Select(TimeboxDocument.ToEntity));
        }

    }
}