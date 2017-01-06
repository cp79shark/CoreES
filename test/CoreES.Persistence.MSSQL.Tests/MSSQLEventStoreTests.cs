using CoreES.Persistence.MSSQL;
using CoreES.Persistence.MSSQL.Tests.Fixtures;
using CoreES.Persistence.MSSQL.Tests.Helpers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoreES.Persistence.MSSQL.Tests
{
    public class MSSQLEventStoreTests
    {
        [Collection(Collections.DatabaseTests)]
        public class Given_No_EventStore_Database_Exists
        {
            private readonly DatabaseFixture fixture;
            private readonly EventStoreHelper eventStoreHelper;
            private readonly IEventStore SUT;

            public Given_No_EventStore_Database_Exists(DatabaseFixture fixture)
            {
                this.fixture = fixture;
                this.SUT = new MSSQLEventStore(fixture.ConnectionString, new StringSerializer()).DropEventStore();

                eventStoreHelper = new EventStoreHelper(fixture);
                eventStoreHelper.DatabaseExists().Should().BeFalse();
            }

            [Fact]
            public void DropEventStore_Will_Be_Idempotent()
            {
                // when
                ((MSSQLEventStore)SUT).DropEventStore();

                // then
                eventStoreHelper.DatabaseExists().Should().BeFalse();
            }

            [Fact]
            public void InitializeEventStore_Will_Create_A_New_Database()
            {
                // when
                ((MSSQLEventStore)SUT).InitializeEventStore();

                // then
                eventStoreHelper.DatabaseExists().Should().BeTrue();
                eventStoreHelper.TableExists("DbVersionHistory").Should().BeTrue();
                eventStoreHelper.TableExists("Events").Should().BeTrue();
                eventStoreHelper.TableExists("Aggregates").Should().BeTrue();
            }
        }

        [Collection(Collections.DatabaseTests)]
        public class Given_An_EventStore_Database_Exists
        {
            private readonly DatabaseFixture fixture;
            private readonly EventStoreHelper eventStoreHelper;
            private readonly IEventStore SUT;

            public Given_An_EventStore_Database_Exists(DatabaseFixture fixture)
            {
                this.fixture = fixture;
                this.SUT = new MSSQLEventStore(fixture.ConnectionString, new StringSerializer()).DropEventStore().InitializeEventStore();

                eventStoreHelper = new EventStoreHelper(fixture);
                eventStoreHelper.DatabaseExists().Should().BeTrue();
            }

            [Fact]
            public void DropEventStore_Will_Remove_The_Database()
            {
                // when
                ((MSSQLEventStore)SUT).DropEventStore();

                // then
                eventStoreHelper.DatabaseExists().Should().BeFalse();
            }

            [Fact]
            public void InitializeEventStore_Will_Be_Idempotent()
            {
                // when
                ((MSSQLEventStore)SUT).InitializeEventStore();

                // then
                eventStoreHelper.DatabaseExists().Should().BeTrue();
                eventStoreHelper.TableExists("DbVersionHistory").Should().BeTrue();
                eventStoreHelper.TableExists("Events").Should().BeTrue();
                eventStoreHelper.TableExists("Aggregates").Should().BeTrue();
            }
        }

        [Collection(Collections.DatabaseTests)]
        public class Given_An_Empty_Event_Store
        {
            private readonly DatabaseFixture fixture;
            private readonly EventStoreHelper eventStoreHelper;
            private readonly IEventStore SUT;

            public Given_An_Empty_Event_Store(DatabaseFixture fixture)
            {
                this.fixture = fixture;
                eventStoreHelper = new EventStoreHelper(fixture);

                this.SUT = new MSSQLEventStore(fixture.ConnectionString, new StringSerializer()).DropEventStore().InitializeEventStore();

                eventStoreHelper = new EventStoreHelper(fixture);
                eventStoreHelper.DatabaseExists().Should().BeTrue();
            }

            [Fact]
            public async Task Adding_A_New_Stream_Will_Add_The_Events_To_The_Store()
            {
                // given
                string StreamId = "1";
                int ExpectedVersion = 0;
                EventData eventOne = new EventData(Guid.NewGuid(), "My EventOne", "My EventOne Data", "My EventOne Metadata");
                EventData eventTwo = new EventData(Guid.NewGuid(), "My EventTwo", "My EventTwo Data", "My EventTwo Metadata");

                List<EventData> events = new List<EventData>
                {
                    eventOne,
                    eventTwo
                };

                // when
                await SUT.AppendToStreamAsync(StreamId, ExpectedVersion, events);

                // then
                var result = eventStoreHelper.GetEvents();
                result.Count().Should().Be(2);
                result[0].Matches(StreamId, 1, eventOne);
                result[1].Matches(StreamId, 2, eventTwo);
            }
        }
    }
}
