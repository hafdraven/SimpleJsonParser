using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJsonParser
{
    class JsonFalse : JsonValue
    {
        public override string ToString()
        {
            return "false";
        }


        public static new JsonFalse Parse(Queue<char> str)
        {
            if (string.Concat(str.Take(5)) == "false")
            {
                DequeueMultiple(str, 5);
                return new JsonFalse();
            }
            else throw new Exception("Unknown value. Expected: false");
        }

        internal override JsonValue Query(JsonPath path)
        {
            return this;
        }
    }
}
