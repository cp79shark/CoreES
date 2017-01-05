using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL.Tests.Fixtures
{
    public class EmptyEventStore : IDisposable
    {
        public IEventStore sut { get; }
        public string ConnectionString { get; }
        public string DatabaseServer { get; } = @"(localdb)\MSSQLLocalDB";
        public string DatabaseName { get; } = "BasicESTests";

        public EmptyEventStore()
        {
            ConnectionString = $"Data Source={DatabaseServer};Initial Catalog={DatabaseName};Integrated Security=True;";

            sut = new MSSQLEventStore(ConnectionString, new Serializer())
                .DropEventStore()
                .InitializeEventStore();
        }

        public void Dispose()
        {
        }

        private class Serializer : ISerializeObjects
        {
            public object Deserialize(byte[] Bytes)
            {
                return Encoding.UTF8.GetString(Bytes);
            }

            public byte[] Serialize(object Object)
            {
                if(Object is string)
                {
                    return Encoding.UTF8.GetBytes(Object as string);
                }
                else
                {
                    throw new ArgumentException("Should be string");
                }
            }
        }
    }
}
