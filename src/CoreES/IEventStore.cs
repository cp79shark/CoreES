using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public interface IEventStore
    {
        void AppendToStreamAsync(string StreamId, int ExpectedVersion, IEnumerable<EventData> Events);
    }
}
