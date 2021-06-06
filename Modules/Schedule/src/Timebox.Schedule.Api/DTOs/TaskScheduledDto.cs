namespace Timebox.Schedule.Api.DTOs
{
    public class TaskScheduledDto
    {
        public string ScheduleId { get; set; }
        public string TimeboxId { get; set; }
        public TimeboxDto Timebox { get; set; }
        public string TaskId { get; set; }

        public static TaskScheduledDto FromEntity(Domain.Entities.Timebox timebox)
        {
            return new TaskScheduledDto
            {
                ScheduleId = timebox.ScheduleId.ToString(),
                TimeboxId = timebox.Id.ToString(),
                Timebox = TimeboxDto.FromEntity(timebox),
                TaskId = timebox.Task?.Id.ToString()
            };
        }
    }
}