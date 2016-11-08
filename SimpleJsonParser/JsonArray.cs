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
        public override string ToString()
        {
            return "[" + string.Join(",", _values.Select(m => m.ToString())) + "]";
        }

        public override string ToXmlText(string parentName="sjp:item")
        {
            return string.Concat(_values.Select(m => "<" + parentName + (parentName == "sjp:item" ? " xmlns:sjp=\"SimpleJsonParser\"" : "") + ">" + m.ToXmlText() + "</" + parentName + ">"));
        }

        public JsonArray(IEnumerable<JsonValue> val)
        {
            _values = val.ToArray();
        }

        static IEnumerable<JsonValue> ParseXmlValues(XmlNodeList l)
        { 
            string name="";
            if (l.Count > 0)
            {
                name = l[0].LocalName;
                foreach (XmlNode x in l)
                {
                    yield return JsonValue.ParseXml(x);
                }
            }
            else
                throw new Exception("Wrong xml node list for array");
        }

        public static new JsonArray ParseXml(XmlNodeList l)
        {
            return new JsonArray(JsonArray.ParseXmlValues(l));
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
