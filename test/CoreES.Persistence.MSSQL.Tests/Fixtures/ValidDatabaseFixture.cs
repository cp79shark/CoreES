using CoreES.Persistence.MSSQL.Tests.Helpers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL.Tests.Fixtures
{
    public class ValidDatabaseFixture : IDisposable
    {
        public Mock<ISerializeObjects> Serializer { get; } = new Mock<ISerializeObjects>();
        public IEventStore sut { get; }
        public string ConnectionString { get; }
        public string DatabaseServer { get; } = @"(localdb)\MSSQLLocalDB";
        public string DatabaseName { get; } = "BasicESTests";

        public ValidDatabaseFixture()
        {
            ConnectionString = $"Data Source={DatabaseServer};Initial Catalog={DatabaseName};Integrated Security=True;";

            sut = new MSSQLEventStore(ConnectionString, Serializer.Object);
        }

        public void Dispose()
        {
            DatabaseHelper.DropDatabase(DatabaseServer, DatabaseName);
        }
    }
}
