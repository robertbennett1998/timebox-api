using System;
using System.Collections.Generic;

namespace Timebox.Schedule.Domain.Entities
{
    public interface ITimebox
    {
        Guid Id { get; }
        Guid ScheduleId { get; }
        int DurationInMinutes { get; }
        DateTime FromDateTime { get; }
        TaskBase Task { get; }
    }
}