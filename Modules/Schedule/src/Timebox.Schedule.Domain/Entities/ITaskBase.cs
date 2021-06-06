using System;

namespace Timebox.Schedule.Domain.Entities
{
    public interface ITaskBase
    {
        Guid Id { get; }
        string Name { get; }
    }
}