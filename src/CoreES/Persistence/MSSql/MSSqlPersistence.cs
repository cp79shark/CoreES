using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSql
{
    public class MSSqlPersistence : IEventStore
    {
        private readonly SqlConnectionStringBuilder EventStoreConnection;
        private readonly SqlConnectionStringBuilder MasterConnection;

        public MSSqlPersistence(string ConnectionString, ISerializeObjects Serializer)
        {
            EventStoreConnection = new SqlConnectionStringBuilder(ConnectionString);
            MasterConnection = new SqlConnectionStringBuilder(ConnectionString) { InitialCatalog = "master" };
        }

        public Task AppendToStreamAsync(string StreamId, int ExpectedVersion, IEnumerable<EventData> Events, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public IEventStore DropEventStore()
        {
            using (var connection = new SqlConnection(MasterConnection.ConnectionString))
            {
                using (var command = new SqlCommand($"IF EXISTS(select * from sys.databases where name='{EventStoreConnection.InitialCatalog}') DROP DATABASE [{EventStoreConnection.InitialCatalog}]", connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return this;
        }

        public IEventStore InitializeEventStore()
        {
            using (var connection = new SqlConnection(MasterConnection.ConnectionString))
            {
                using (var command = new SqlCommand($"IF NOT EXISTS(select * from sys.databases where name='{EventStoreConnection.InitialCatalog}')CREATE DATABASE [{EventStoreConnection.InitialCatalog}]", connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return this;
        }

        public void PersistEvents(string StreamId, int ExpectedVersion, IEnumerable<EventData> Events)
        {
            throw new NotImplementedException();
        }

        public Task<AllEventsSlice> ReadAllEventsAsync(long Position, int MaxCount, ReadDirection ReadDirection = ReadDirection.Forward, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<StreamEventsSlice> ReadStreamEventsAsync(string StreamId, int Start, int Count, ReadDirection ReadDirection = ReadDirection.Forward, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }
    }
}
