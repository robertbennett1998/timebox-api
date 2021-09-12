using System;
using System.Collections.Generic;

namespace Timebox.Schedule.Domain.Entities
{
    public interface ISchedule : IEntity
    {
        public string Name { get; }
        public DateTime Date { get; }
        IEnumerable<ITimebox> Timeboxes { get; }
    }
}