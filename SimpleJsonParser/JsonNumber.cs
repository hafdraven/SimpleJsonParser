using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJsonParser
{
    class JsonNumber : JsonValue
    {
        internal double _value;
        public override string ToString()
        {
            return _value.ToString();
        }

        internal override JsonValue Query(JsonPath path)
        {
            return this;
        }

        public JsonNumber(double n)
        {
            _value = n;
        }

        public static new JsonNumber Parse(Queue<char> str)
        {
            StringBuilder sb = new StringBuilder();
            while (str.Count > 0)
            {
                char c = str.Peek();
                if (c == ',' || c == '}' || c == ']' || char.IsWhiteSpace(c)) { break; }
                str.Dequeue();
                sb.Append(c);
            }
            return new JsonNumber(double.Parse(sb.ToString()));
        }
    }
}
