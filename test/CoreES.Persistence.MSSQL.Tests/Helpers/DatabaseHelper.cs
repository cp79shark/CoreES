using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
    }
}
