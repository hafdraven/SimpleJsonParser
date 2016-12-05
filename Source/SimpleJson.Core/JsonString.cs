using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleJson.Core
{
    /// <summary>
    /// Json String scalar Value
    /// </summary>
    public class JsonString : JsonScalar, IEquatable<JsonString>
    {
        /// <summary>
        /// insternal string state
        /// </summary>
        internal string _value;

        /// <summary>
        /// string serializer
        /// </summary>
        /// <returns>quoted string representation</returns>
        public override string ToString()
        {
            return "\"" + _value + "\"";
        }


        /// <summary>
        /// Queries the Values using the path
        /// </summary>
        /// <param name="path">evaluated path</param>
        /// <returns>Json value</returns>
        internal override JsonValue Query(JsonPath path)
        {
            return this;
        }

        /// <summary>
        /// initializer constructor
        /// </summary>
        /// <param name="str">initializer string parameter</param>
        public JsonString(string str)
        {
            _value = str;
        }

        /// <summary>
        /// Parses string into Json Value
        /// </summary>
        /// <param name="str">input string, containing json value</param>
        /// <returns>parsed json value</returns>
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

        /// <summary>
        /// GetHashCode for IEquatable inteface implementation
        /// </summary>
        /// <returns>hash identifier</returns>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <summary>
        /// Equals for IEquatable inteface implementation
        /// </summary>
        /// <param name="other">JsonString to compare with</param>
        /// <returns>true if objects are equal. false otherwise.</returns>
        public bool Equals(JsonString other)
        {
            return _value.Equals(other._value);
        }
    }

}
