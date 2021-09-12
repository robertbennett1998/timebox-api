using System;

namespace Timebox.Schedule.Domain.Entities
{
    public interface ITaskBase : IEntity
    {
        string Name { get; }
    }
}