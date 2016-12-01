using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleJson.Core
{
    public class JsonArray : JsonValue
    {
        internal List<JsonValue> _values=new List<JsonValue>();

        internal override JsonValue Query(JsonPath path)
        {
            JsonPathSection section = path.Current;
            if (section != null)
            {
                path.Pop();
                if (section.indexerPesent)
                {
                    int indexer = section.indexerValue;
                    if (indexer>=_values.Count)
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
                    else if (_values.Count > 0)
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
            _values = new List<JsonValue>(val);
        }

        public JsonArray()
        {
            _values = new List<JsonValue>();
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

        public void Add(JsonValue v)
        {
            _values.Add(v);
        }

        public void Add(string v)
        {
            Add(JsonValue.Parse(v));
        }
        public void Merge(JsonArray v)
        {
            _values.AddRange(v._values);
        }
    }
}
