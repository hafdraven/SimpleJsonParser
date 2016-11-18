using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SimpleJsonParser
{
    class JsonArray : JsonValue
    {
        internal JsonValue[] _values;

        internal override JsonValue Query(JsonPath path)
        {
            JsonPathSection section = path.Current;
            if (section != null)
            {
                path.Pop();
                if (section.indexerPesent)
                {
                    int indexer = section.indexerValue;
                    if (indexer>=_values.Length)
                    {
                        if (path.PathMode == JsonPathEvaluationMode.Strict) throw new Exception("Path error in strict mode");
                        else return null;
                    }
                    else
                        return _values[indexer].Query(path);
                }
                else
                {
                    if (path.Current == null) return this;
                    else if (_values.Length > 0)
                        return _values[0].Query(path);
                    else if (path.PathMode == JsonPathEvaluationMode.Strict) throw new Exception("Path error in strict mode");
                    else return null;
                }
            }
            return this;
        }

        public override string ToString()
        {
            return "[" + string.Join(",", _values.Select(m => m.ToString())) + "]";
        }

        public JsonArray(IEnumerable<JsonValue> val)
        {
            _values = val.ToArray();
        }

        static IEnumerable<JsonValue> ParseValues(Queue<char> str)
        {
            if (str.Peek() == '[') str.Dequeue();
            while (str.Count > 0)
            {
                SkipWhiteSpace(str);
                yield return JsonValue.Parse(str);
                SkipWhiteSpace(str);
                if (str.Peek() == ',') { str.Dequeue(); continue; }
                if (str.Peek() == ']') { str.Dequeue(); break; }
            }
        }

        public static new JsonArray Parse(Queue<char> str)
        {
            return new JsonArray(ParseValues(str));
        }
    }
}
