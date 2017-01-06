using CoreES.Persistence.MSSQL.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL.Tests.Helpers
{
    public class EventStoreHelper
    {
        private readonly DatabaseFixture fixture;

        public EventStoreHelper(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        public bool DatabaseExists()
        {
            // from http://stackoverflow.com/a/33782992
            using (var connection = new SqlConnection(ConnectionStringFor("master")))
            {
                using (var command = new SqlCommand($"SELECT db_id('{fixture.EventStoreDatabase}')", connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        public bool TableExists(string Table)
        {
            using (var connection = new SqlConnection(ConnectionStringFor(fixture.EventStoreDatabase)))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT id FROM sysobjects WHERE name='{Table}'";

                var tableId = command.ExecuteScalar();
                connection.Close();
                SqlConnection.ClearAllPools();

                if (tableId == null || tableId == DBNull.Value)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public List<EventRow> GetEvents()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionStringFor(fixture.EventStoreDatabase)))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM [Events] ORDER BY [StreamPosition]";

                var eventsReader = command.ExecuteReader();

                List<EventRow> events = new List<EventRow>();

                while (eventsReader.Read())
                {
                    events.Add(
                            new EventRow
                            {
                                StreamPosition = eventsReader.GetInt64(0),
                                StreamId = eventsReader.GetString(2),
                                EventId = eventsReader.GetGuid(3),
                                EventNumber = eventsReader.GetInt32(4),
                                EventType = eventsReader.GetString(5),
                                Data = new StreamReader(eventsReader.GetStream(6)).ReadToEnd(),
                                Metadata = new StreamReader(eventsReader.GetStream(7)).ReadToEnd(),
                                Created = eventsReader.GetDateTime(8)
                            }
                        );
                }

                connection.Close();
                SqlConnection.ClearPool(connection);

                return events;
            }
        }

        private string ConnectionStringFor(string DatabaseName)
        {
            return $"Data Source={fixture.DatabaseServer}; Initial Catalog={DatabaseName}; Integrated Security=SSPI";
        }
    }

    public class EventRow
    {
        public long StreamPosition { get; set; }

        public string StreamId { get; set; }

        public Guid EventId { get; set; }

        public int EventNumber { get; set; }

        public string EventType { get; set; }

        public string Data { get; set; }

        public string Metadata { get; set; }

        public DateTime Created { get; set; }

    }
}
