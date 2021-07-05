using System;

namespace Timebox.Schedule.Api.DTOs
{
    public class CreateScheduleDto
    {        
        public CreateScheduleDto(DateTime date)
        {
            Date = date;
        }
        
        public DateTime Date { get; private set; }
    }
}