using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleJson.Core
{

    class JsonTrue : JsonScalar
    {
        public override string ToString()
        {
            return "true";
        }

        internal override JsonValue Query(JsonPath path)
        {
            return this;
        }

        public static new JsonTrue Parse(Queue<char> str)
        {
            if (string.Concat(str.Take(5)) == "true")
            {
                DequeueMultiple(str, 5);
                return new JsonTrue();
            }
            else throw new Exception("Unknown value. Expected: true");
        }

    }
}
