using System;
using System.Collections.Generic;

namespace Timebox.Schedule.Domain.Entities
{
    public interface ISchedule
    {
        Guid Id { get; }
        public DateTime Date { get; }
        IEnumerable<Timebox> Timeboxes { get; }
    }
}