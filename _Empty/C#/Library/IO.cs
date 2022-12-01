using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class IO
    {
        public static IEnumerable<string> ReadInputAsStringArray(bool isTest = false)
        {
            var args = Environment.GetCommandLineArgs();
            var filePath = isTest ? "Input/test.txt" : "Input/data.txt";
            if (args.Length > 1)
            {
                filePath = args[1];
            }
            return File.ReadAllLines(filePath);
        }

        public static IEnumerable<int> ReadInputAsIntArray()
        {
            return ReadInputAsStringArray().Select(line => int.Parse(line));
        }
    }
}
