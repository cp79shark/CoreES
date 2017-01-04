using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreES
{
    public interface ISerializeObjects
    {
        byte[] Serialize(object Object);
        object Deserialize(byte[] Bytes);
    }
}
