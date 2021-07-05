using System;
using System.Collections.Generic;

namespace Timebox.Task.Application.Exceptions
{
    public class InvalidParametersException : Exception
    {
        public Dictionary<string, string> InvalidParameters { get; }

        public InvalidParametersException(Dictionary<string, string> invalidParameters)
        {
            InvalidParameters = invalidParameters;
        }
    }
}