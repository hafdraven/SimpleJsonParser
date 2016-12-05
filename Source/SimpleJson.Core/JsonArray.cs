using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleJson.Core
{
    /// <summary>
    /// Encapsulates Json Array
    /// </summary>
    public class JsonArray : JsonValue
    {
        /// <summary>
        /// array items collection
        /// </summary>
        internal List<JsonValue> _values=new List<JsonValue>();

        /// <summary>
        /// Queries the Values using the path
        /// </summary>
        /// <param name="path">evaluated path</param>
        /// <returns>Json value</returns>
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

        /// <summary>
        /// String serializer
        /// </summary>
        /// <returns>serialized string representation</returns>
        public override string ToString()
        {
            return "[" + string.Join(",", _values.Select(m => m.ToString())) + "]";
        }

        /// <summary>
        /// initializer constructor
        /// </summary>
        /// <param name="val">Enumerable with array items</param>
        public JsonArray(IEnumerable<JsonValue> val)
        {
            _values = new List<JsonValue>(val);
        }

        /// <summary>
        /// default constructor
        /// </summary>
        public JsonArray()
        {
            _values = new List<JsonValue>();
        }

        /// <summary>
        /// parse method
        /// </summary>
        /// <param name="str">parsing character queue</param>
        /// <returns>collection of array items</returns>
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

        /// <summary>
        /// Parses string into Json Value
        /// </summary>
        /// <param name="str">input string, containing json value</param>
        /// <returns>parsed json value</returns>
        public static new JsonArray Parse(Queue<char> str)
        {
            return new JsonArray(ParseValues(str));
        }

        /// <summary>
        /// adds an item to array
        /// </summary>
        /// <param name="v">Json value to be added</param>
        public void Add(JsonValue v)
        {
            _values.Add(v);
        }

        /// <summary>
        /// adds an item to array
        /// </summary>
        /// <param name="v">string that is parsed as Json value and added to the array</param>
        public void Add(string v)
        {
            Add(JsonValue.Parse(v));
        }

        /// <summary>
        /// merges two arrays
        /// </summary>
        /// <param name="v">other array to merge with</param>
        public void Merge(JsonArray v)
        {
            _values.AddRange(v._values);
        }
    }
}
