using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleJsonParser
{
    abstract public class JsonValue
    {
        protected static void GetNextPathSection(string path)
        {
            
        }

        protected static void SkipWhiteSpace(Queue<char> str)
        {
            while (char.IsWhiteSpace(str.Peek()) && str.Count > 0) { str.Dequeue(); }
        }

        protected static void DequeueMultiple(Queue<char> str, int num)
        {
            for (int i = 0; i < num; i++) str.Dequeue();
        }

        public abstract JsonValue Query(string path);
        public abstract JsonValue Value(string path);
        public abstract JsonValue Modify(string path, string newValue);


        public static JsonValue Parse(Queue<char> str)
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
    }
}
