using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CoreES.Persistence.MSSQL.Tests.IntegrationTests
{
    public class SqlConnectionStringBuilderTests
    {
        [Fact]
        public void SqlConnectionStringBuilder_Does_Not_Protect_Against_ConnectionString_SQL_Injection()
        {
            string connectionString = "Data Source=DbServer;Initial Catalog='test]; DROP DATABASE test;';Integrated Security=True;";
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

            var initialCatalog = builder.InitialCatalog;

            initialCatalog.Should().Be("test]; DROP DATABASE test;");
        }
    }
}
