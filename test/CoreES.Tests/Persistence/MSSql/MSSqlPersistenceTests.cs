using CoreES.Persistence.MSSql;
using CoreES.Tests.Helpers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoreES.Tests.Persistence.MSSql
{
    public class MSSqlPersistenceTests
    {
        protected readonly Mock<ISerializeEvents> Serializer = new Mock<ISerializeEvents>();
        protected readonly IPersistEvents sut;
        protected readonly string ConnectionString;
        protected readonly string DatabaseServer = @"(localdb)\MSSQLLocalDB";
        protected readonly string DatabaseName = "BasicESTests";

        protected MSSqlPersistenceTests()
        {
            DatabaseHelper.DropDatabase(DatabaseServer, DatabaseName);

            ConnectionString = $"Data Source={DatabaseServer};Initial Catalog={DatabaseName};Integrated Security=True;";

            sut = new MSSqlPersistence(ConnectionString, Serializer.Object);
        }

        [Collection("Database Tests")]
        public class Given_No_Database_Exists : MSSqlPersistenceTests
        {
            public Given_No_Database_Exists() : base()
            {

            }

            [Fact]
            public void DropEventStore_Will_Be_Idempotent()
            {
                // when
                sut.DropEventStore();

                // then
                DatabaseHelper.DatabaseExists(DatabaseServer, DatabaseName).Should().BeFalse();
            }

            [Fact]
            public void InitializeEventStore_Will_Create_A_New_Database()
            {
                // when
                sut.InitializeEventStore();

                // then
                DatabaseHelper.DatabaseExists(DatabaseServer, DatabaseName).Should().BeTrue();
            }
        }

        [Collection("Database Tests")]
        public class Given_A_Database_Exists : MSSqlPersistenceTests
        {
            public Given_A_Database_Exists() : base()
            {
                sut.InitializeEventStore();
            }

            [Fact]
            public void DropEventStore_Will_Remove_The_Database()
            {
                // when
                sut.DropEventStore();

                // then
                DatabaseHelper.DatabaseExists(DatabaseServer, DatabaseName).Should().BeFalse();
            }

            [Fact]
            public void InitializeEventStore_Will_Be_Idempotent()
            {
                // when
                sut.InitializeEventStore();

                // then
                DatabaseHelper.DatabaseExists(DatabaseServer, DatabaseName).Should().BeTrue();
            }
        }

        
    }
}
