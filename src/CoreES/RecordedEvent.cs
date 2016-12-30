using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public class RecordedEvent
    {
        public string EventStreamId { get; }

        public Guid EventId { get; }

        public byte[] Data { get; }

        public byte[] Metadata { get; }
    }
}
