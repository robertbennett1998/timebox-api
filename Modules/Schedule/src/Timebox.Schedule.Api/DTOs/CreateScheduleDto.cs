using System;

namespace Timebox.Schedule.Api.DTOs
{
    public class CreateScheduleDto
    {        
        public CreateScheduleDto(DateTime date)
        {
            Id = Guid.NewGuid();
            Date = date;
        }
        
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }
    }
}