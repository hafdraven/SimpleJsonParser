using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleJson.Core
{
    /// <summary>
    /// Defines the merge methods for json objects
    /// </summary>
    public enum JsonObjectMergeConflictOption : byte
    {
        TakeLocal = 0,
        TakeRemote = 1,
        MergeArray = 2
    }

    /// <summary>
    /// encapsulates json object
    /// </summary>
    public class JsonObject : JsonValue
    {
        /// <summary>
        /// internal dictionary of object properties
        /// </summary>
        internal Dictionary<JsonString, JsonValue> _values = new Dictionary<JsonString, JsonValue>();

        /// <summary>
        /// String serializer
        /// </summary>
        /// <returns>serialized string representation</returns>
        public override string ToString()
        {
            return "{" + string.Join(",", _values.Select(m => m.Key.ToString() + ":" + m.Value.ToString()).ToArray()) + "}";
        }

        /// <summary>
        /// parse method
        /// </summary>
        /// <param name="str">parsing character queue</param>
        /// <returns>collection of object properties</returns>
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

        /// <summary>
        /// Parses string into Json Value
        /// </summary>
        /// <param name="str">input string, containing json value</param>
        /// <returns>parsed json value</returns>
        public static new JsonObject Parse(Queue<char> str)
        {
            return new JsonObject() { _values = ParseValues(str).ToDictionary(m => m.Key, m => m.Value) };
        }

        /// <summary>
        /// Queries the Values using the path
        /// </summary>
        /// <param name="path">evaluated path</param>
        /// <returns>Json value</returns>
        internal override JsonValue Query(JsonPath path)
        {
            JsonPathSection section = path.Current;
            if (section != null && section.sectionName == "$") { path.Pop(); section = path.Current; }

            if (section != null)
            {
                JsonString s = new JsonString(section.sectionName);
                if (_values.ContainsKey(s))
                {
                    JsonValue v = _values[s];
                    if (!(v is JsonArray)) path.Pop();
                    return _values[s].Query(path);
                }
                else if (path.PathMode == JsonPathEvaluationMode.Strict) throw new Exception("Path error in strict mode");
                else return null;
            }
            return this;
        }

        /// <summary>
        /// add an object property
        /// </summary>
        /// <param name="key">property key</param>
        /// <param name="value">property value</param>
        /// <param name="mergeOption">merge option if such property exists</param>
        public void Add(JsonString key, JsonValue value, JsonObjectMergeConflictOption mergeOption=JsonObjectMergeConflictOption.TakeRemote)
        {
            if (_values.ContainsKey(key))
            {
                switch (mergeOption)
                {
                    case JsonObjectMergeConflictOption.TakeRemote:
                        _values[key] = value;
                        break;
                    case JsonObjectMergeConflictOption.MergeArray:
                        if (_values[key] is JsonArray)
                        {
                            (_values[key] as JsonArray).Add(value);
                        }
                        else
                        {
                            JsonArray newArray = new JsonArray();
                            newArray.Add(_values[key]);
                            if (value is JsonArray)
                            {
                                newArray.Add(value as JsonArray);
                            }
                            else
                            {
                                newArray.Add(value);
                            }
                            _values[key] = newArray;
                        }
                        break;
                }

            }
            else
            {
                _values.Add(key, value);
            }
        }

        /// <summary>
        /// merge two json objects
        /// </summary>
        /// <param name="obj">the object to merge with</param>
        /// <param name="option">object merging option for properties with duplicate keys</param>
        public void Merge(JsonObject obj, JsonObjectMergeConflictOption option)
        {
            foreach (var p in obj._values)
            {
                if (_values.ContainsKey(p.Key))
                {
                    switch (option)
                    {
                        case JsonObjectMergeConflictOption.TakeRemote:
                            _values[p.Key] = p.Value;
                            break;
                        case JsonObjectMergeConflictOption.MergeArray:
                            if (p.Value is JsonArray && _values[p.Key] is JsonArray)
                            {
                                (_values[p.Key] as JsonArray).Add(p.Value as JsonArray);
                            } else if (p.Value is JsonArray)
                            {
                                JsonArray newArray = new JsonArray();
                                newArray.Add(_values[p.Key]);
                                newArray.Add(p.Value as JsonArray);
                                _values[p.Key] = newArray;
                            }
                            else if (_values[p.Key] is JsonArray)
                            {
                                (_values[p.Key] as JsonArray).Add(p.Value as JsonArray);
                            }
                            else
                            {
                                JsonArray newArray = new JsonArray();
                                newArray.Add(_values[p.Key]);
                                newArray.Add(p.Value);
                            }
                            break;
                    }
                }
                else
                {
                    _values.Add(p.Key, p.Value);
                }
            }
        }
    }
}
