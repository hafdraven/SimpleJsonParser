using System.Collections.Generic;

namespace SimpleJson.Core
{
    /// <summary>
    /// Abstract JSON value class. All the Json-related classes inherit it
    /// </summary>
    abstract public class JsonValue
    {
        protected static void SkipWhiteSpace(Queue<char> str)
        {
            while (char.IsWhiteSpace(str.Peek()) && str.Count > 0) { str.Dequeue(); }
        }

        protected static void DequeueMultiple(Queue<char> str, int num)
        {
            for (int i = 0; i < num; i++) str.Dequeue();
        }

        public JsonValue Query(string path)
        {
            return Query(new JsonPath(path));
        }

        internal abstract JsonValue Query(JsonPath path);
        //public abstract JsonValue Value(string path);
        //public abstract JsonValue Modify(string path, string newValue);

        public static JsonValue Parse(string str)
        {
            return Parse(new Queue<char>(str.ToCharArray()));
        }

        internal static JsonValue Parse(Queue<char> str)
        {
            char c = str.Peek();
            SkipWhiteSpace(str);
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
