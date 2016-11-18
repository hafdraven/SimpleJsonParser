using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleJsonParser
{
    class JsonObject : JsonValue
    {
        internal Dictionary<JsonString, JsonValue> _values = new Dictionary<JsonString, JsonValue>();
        public override string ToString()
        {
            return "{" + string.Join(";", _values.Select(m => m.Key.ToString() + ":" + m.Value.ToString()).ToArray()) + "}";
        }

        private static IEnumerable<KeyValuePair<JsonString, JsonValue>> ParseValues(Queue<char> str)
        {
            char c = str.Peek();
            if (c == '{') str.Dequeue();
            while (str.Count > 0)
            {
                SkipWhiteSpace(str);
                JsonString s = JsonString.Parse(str);
                SkipWhiteSpace(str);
                if (str.Peek() != ':') { throw new Exception("Invalid symbol. Expected \":\""); } else { str.Dequeue(); }
                SkipWhiteSpace(str);
                JsonValue val = JsonValue.Parse(str);
                yield return new KeyValuePair<JsonString, JsonValue>(s, val);
                SkipWhiteSpace(str);
                if (str.Peek() == ',') { str.Dequeue(); continue; }
                if (str.Peek() == '}') { str.Dequeue(); break; }
            }
        }

        public static new JsonObject Parse(Queue<char> str)
        {
            return new JsonObject() { _values = ParseValues(str).ToDictionary(m => m.Key, m => m.Value) };
        }

        internal override JsonValue Query(JsonPath path)
        {
            JsonPathSection section = path.Current;
            if (section != null && section.sectionName == "$") { path.Pop(); section = path.Current; }

            if (section != null)
            {
                JsonString s = new JsonString(section.sectionName);
                if (_values.ContainsKey(s))
                {
                    return _values[s].Query(path);
                }
                else if (path.PathMode == JsonPathEvaluationMode.Strict) throw new Exception("Path error in strict mode");
                else return null;
            }
            return this;
        }
    }
}
