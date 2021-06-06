using System;
using System.Collections.Generic;

namespace Timebox.Schedule.Domain.Entities
{
    public interface ITimebox
    {
        Guid Id { get; }
        int DurationInMinutes { get; }
        TaskBase Task { get; }
    }
}