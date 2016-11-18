using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJsonParser
{

    class JsonTrue : JsonValue
    {
        public override string ToString()
        {
            return "true";
        }

        //public override string ToXmlText(string parentName)
        //{
        //    return "true";
        //}

        public static new JsonTrue Parse(Queue<char> str)
        {
            if (string.Concat(str.Take(5)) == "true")
            {
                DequeueMultiple(str, 5);
                return new JsonTrue();
            }
            else throw new Exception("Unknown value. Expected: true");
        }

    }
}
