using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SimpleJsonParser;

namespace JsonParserTest
{

    class Program
    {

        static void Main(string[] args)
        {
            string str = System.IO.File.ReadAllText("input.json");
            JsonValue v = JsonValue.Parse(str);
            Console.WriteLine(v.Query("$.a").ToString());
            Console.ReadLine();
        }

        
    }
}
