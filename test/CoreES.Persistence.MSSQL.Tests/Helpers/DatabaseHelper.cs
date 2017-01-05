using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL.Tests.Helpers
{
    public class DatabaseHelper
    {
        public static bool DatabaseExists(string Server, string Database)
        {
            string connectionString = $"Data Source={Server};Initial Catalog=master;Integrated Security=SSPI;";

            // from http://stackoverflow.com/a/33782992
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand($"SELECT db_id('{Database}')", connection))
                {
                    connection.Open();
                    return (command.ExecuteScalar() != DBNull.Value);
                }
            }
        }

        public static void DropDatabase(string Server, string Database)
        {
            string connectionString = $"Data Source={Server};Initial Catalog=master;Integrated Security=SSPI;";

            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"IF EXISTS(select * from sys.databases where name='{Database}') DROP DATABASE [{Database}]";

                command.ExecuteNonQuery();

                connection.Close();
                SqlConnection.ClearAllPools();
            }
        }

        public static bool TableExists(string Server, string Database, string Table)
        {
            string connectionString = $"Data Source={Server};Initial Catalog={Database};Integrated Security=SSPI;";

            using (IDbConnection connection = new SqlConnection(connectionString))
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

        public static List<EventRow> GetEvents(string Server, string Database)
        {
            string connectionString = $"Data Source={Server};Initial Catalog={Database};Integrated Security=SSPI;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = $"SELECT * FROM [Events] ORDER BY [StreamPosition]";

                var eventsReader = command.ExecuteReader();

                List<EventRow> events = new List<EventRow>();

                while(eventsReader.Read())
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
