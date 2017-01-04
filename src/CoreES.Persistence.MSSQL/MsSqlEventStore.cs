using CoreES.Persistence.MSSQL.Internal;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL
{
    public class MSSQLEventStore : IEventStore
    {
        private readonly SqlConnectionStringBuilder EventStoreConnection;
        private readonly SqlConnectionStringBuilder MasterConnection;

        public MSSQLEventStore(string ConnectionString, ISerializeObjects ObjectSerializer)
        {
            EventStoreConnection = new SqlConnectionStringBuilder(ConnectionString);
            MasterConnection = new SqlConnectionStringBuilder(ConnectionString) { InitialCatalog = "master" };
        }

        public Task AppendToStreamAsync(string StreamId, int ExpectedVersion, IEnumerable<EventData> Events, CancellationToken cancellationToken = default(CancellationToken))
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

        public MSSQLEventStore DropEventStore()
        {
            using (var connection = new SqlConnection(MasterConnection.ConnectionString))
            {
                using (var command = new SqlCommand(TSqlBuilder.DropDatabaseStatement(EventStoreConnection.InitialCatalog), connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return this;
        }

        public MSSQLEventStore InitializeEventStore()
        {
            using (var connection = new SqlConnection(MasterConnection.ConnectionString))
            {
                using (var command = new SqlCommand(TSqlBuilder.CreateDatabaseStatement(EventStoreConnection.InitialCatalog), connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            return this;
        }
    }
}
