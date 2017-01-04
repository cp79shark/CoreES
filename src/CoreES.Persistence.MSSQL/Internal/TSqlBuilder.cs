using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL.Internal
{
    internal class TSqlBuilder
    {
        public static string CreateDatabaseStatement(string DatabaseName)
        {
            return $"IF NOT EXISTS(select * from sys.databases where name='{DatabaseName}') CREATE DATABASE [{DatabaseName}]";
        }

        public static string DropDatabaseStatement(string DatabaseName)
        {
            return $"IF EXISTS(select * from sys.databases where name='{DatabaseName}') DROP DATABASE [{DatabaseName}]";
        }
    }
}
