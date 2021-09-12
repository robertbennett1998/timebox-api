using System;

namespace Timebox.Schedule.Api.DTOs
{
    public class ScheduleDto
    {
        public ScheduleDto(Guid id, string name, DateTime date)
        {
            Id = id;
            Name = name;
            Date = date;
        }
        
        public Guid Id { get; }
        public string Name { get; }
        public DateTime Date { get; }
        
        public static ScheduleDto FromEntity(Domain.Entities.ISchedule schedule)
        {
            return new ScheduleDto(schedule.Id, schedule.Name, schedule.Date);
        }
    }
}