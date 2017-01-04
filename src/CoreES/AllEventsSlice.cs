using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public sealed class AllEventsSlice
    {
        public ReadDirection ReadDirection { get; }

        public long FromPositionNumber { get; }

        public long ToPostionNumber { get; }

        public long NextPositionNumber { get; }

        public bool IsEndOfStream { get; }

        public RecordedEvent[] Events { get; }

        public AllEventsSlice(ReadDirection ReadDirection, long FromPositionNumber, long ToPostionNumber, long NextPositionNumber, bool IsEndOfStream, RecordedEvent[] Events)
        {
            this.ReadDirection = ReadDirection;
            this.FromPositionNumber = FromPositionNumber;
            this.ToPostionNumber = ToPostionNumber;
            this.NextPositionNumber = NextPositionNumber;
            this.IsEndOfStream = IsEndOfStream;
            this.Events = Events;
        }
    }
}
