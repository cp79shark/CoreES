using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoreES.Persistence.MSSQL.Tests.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        public string ConnectionString { get { return $"Data Source={DatabaseServer};Initial Catalog={EventStoreDatabase};Integrated Security=SSPI;"; } }

        public string DatabaseServer { get; } = @"(localdb)\MSSQLLocalDB";

        public string EventStoreDatabase { get; } = "BasicESTests";

        public void Dispose()
        {
        }
    }

    [CollectionDefinition(Collections.DatabaseTests)]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {

    }
}
