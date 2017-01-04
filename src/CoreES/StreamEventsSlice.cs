using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public sealed class StreamEventsSlice
    {
        public string StreamId { get; }

        public ReadDirection ReadDirection { get; }

        public int FromEventNumber { get; }

        public int ToEventNumber { get; }

        public int NextEventNumber { get; }

        public bool IsEndOfStream { get; }

        public RecordedEvent[] Events { get; }

        public StreamEventsSlice(string StreamId, ReadDirection ReadDirection, int FromEventNumber, int ToEventNumber, int NextEventNumber, bool IsEndOfStream, RecordedEvent[] Events)
        {
            this.StreamId = StreamId;
            this.ReadDirection = ReadDirection;
            this.FromEventNumber = FromEventNumber;
            this.ToEventNumber = ToEventNumber;
            this.NextEventNumber = NextEventNumber;
            this.IsEndOfStream = IsEndOfStream;
            this.Events = Events;
        }
    }
}
