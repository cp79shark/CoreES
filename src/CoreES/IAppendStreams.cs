using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreES
{
    public interface IAppendStreams
    {
        Task AppendToStreamAsync(string StreamId, int ExpectedVersion, IEnumerable<EventData> Events, CancellationToken cancellationToken = default(CancellationToken));
    }
}
