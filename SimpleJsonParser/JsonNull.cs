using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleJson.Core
{
    class JsonNull : JsonScalar
    {
        public override string ToString()
        {
            return "null";
        }

        internal override JsonValue Query(JsonPath path)
        {
            return this;
        }

        public static new JsonNull Parse(Queue<char> str)
        {
            if (string.Concat(str.Take(4)) == "null")
            {
                DequeueMultiple(str, 4);
                return new JsonNull();
            }
            else throw new Exception("Unknown value. Expected: null");
        }
    }
}
