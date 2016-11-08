using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading.Tasks;

namespace SimpleJsonParser
{
    abstract public class JsonValue
    {
        public abstract string ToXmlText(string parentName="root");

        protected static void SkipWhiteSpace(Queue<char> str)
        {
            while (char.IsWhiteSpace(str.Peek()) && str.Count > 0) { str.Dequeue(); }
        }

        protected static void DequeueMultiple(Queue<char> str, int num)
        {
            for (int i = 0; i < num; i++) str.Dequeue();
        }

        public static JsonValue ParseXml(XmlNode node)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.Text:
                    double r;
                    if (double.TryParse(node.InnerText,out r)) 
                        return new JsonNumber(r); 
                    else 
                        switch (node.InnerText)
                        {
                            case "true":
                                return new JsonTrue();
                            case "false":
                                return new JsonFalse();
                            case "null":
                                return new JsonNull();
                            default:
                                return new JsonString(node.InnerText);
                        }
                case XmlNodeType.Element:
                    string name = node.LocalName;
                    if (int.Parse(node.ParentNode.SelectSingleNode("count(" + name + ")").InnerText) > 0)
                        return JsonArray.ParseXml(node.ParentNode.SelectNodes("./" + name));
                    else
                        return JsonObject.ParseXml(node);
            }
            return new JsonNull();
        }

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
