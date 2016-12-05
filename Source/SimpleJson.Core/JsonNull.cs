using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleJson.Core
{
    /// <summary>
    /// Json Null scalar Value
    /// </summary>
    class JsonNull : JsonScalar
    {
        /// <summary>
        /// String serializer
        /// </summary>
        /// <returns>serialized string representation</returns>
        public override string ToString()
        {
            return "null";
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
        /// Parses string into Json Value
        /// </summary>
        /// <param name="str">input string, containing json value</param>
        /// <returns>parsed json value</returns>
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
