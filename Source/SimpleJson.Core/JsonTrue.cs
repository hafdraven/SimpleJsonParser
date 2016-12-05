using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleJson.Core
{
    /// <summary>
    /// Json True scalar Value
    /// </summary>
    class JsonTrue : JsonScalar
    {
        /// <summary>
        /// String serializer
        /// </summary>
        /// <returns>serialized string representation</returns>
        public override string ToString()
        {
            return "true";
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
