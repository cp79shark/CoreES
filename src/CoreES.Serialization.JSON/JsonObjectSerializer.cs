using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace CoreES.Serialization.JSON
{
    public class JsonObjectSerializer : ISerializeObjects
    {
        public object Deserialize(byte[] Bytes)
        {
            throw new NotImplementedException();
        }

        public byte[] Serialize(object Object)
        {
            string json = JsonConvert.SerializeObject(Object, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
