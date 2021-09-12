using System;
using System.Collections.Generic;

namespace Timebox.Schedule.Domain.Entities
{
    public interface ITimebox : IEntity
    {
        Guid ScheduleId { get; }
        int DurationInMinutes { get; }
        DateTime FromDateTime { get; }
        ITaskBase Task { get; }
    }
}