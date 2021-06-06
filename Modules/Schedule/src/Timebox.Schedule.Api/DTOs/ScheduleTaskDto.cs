using System;

namespace Timebox.Schedule.Api.DTOs
{
    public class ScheduleTaskDto
    {
        public DateTime ScheduledDateTime { get; set; }
        public int DurationInMinutes { get; set; }
        public string TaskId { get; set; }
    }
}