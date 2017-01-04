using CoreES.Persistence.MSSQL;
using CoreES.Serialization.JSON;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CoreES.AcceptanceTests
{
    public class EventStoreFeature
    {
        [Fact]
        public void Can_Add_New_Stream()
        {
            // given
            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BasicESTests;Integrated Security=True;";
            ISerializeObjects serializer = new JsonObjectSerializer();
            IEventStore eventStore = new MSSQLEventStore(connectionString, serializer)
                .DropEventStore()
                .InitializeEventStore();
            Guid eventId = Guid.NewGuid();
            string type = "My Type";
            string streamId = "1";
            int expectedVersion = 0;
            List<EventData> events = new List<EventData>
            {
                new EventData(eventId, type, "Hello", "My metadata")
            };

            // when
            eventStore.AppendToStreamAsync(streamId, expectedVersion, events);

            // then
            using (IDbConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();

                command.CommandText = "SELECT * from dbo.Events";
                var reader = command.ExecuteReader();
                object data = reader["Data"];
                byte[] dataBytes = (byte[])data;

                string dataString = JsonConvert.DeserializeObject<string>(Encoding.UTF8.GetString(dataBytes), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                object metadata = reader["Metadata"];
                byte[] metadataBytes = (byte[])metadata;

                string metadataString = JsonConvert.DeserializeObject<string>(Encoding.UTF8.GetString(metadataBytes), new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });

                dataString.Should().Be("Hello");
                metadataString.Should().Be("My metadata");

                connection.Close();
            }
        }
    }
}
