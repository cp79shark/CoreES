using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSql
{
    public class MSSqlPersistence : IPersistEvents
    {
        private readonly SqlConnectionStringBuilder EventStoreConnection;
        private readonly SqlConnectionStringBuilder MasterConnection;

        public MSSqlPersistence(string ConnectionString, ISerializeEvents Serializer)
        {
            EventStoreConnection = new SqlConnectionStringBuilder(ConnectionString);
            MasterConnection = new SqlConnectionStringBuilder(ConnectionString) { InitialCatalog = "master" };
        }

        public IPersistEvents DropEventStore()
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

        public IPersistEvents InitializeEventStore()
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
    }
}
