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
            string str=System.IO.File.ReadAllText("input.json");
            //Console.WriteLine(str);
            Queue<char> s = new Queue<char>(str.ToCharArray());
            JsonValue v= JsonValue.Parse(s);
            Console.WriteLine(v.ToString());
            Console.ReadLine();
        }

        
    }
}
