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
        [Collection("Database Tests")]
        public class Given_No_Database_Exists : IClassFixture<ValidDatabaseFixture>
        {
            ValidDatabaseFixture fixture;

            public Given_No_Database_Exists(ValidDatabaseFixture fixture)
            {
                this.fixture = fixture;
            }

            [Fact]
            public void DropEventStore_Will_Be_Idempotent()
            {
                // when
                ((MSSQLEventStore)fixture.sut).DropEventStore();

                // then
                DatabaseHelper.DatabaseExists(fixture.DatabaseServer, fixture.DatabaseName).Should().BeFalse();
            }

            [Fact]
            public void InitializeEventStore_Will_Create_A_New_Database()
            {
                // when
                ((MSSQLEventStore)fixture.sut).InitializeEventStore();

                // then
                DatabaseHelper.DatabaseExists(fixture.DatabaseServer, fixture.DatabaseName).Should().BeTrue();
                DatabaseHelper.TableExists(fixture.DatabaseServer, fixture.DatabaseName, "DbVersionHistory").Should().BeTrue();
                DatabaseHelper.TableExists(fixture.DatabaseServer, fixture.DatabaseName, "Events").Should().BeTrue();
                DatabaseHelper.TableExists(fixture.DatabaseServer, fixture.DatabaseName, "Aggregates").Should().BeTrue();
            }
        }

        [Collection("Database Tests")]
        public class Given_A_Database_Exists : IClassFixture<ValidDatabaseFixture>
        {
            ValidDatabaseFixture fixture;

            public Given_A_Database_Exists(ValidDatabaseFixture fixture)
            {
                this.fixture = fixture;

                ((MSSQLEventStore)this.fixture.sut).InitializeEventStore();
            }

            [Fact]
            public void DropEventStore_Will_Remove_The_Database()
            {
                // when
                ((MSSQLEventStore)fixture.sut).DropEventStore();

                // then
                DatabaseHelper.DatabaseExists(fixture.DatabaseServer, fixture.DatabaseName).Should().BeFalse();
            }

            [Fact]
            public void InitializeEventStore_Will_Be_Idempotent()
            {
                // when
                ((MSSQLEventStore)fixture.sut).InitializeEventStore();

                // then
                DatabaseHelper.DatabaseExists(fixture.DatabaseServer, fixture.DatabaseName).Should().BeTrue();
                DatabaseHelper.TableExists(fixture.DatabaseServer, fixture.DatabaseName, "DbVersionHistory").Should().BeTrue();
                DatabaseHelper.TableExists(fixture.DatabaseServer, fixture.DatabaseName, "Events").Should().BeTrue();
                DatabaseHelper.TableExists(fixture.DatabaseServer, fixture.DatabaseName, "Aggregates").Should().BeTrue();
            }
        }

        [Collection("Database Tests")]
        public class Given_An_Empty_Event_Store : IClassFixture<EmptyEventStore>
        {
            EmptyEventStore fixture;

            public Given_An_Empty_Event_Store(EmptyEventStore fixture)
            {
                this.fixture = fixture;
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
                await fixture.sut.AppendToStreamAsync(StreamId, ExpectedVersion, events);

                // then
                var result = DatabaseHelper.GetEvents(fixture.DatabaseServer, fixture.DatabaseName);
                result.Count().Should().Be(2);
                result[0].Matches(StreamId, 1, eventOne);
                result[1].Matches(StreamId, 2, eventTwo);
            }
        }
    }
}
