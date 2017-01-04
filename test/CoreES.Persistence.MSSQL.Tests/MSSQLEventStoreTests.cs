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
            }
        }
    }
}
