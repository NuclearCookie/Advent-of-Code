using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_6
{
    class Program
    {
        private static readonly char[] AllOptions = Enumerable.Range((int)'a', (int)'z').Select(c => (char)c).ToArray();

        static void Main(string[] args)
        {
            var data = File.ReadAllText("Input/data.txt");
            var groups = data.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            PartA(groups);
            PartB(groups);
        }

        private static void PartA(string[] groups)
        {
            Console.WriteLine("Part A:");
            var answers_per_group = groups
                .Select(group => group.Replace(Environment.NewLine, "").Distinct())
                .Aggregate(0, (total, group) => total + group.Count());
            Console.WriteLine($"Sum of any positive answer for all groups: {answers_per_group}");
        }

        private static void PartB(string[] groups)
        {
            Console.WriteLine("Part B:");
            var answers_per_group = groups
                .Select(group =>
                {
                    var line_per_person = group.Split(Environment.NewLine);
                    IEnumerable<char> result = AllOptions;
                    foreach(var person in line_per_person)
                    {
                        result = result.Intersect(person);
                    }
                    return result;
                })
                .Aggregate(0, (total, group) => total + group.Count());
            Console.WriteLine($"Sum of all positive answers for all groups: {answers_per_group}");
        }
    }
}
