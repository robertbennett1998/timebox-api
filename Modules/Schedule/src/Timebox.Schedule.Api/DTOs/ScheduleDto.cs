using System;

namespace Timebox.Schedule.Api.DTOs
{
    public class ScheduleDto
    {
        public ScheduleDto(Guid id)
        {
            Id = id;
        }
        
        public Guid Id { get; }
        
        public static ScheduleDto FromEntity(Domain.Entities.Schedule schedule)
        {
            return new ScheduleDto(schedule.Id);
        }
    }
}