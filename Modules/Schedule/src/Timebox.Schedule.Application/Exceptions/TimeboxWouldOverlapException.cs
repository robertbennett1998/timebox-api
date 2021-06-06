using System;

namespace Timebox.Schedule.Application.Exceptions
{
    public class TimeboxWouldOverlapException : Exception
    {
        public string ExistingTimeboxId { get; }

        public TimeboxWouldOverlapException(string existingTimeboxId)
        {
            ExistingTimeboxId = existingTimeboxId;
        }
    }
}