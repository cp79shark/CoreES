using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public interface IPersistEvents
    {
        IPersistEvents DropEventStore();

        IPersistEvents InitializeEventStore();

        void PersistEvents(string StreamId, int ExpectedVersion, IEnumerable<EventData> Events);
    }
}
