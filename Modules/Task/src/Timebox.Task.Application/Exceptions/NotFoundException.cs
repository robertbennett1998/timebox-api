using System;

namespace Timebox.Task.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string resourceName, string resourceIdentifier)
        {
            ResourceName = resourceName;
            ResourceIdentifier = resourceIdentifier;
        }

        public string ResourceName { get; }
        public string ResourceIdentifier { get; }
    }
}