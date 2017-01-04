using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Data;
using System.Data.SqlClient;

namespace CoreES.Tests
{
    /*
    public class EventStoreTests
    {
        private readonly Mock<IPersistEvents> persistence;
        private readonly IEventStore eventStore;

        public EventStoreTests()
        {
            persistence = new Mock<IPersistEvents>();
            eventStore = new EventStore(persistence.Object);
        }

        [Fact]
        public void It_Will_Add_The_Events_To_The_Database()
        {
            // given
            Guid eventId = Guid.NewGuid();
            string eventType = "HelloType";
            string streamId = "1";
            int expectedVersion = 0;
            List<EventData> events = new List<EventData>
            {
                new EventData(eventId, eventType, "Hello", "My metadata")
            };

            // when
            eventStore.AppendToStreamAsync(streamId, expectedVersion, events);

            // then
            persistence.Verify(p => p.PersistEvents(It.Is<string>(id => id == streamId), It.Is<int>(ver => ver == expectedVersion), It.Is<List<EventData>>(
                e => e[0].EventId == eventId
                && e[0].Type == eventType
                && e[0].Data == "Hello" as object
                && e[0].Metadata == "My metadata" as object
                )));
        }

        class FakeEvent
        {
            public string Value { get; }

            public FakeEvent(string Value)
            {
                this.Value = Value;
            }
        }
    }

    interface IStream
    {

    }

    public class StreamEventsSlice
    {

    }

    public class Stream
    {
        public object MetaData { get; set; }
    }

    public class StreamMetadata
    {
        public Type MetadataType { get; set; }

    }
    */
}
