using System;

namespace Timebox.Schedule.Domain.Entities
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}