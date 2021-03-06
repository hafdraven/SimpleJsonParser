﻿using System.Collections.Generic;
using System.Text;

namespace SimpleJson.Core
{
    class JsonNumber : JsonScalar
    {
        internal double _value;
        public override string ToString()
        {
            return _value.ToString(System.Globalization.CultureInfo.InvariantCulture);
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
            return new JsonNumber(double.Parse(sb.ToString(),System.Globalization.CultureInfo.InvariantCulture));
        }
    }
}
