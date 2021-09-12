using System;
using System.Collections;
using System.Collections.Generic;

namespace Timebox.Schedule.Domain.Entities
{
    public class Schedule : ISchedule
    {
        public Schedule(string name, DateTime date, IEnumerable<ITimebox> timeboxes=null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Date = date;
            Timeboxes = timeboxes ?? new List<ITimebox>();
        }

        public Schedule(Guid id, string name, DateTime date, IEnumerable<ITimebox> timeboxes=null)
        {
            Id = id;
            Name = name;
            Date = date;
            Timeboxes = timeboxes ?? new List<ITimebox>();
        }

        public Guid Id { get; private set; }
        public string Name { get; set; }
        public DateTime Date { get; private set; }
        public IEnumerable<ITimebox> Timeboxes { get; private set; }
    }
}