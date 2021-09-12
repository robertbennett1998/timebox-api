using System;

namespace Timebox.Schedule.Api.DTOs
{
    public class CreateScheduleDto
    {        
        public CreateScheduleDto(string name, DateTime date)
        {
            Name = name;
            Date = date;
        }
        
        public string Name { get; private set; }
        public DateTime Date { get; private set; }
    }
}