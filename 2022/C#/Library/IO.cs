using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class IO
    {
        public static string ReadInputAsString(bool isTest = false)
        {
            string filePath = GetFilePath(isTest);
            return File.ReadAllText(filePath);
        }

        public static IEnumerable<string> ReadInputAsStringArray(bool isTest = false)
        {
            string filePath = GetFilePath(isTest);
            return File.ReadAllLines(filePath);
        }

        public static IEnumerable<int> ReadInputAsIntArray(bool isTest = false)
        {
            return ReadInputAsStringArray().Select(line => int.Parse(line));
        }

        private static string GetFilePath(bool isTest)
        {
            var args = Environment.GetCommandLineArgs();
            var filePath = isTest ? "Input/test.txt" : "Input/data.txt";
            if (args.Length > 1)
            {
                filePath = args[1];
            }

            return filePath;
        }
    }
}
