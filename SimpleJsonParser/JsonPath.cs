using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleJsonParser
{
    internal class JsonPathSection
    {
        internal string sectionName;
        internal bool indexerPesent;
        internal int indexerValue;
    }

    public enum JsonPathEvaluationMode
    {
        Lax=0,
        Strinct=1
    }

    internal class JsonPath
    {
        public JsonPathEvaluationMode PathMode = JsonPathEvaluationMode.Lax;
        internal Stack<JsonPathSection> _sections = new Stack<JsonPathSection>();



        public JsonPath(string path)
        {
            path = path.Trim();

            if (path.Substring(0, 4) == "lax ") { PathMode = JsonPathEvaluationMode.Lax; path = path.Substring(5); }
            if (path.Substring(0, 6) == "strict ") { PathMode = JsonPathEvaluationMode.Strinct; path = path.Substring(7); }

            Regex rxSection = new Regex("^(\\w+)(\\[\\(d+)\\])?$");

            string[] sections = path.Split('.');
            foreach (string section in sections)
            {
                Match m = rxSection.Match(section);
                if (m.Success)
                {
                    _sections.Push(new JsonPathSection() { sectionName = m.Groups[0].Value, indexerPesent = m.Groups[1].Success, indexerValue = m.Groups[2].Success ? int.Parse(m.Groups[2].Value) : -1 });
                }
                else
                    break;
            }
        }
    }

}
