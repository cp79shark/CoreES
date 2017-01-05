using CoreES.Persistence.MSSQL.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL
{
    public class MSSQLEventStore : IEventStore
    {
        private readonly SqlConnectionStringBuilder EventStoreConnection;
        private readonly SqlConnectionStringBuilder MasterConnection;
        private readonly ISerializeObjects Serializer;

        public MSSQLEventStore(string ConnectionString, ISerializeObjects ObjectSerializer)
        {
            EventStoreConnection = new SqlConnectionStringBuilder(ConnectionString);
            MasterConnection = new SqlConnectionStringBuilder(ConnectionString) { InitialCatalog = "master" };
            this.Serializer = ObjectSerializer;
        }

        public async Task AppendToStreamAsync(string StreamId, int ExpectedVersion, IEnumerable<EventData> Events, CancellationToken cancellationToken = default(CancellationToken))
        {
            using (var connection = new SqlConnection(EventStoreConnection.ConnectionString))
            {
                using (var command = new SqlCommand(TSqlBuilder.AppendEventStatement(), connection))
                {
                    await connection.OpenAsync(cancellationToken);

                    var StreamIdHashParameter = command.Parameters.Add("StreamIdHash", SqlDbType.Char, 40);
                    var StreamIdParameter = command.Parameters.Add("StreamId", SqlDbType.NVarChar, 1000);
                    var EventIdParameter = command.Parameters.Add("EventId", SqlDbType.UniqueIdentifier);
                    var EventNumberParameter = command.Parameters.Add("EventNumber", SqlDbType.Int);
                    var EventTypeParameter = command.Parameters.Add("EventType", SqlDbType.NVarChar, 1000);
                    var DataParameter = command.Parameters.Add("Data", SqlDbType.VarBinary, -1);
                    var MetadataParameter = command.Parameters.Add("Metadata", SqlDbType.VarBinary, -1);
                    var CreatedParameter = command.Parameters.Add("Created", SqlDbType.DateTime2, 7);
                    command.Prepare();

                    var hashBytes = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(StreamId));
                    var hashValue = BitConverter.ToString(hashBytes).Replace("-", "");

                    StreamIdHashParameter.Value = hashValue;
                    StreamIdParameter.Value = StreamId;

                    int EventNumber = 1;

                    foreach (var @event in Events)
                    {
                        EventIdParameter.Value = @event.EventId;
                        EventNumberParameter.Value = EventNumber;
                        EventTypeParameter.Value = @event.Type;
                        DataParameter.Value = Serializer.Serialize(@event.Data);
                        MetadataParameter.Value = Serializer.Serialize(@event.Metadata);
                        CreatedParameter.Value = DateTime.UtcNow;
                        await command.ExecuteNonQueryAsync(cancellationToken);

                        ++EventNumber;
                    }

                    



                    connection.Close();
                }
            }
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
            SqlConnection.ClearAllPools();

            using (var connection = new SqlConnection(MasterConnection.ConnectionString))
            {
                using (var command = new SqlCommand(TSqlBuilder.DropDatabaseStatement(EventStoreConnection.InitialCatalog), connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
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
                    connection.Close();
                }
            }

            using (var connection = new SqlConnection(EventStoreConnection.ConnectionString))
            {
                using (var command = new SqlCommand(TSqlBuilder.CreateSchemaStatementVersion1_0_0(), connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return this;
        }
    }
}
