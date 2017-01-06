using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreES.Persistence.MSSQL.Tests.Fixtures
{
    public class StringSerializer : ISerializeObjects
    {
        public object Deserialize(byte[] Bytes)
        {
            return Encoding.UTF8.GetString(Bytes);
        }

        public byte[] Serialize(object Object)
        {
            if (Object is string)
            {
                return Encoding.UTF8.GetBytes(Object as string);
            }
            else
            {
                throw new ArgumentException("Tests should be using a string");
            }
        }
    }
}
