using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreES
{
    public interface IEventStore : IAppendStreams
    {
        Task<StreamEventsSlice> ReadStreamEventsAsync(string StreamId, int Start, int Count, ReadDirection ReadDirection = ReadDirection.Forward, CancellationToken cancellationToken = default(CancellationToken));

        Task<AllEventsSlice> ReadAllEventsAsync(long Position, int MaxCount, ReadDirection ReadDirection = ReadDirection.Forward, CancellationToken cancellationToken = default(CancellationToken));
    }
}
