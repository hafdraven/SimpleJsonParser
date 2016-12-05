using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleJson.Core
{
    /// <summary>
    /// encapsulates the part of json path
    /// </summary>
    public class JsonPathSection
    {
        internal string sectionName;
        internal bool indexerPesent;
        internal int indexerValue;
    }

    public enum JsonPathEvaluationMode
    {
        Lax=0,
        Strict=1
    }

    /// <summary>
    /// encapsulates the json path functionality
    /// </summary>
    public class JsonPath
    {
        /// <summary>
        /// Path evaluation mode
        /// </summary>
        public JsonPathEvaluationMode PathMode = JsonPathEvaluationMode.Lax;
        /// <summary>
        /// inner section collection
        /// </summary>
        private Queue<JsonPathSection> _sections = new Queue<JsonPathSection>();

        /// <summary>
        /// Current section
        /// </summary>
        public JsonPathSection Current
        {
            get
            {
                if (_sections.Count > 0)
                    return _sections.Peek();
                else
                    return null;
            }
        }


        /// <summary>
        /// initializer constructor
        /// </summary>
        /// <param name="path">represents path</param>
        public JsonPath(string path)
        {
            path = path.Trim();

            if (path.Length > 4 && path.Substring(0, 4) == "lax ") { PathMode = JsonPathEvaluationMode.Lax; path = path.Substring(4); }
            if (path.Length > 7 && path.Substring(0, 7) == "strict ") { PathMode = JsonPathEvaluationMode.Strict; path = path.Substring(7); }

            Regex rxSection = new Regex("^([\\w\\$]+)(\\[(\\d+)\\])?$");

            string[] sections = path.Split('.');
            foreach (string section in sections)
            {
                Match m = rxSection.Match(section);
                if (m.Success)
                {
                    _sections.Enqueue(new JsonPathSection() { sectionName = m.Groups[1].Value, indexerPesent = m.Groups[2].Success, indexerValue = m.Groups[3].Success ? int.Parse(m.Groups[3].Value) : -1 });
                }
                else
                    break;
            }
        }
    }

}
