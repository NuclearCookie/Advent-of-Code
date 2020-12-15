using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day_15
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("Input/data.txt");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            PartA(input);
            stopwatch.Stop();
            Console.WriteLine($"elapsed time: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();
            stopwatch.Start();
            PartB(input);
            stopwatch.Stop();
            Console.WriteLine($"elapsed time: {stopwatch.ElapsedMilliseconds}ms");
        }

        private static void PartA(string input)
        {
            Console.WriteLine("Part A:");
            var start_input = input.Split(',');
            var numbers_map = start_input.Select((x, i) => (number: ulong.Parse(x), index: (ulong)i)).ToDictionary(x => x.number, x => x.index);
            var start_index = (ulong)start_input.Length;
            var last_spoken_number = Play(numbers_map, start_index, 2020uL);
            Console.WriteLine($"The 2020th spoken number is {last_spoken_number}");
        }

        private static void PartB(string input)
        {
            Console.WriteLine("Part B:");
            var start_input = input.Split(',');
            var numbers_map = start_input.Select((x, i) => (number: ulong.Parse(x), index: (ulong)i)).ToDictionary(x => x.number, x => x.index);
            var start_index = (ulong)start_input.Length;
            var last_spoken_number = Play(numbers_map, start_index, 30000000uL);
            Console.WriteLine($"The 30000000th spoken number is {last_spoken_number}");
        }
        private static ulong Play(Dictionary<ulong, ulong> numbers_map, ulong start_index, ulong end_index)
        {
            var last_spoken_number = 0uL;
            end_index -= 1;
            for (ulong i = start_index; i < end_index; ++i)
            {
                ulong new_number = 0;
                if (numbers_map.TryGetValue(last_spoken_number, out var previous_index))
                    new_number = i - previous_index;
                numbers_map[last_spoken_number] = i;
                last_spoken_number = new_number;
            }

            return last_spoken_number;
        }
    }
}
