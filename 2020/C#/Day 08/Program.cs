using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Day_08
{
    class Program
    {
        private static Regex parseRegex = new Regex(@"([a-z]{3}) (\+|-)([0-9]+)");
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("Input/data.txt");
            PartA(data);
            PartB(data);
        }

        private static void PartA(string[] data)
        {
            Console.WriteLine("Part A:");
            Process(data, out int accumulative_value);
            Console.WriteLine($"Accumulative value: {accumulative_value}");
        }
        private static void PartB(string[] data)
        {
            Console.WriteLine("Part B:");
            string[] modified_data = new string[data.Length];
            Array.Copy(data, modified_data, data.Length);
            int accumulative_value = 0;
            for (int i = 0; i < data.Length; i++)
            {
                string instruction = data[i];
                if (instruction.StartsWith("nop"))
                {
                    modified_data[i] = data[i].Replace("nop", "jmp");
                }
                else if (instruction.StartsWith("jmp"))
                {
                    modified_data[i] = data[i].Replace("jmp", "nop");
                }
                else
                {
                    continue;
                }
                if (Process(modified_data, out accumulative_value))
                {
                    break;
                }
                modified_data[i] = data[i];
            }
            Console.WriteLine($"Accumulative value: {accumulative_value}");
        }

        private static bool Process(string[] data, out int total_value)
        {
            total_value = 0;
            var visited_locs = new HashSet<int>(data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                if (!visited_locs.Add(i))
                {
                    return false;
                }
                string instruction = data[i];
                var match = parseRegex.Match(instruction);
                Debug.Assert(match.Success);
                Debug.Assert(match.Groups.Count == 4);
                var op = match.Groups[1].Value;
                var sign = match.Groups[2].Value[0];
                var amount = int.Parse(match.Groups[3].Value);
                var accumulation = GetSign(sign) * amount;
                switch (op)
                {
                    case "nop":
                        break;
                    case "acc":
                        total_value += accumulation;
                        break;
                    case "jmp":
                        i += accumulation - 1;
                        break;
                }
            }
            return true;
        }

        static int GetSign(char sign)
        {
            return (sign == '+' ? 1 : -1);
        }
    }
}
