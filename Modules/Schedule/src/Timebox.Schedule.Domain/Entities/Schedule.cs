using System;
using System.Collections;
using System.Collections.Generic;

namespace Timebox.Schedule.Domain.Entities
{
    public class Schedule : ISchedule
    {
        public Schedule(DateTime date, Timebox[] timeboxes=null)
        {
            Id = Guid.NewGuid();
            Date = date;
            Timeboxes = timeboxes;
        }
        
        public Schedule(Guid id, DateTime date, Timebox[] timeboxes=null)
        {
            Id = id;
            Date = date;
            Timeboxes = timeboxes;
        }

        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
        public IEnumerable<Timebox> Timeboxes { get; private set; }
    }
}