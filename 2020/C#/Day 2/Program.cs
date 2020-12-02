using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_2
{
    class Program
    {
        private struct ParsedData
        {
            public short pos1;
            public short pos2;
            public char letter;
            public string password;
        }

        static Regex parser = new Regex("([0-9]+)-([0-9]+) ([a-z]): (.+)");
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("Input/data.txt");
            var parsed_data = data.Select(x =>
            {
                var match = parser.Match(x);
                Debug.Assert(match.Success);
                var groups = match.Groups;
                // group 0 is the total matched string.
                Debug.Assert(groups.Count == 5);
                return new ParsedData
                {
                    pos1 = short.Parse(groups[1].Captures[0].Value),
                    pos2 = short.Parse(groups[2].Captures[0].Value),
                    letter = groups[3].Captures[0].Value[0],
                    password = groups[4].Captures[0].Value
                };
            });
            PartA(parsed_data);
            PartB(parsed_data);
        }

        private static void PartA(IEnumerable<ParsedData> parsed_data)
        {
            Console.WriteLine($"PART A:");
            var valid_options = parsed_data.Where(x =>
            {
                var count = x.password.Count(c => c == x.letter);
                return x.pos1 <= count && count <= x.pos2;
            });
            Console.WriteLine($"Valid options: {valid_options.Count()}");
        }
        private static void PartB(IEnumerable<ParsedData> parsed_data)
        {
            Console.WriteLine($"PART B:");
            var valid_options = parsed_data.Where(x =>
            {
                var pos1 = x.pos1 - 1;
                var pos2 = x.pos2 - 1;
                var match1 = x.password.Length > pos1 && x.password[pos1] == x.letter;
                var match2 = x.password.Length > pos2 && x.password[pos2] == x.letter;
                return match1 ^ match2;
            });
            Console.WriteLine($"Valid options: {valid_options.Count()}");
        }

 
    }
}
