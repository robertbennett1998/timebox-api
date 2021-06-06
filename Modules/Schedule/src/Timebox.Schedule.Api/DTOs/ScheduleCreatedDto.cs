using System;

namespace Timebox.Schedule.Api.DTOs
{
    public class ScheduleCreatedDto
    {
        public ScheduleCreatedDto(Guid id, DateTime date)
        {
            Id = id;
            Date = date;
        }
        
        public Guid Id { get; }
        public DateTime Date { get; }

        public static ScheduleCreatedDto FromEntity(Domain.Entities.Schedule scheduleEntity)
        {
            return new ScheduleCreatedDto(scheduleEntity.Id, scheduleEntity.Date);
        }
    }
}