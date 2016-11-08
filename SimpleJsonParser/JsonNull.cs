﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleJsonParser
{
    class JsonNull : JsonValue
    {
        public override string ToString()
        {
            return "null";
        }

        public override string ToXmlText(string parentName = "root")
        {
            return "null";
        }

        public static new JsonNull Parse(Queue<char> str)
        {
            if (string.Concat(str.Take(4)) == "null")
            {
                DequeueMultiple(str, 4);
                return new JsonNull();
            }
            else throw new Exception("Unknown value. Expected: null");
        }
    }
}
