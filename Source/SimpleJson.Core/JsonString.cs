using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJson.Core
{
    public class JsonString : JsonScalar, IEquatable<JsonString>
    {
        internal string _value;
        public override string ToString()
        {
            return "\"" + _value + "\"";
        }

        internal override JsonValue Query(JsonPath path)
        {
            return this;
        }

        public JsonString(string str)
        {
            _value = str;
        }

        public static new JsonString Parse(Queue<char> str)
        {
            StringBuilder sb = new StringBuilder();
            bool BracketsOpen = false;
            bool BracketsClosed = false;

            while (str.Count > 0)
            {
                char c = str.Peek();
                if (!BracketsOpen && c == '"') { BracketsOpen = true; str.Dequeue(); continue; }
                if (BracketsOpen && c == '"') { BracketsClosed = true; str.Dequeue(); break; }
                str.Dequeue();
                sb.Append(c);
            }
            if (!BracketsOpen) throw new Exception("No string to parse");
            if (!BracketsClosed) throw new Exception("Unxpected end of string");
            return new JsonString(sb.ToString());
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public bool Equals(JsonString other)
        {
            return _value.Equals(other._value);
        }
    }

}
