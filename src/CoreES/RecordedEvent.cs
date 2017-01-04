using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public sealed class RecordedEvent
    {
        public string EventStreamId { get; }

        public Guid EventId { get; }

        public int EventNumber { get; }

        public string EventType { get; }

        public object Data { get; }

        public object Metadata { get; }

        public DateTime Created { get; }

        public RecordedEvent(string EventStreamId, Guid EventId, int EventNumber, string EventType, object Data, object Metadata, DateTime Created)
        {
            this.EventStreamId = EventStreamId;
            this.EventId = EventId;
            this.EventNumber = EventNumber;
            this.EventType = EventType;
            this.Data = Data;
            this.Metadata = Metadata;
            this.Created = Created;
        }
    }
}
