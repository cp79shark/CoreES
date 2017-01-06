using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public class DuplicateEventException : Exception
    {
        public Guid DuplicateEventId { get; }

        public DuplicateEventException(Guid DuplicateEventId) : base($"Duplicate event detected with id: {DuplicateEventId.ToString()}")
        {
            this.DuplicateEventId = DuplicateEventId;
        }

        public DuplicateEventException(Guid DulicateEventId, string message) : base(message)
        {
            this.DuplicateEventId = DuplicateEventId;
        }

        public DuplicateEventException(Guid DulicateEventId, string message, Exception innerException) : base(message, innerException)
        {
            this.DuplicateEventId = DuplicateEventId;
        }
    }
}
