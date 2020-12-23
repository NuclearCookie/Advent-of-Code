using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Day_23
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            PartA();
            stopwatch.Stop();
            Console.WriteLine($"duration: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();
            stopwatch.Start();
            PartB();
            stopwatch.Stop();
            Console.WriteLine($"duration: {stopwatch.ElapsedMilliseconds}ms");
        }

        private static void PartA()
        {
            Console.WriteLine("Part A:");
            var data = File.ReadAllText("Input/data.txt").Select(x => (int)char.GetNumericValue(x)).ToList();
            var max_cup = data.Count;
            Console.WriteLine("Start converting data.");
            var converted_data = new List<int>(data.Count);
            for (int i= 0; i < data.Count; ++i)
            {
                converted_data.Add(data[(data.IndexOf(i + 1) + 1) % data.Count]);
            }
            Console.WriteLine("Done converting data.");

            var indexed_data = Play(converted_data, data[0], rounds: 100);
            var next_label = indexed_data[1 - 1];
            var result = new StringBuilder();
            for (var i = 1; i < indexed_data.Count; ++i)
            {
                result.Append(next_label);
                next_label = indexed_data[next_label - 1];
            }
            Console.WriteLine($"Game result: {result}");
        }

        private static void PartB()
        {
            Console.WriteLine("Part B:");
            var data = File.ReadAllText("Input/data.txt").Select(x => (int)char.GetNumericValue(x)).ToList();
            var max_cup = data.Count;
            Console.WriteLine("Start converting data.");
            var converted_data = new List<int>(10000000);
            for (int i= 0; i < data.Count; ++i)
            {
                converted_data.Add(data[(data.IndexOf(i + 1) + 1) % data.Count]);
            }
            Console.WriteLine("Done converting data.");

            var extended_converted_data = converted_data.Concat(Enumerable.Range(max_cup + 2, 1000000 - max_cup)).ToList();
            var warp_around_label = data[data.Count - 1];
            var first_label = data[0];
            extended_converted_data[warp_around_label - 1] = data.Count + 1;
            extended_converted_data[extended_converted_data.Count - 1] = first_label;
            var indexed_data = Play(extended_converted_data, first_label, rounds: 10000000);
            var next_label = indexed_data[1 - 1];
            var result = 1uL;
            for (var i = 1; i < 3; ++i)
            {
                result *= (ulong)next_label;
                next_label = indexed_data[next_label - 1];
            }
            Console.WriteLine($"Game result: {result}");
        }

        private static List<int> Play(List<int> converted_data, int start_index, int rounds)
        {
            // convert the data so that arr[label] == clockwise of label
            var max_cup = converted_data.Count;
            var current_cup_index = start_index;
            var picked_up_cups = new int[3];
            for (int round = 0; round < rounds; ++round)
            {
                var current_cup = current_cup_index;
                var next = current_cup_index;
                for (int i = 0; i < 3; ++i)
                {
                    next = converted_data[next - 1];
                    picked_up_cups[i] = next;
                }
                // eliminate all 3 picked up cups.
                converted_data[current_cup_index - 1] = converted_data[next-1];

                var next_cup = current_cup;
                while (true)
                {
                    next_cup--;
                    if (next_cup < 1)
                    {
                        next_cup = max_cup;
                    }
                    if (Array.IndexOf(picked_up_cups, next_cup) == -1)
                        break;
                }
                var after_reinsert = converted_data[next_cup - 1];
                converted_data[next_cup - 1] = picked_up_cups[0];
                converted_data[picked_up_cups[2] - 1] = after_reinsert;
                current_cup_index = converted_data[current_cup_index - 1];
            }
            return converted_data;
        }
    }
}
