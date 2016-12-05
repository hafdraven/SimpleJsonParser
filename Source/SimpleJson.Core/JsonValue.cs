using System.Collections.Generic;

namespace SimpleJson.Core
{
    /// <summary>
    /// Abstract JSON value class. All the Json-related classes derive from it
    /// </summary>
    abstract public class JsonValue
    {
        /// <summary>
        /// Auxilary static method to dequeue while the current character is a whitespace
        /// </summary>
        /// <param name="str">referenced character queue</param>
        protected static void SkipWhiteSpace(Queue<char> str)
        {
            while (char.IsWhiteSpace(str.Peek()) && str.Count > 0) { str.Dequeue(); }
        }

        /// <summary>
        /// Auxilary static method to dequeue multiple times
        /// </summary>
        /// <param name="str">referenced character queue</param>
        /// <param name="num">number of times to dequeue</param>
        protected static void DequeueMultiple(Queue<char> str, int num)
        {
            for (int i = 0; i < num; i++) str.Dequeue();
        }

        /// <summary>
        /// Query a json value with a query string
        /// </summary>
        /// <param name="path">string, that contains a query</param>
        /// <returns>json value representing the query result</returns>
        public JsonValue Query(string path)
        {
            return Query(new JsonPath(path));
        }

        /// <summary>
        /// Method placeholder for quering the json value
        /// </summary>
        /// <param name="path">JsonPath object with parsed path sections</param>
        /// <returns>json value representing the query result</returns>
        internal abstract JsonValue Query(JsonPath path);

        /// <summary>
        /// Parses the string and generates the JsonValue
        /// </summary>
        /// <param name="str">inpul string, containing Json</param>
        /// <returns>Parsed JsonValue</returns>
        public static JsonValue Parse(string str)
        {
            return Parse(new Queue<char>(str.ToCharArray()));
        }

        /// <summary>
        /// Internal parser method. Relies on queue of characters
        /// </summary>
        /// <param name="str">reference to character queue</param>
        /// <returns>parsed json value</returns>
        internal static JsonValue Parse(Queue<char> str)
        {
            SkipWhiteSpace(str);
            char c = str.Peek();
            switch (c)
            {
                case '"':
                    return JsonString.Parse(str);
                case '{':
                    return JsonObject.Parse(str);
                case '[':
                    return JsonArray.Parse(str);
                case 't':
                    return JsonTrue.Parse(str);
                case 'f':
                    return JsonFalse.Parse(str);
                case 'n':
                    return JsonNull.Parse(str);
                default:
                    return JsonNumber.Parse(str);
            }
        }

        /// <summary>
        /// Parsing method, that returns either null, true, false, number or string in special Json class
        /// </summary>
        /// <param name="value">input string to be parsed</param>
        /// <returns>Scalar json class</returns>
        public static JsonValue ParseScalar(string value)
        {
            switch (value)
            {
                case "null": return new JsonNull();
                case "true":return new JsonTrue();
                case "false":return new JsonFalse();
                default:
                        double num = 0;
                        if (double.TryParse(value,System.Globalization.NumberStyles.Any,System.Globalization.CultureInfo.InvariantCulture, out num))
                            return new JsonNumber(num);
                        else
                            return new JsonString(value);
            }
        }
    }
}
