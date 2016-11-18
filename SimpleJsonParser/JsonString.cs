using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJsonParser
{
    class JsonString : JsonValue, IComparable<JsonString>
    {
        internal string _value;
        public override string ToString()
        {
            return "\"" + _value + "\"";
        }

        //public override string ToXmlText(string parentName="root")
        //{
        //    return _value;
        //}

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

        public int CompareTo(JsonString other)
        {
            return _value.CompareTo(other._value);
        }
    }
}
