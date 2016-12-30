using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public class EventData
    {
        public Guid EventId { get; }

        public string Type { get; }

        public object Data { get; }

        public object Metadata { get; }

        public EventData(Guid EventId, string Type, object Data, object Metadata)
        {
            this.EventId = EventId;
            this.Type = Type;
            this.Data = Data;
            this.Metadata = Metadata;
        }
    }
}
