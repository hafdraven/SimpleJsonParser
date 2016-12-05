using System.Collections.Generic;
using System.Text;

namespace SimpleJson.Core
{
    /// <summary>
    /// Json Number Value
    /// </summary>
    class JsonNumber : JsonScalar
    {
        /// <summary>
        /// internal value of [double] type
        /// </summary>
        internal double _value;
        public override string ToString()
        {
            return _value.ToString(System.Globalization.CultureInfo.InvariantCulture);
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
        /// Initializer constructor
        /// </summary>
        /// <param name="n">initial value</param>
        public JsonNumber(double n)
        {
            _value = n;
        }

        /// <summary>
        /// Parses string into Json Value
        /// </summary>
        /// <param name="str">input string, containing json value</param>
        /// <returns>parsed json value</returns>
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
