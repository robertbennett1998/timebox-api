using System;
namespace Timebox.Schedule.Api.DTOs
{
    public class TimeboxDto
    {
        public string TimeboxId { get; set; }
        public string TaskId { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime FromDateTime { get; set; }

        public static TimeboxDto FromEntity(Domain.Entities.Timebox timebox)
        {
            return new TimeboxDto
            {
                TimeboxId = timebox.Id.ToString(),
                TaskId = timebox.Task?.Id.ToString(),
                DurationInMinutes = timebox.DurationInMinutes,
                FromDateTime = timebox.FromDateTime
            };
        }
    }
}