using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public class EventStore : IEventStore
    {
        private readonly IPersistEvents persistence;

        public EventStore(IPersistEvents persistence)
        {
            this.persistence = persistence;
        }

        public void AppendToStreamAsync(string StreamId, int ExpectedVersion, IEnumerable<EventData> Events)
        {
            persistence.PersistEvents(StreamId, ExpectedVersion, Events);
        }
    }
}
