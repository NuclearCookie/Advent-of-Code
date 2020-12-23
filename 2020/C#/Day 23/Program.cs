using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day_23
{
    class Program
    {
        static void Main(string[] args)
        {
            //PartA();
            PartB();
        }

        private static void PartA()
        {
            Console.WriteLine("Part A:");
            var data = File.ReadAllText("Input/data.txt").Select(x => (int)char.GetNumericValue(x)).ToList();
            var max_cup = data.Count;
            Play(data, rounds: 100);
            var index_of_cup_1 = data.IndexOf(1);
            var result = new StringBuilder();
            for (var i = 1; i < data.Count; ++i)
            {
                result.Append(data[(i + index_of_cup_1) % data.Count]);
            }
            Console.WriteLine($"Game result: {result.ToString()}");
        }

        private static void PartB()
        {
            Console.WriteLine("Part B:");
            var data = File.ReadAllText("Input/test.txt").Select(x => (int)char.GetNumericValue(x));
            var max_cup = data.Count();
            var list = data.Concat(Enumerable.Range(max_cup + 1, 1000000 - max_cup + 1)).ToList();
            Play(list, rounds: 10000000);
            var index_of_cup_1 = list.IndexOf(1);
            var result = 1uL;
            for (var i = 1; i < 3; ++i)
            {
                result *= (ulong)list[(i + index_of_cup_1) % list.Count];
            }
            Console.WriteLine($"Game result: {result}");
        }

        private static void Play(List<int> data, int rounds)
        {
            var max_cup = data.Count;
            var current_cup_index = 0;
            var picked_up_cups = new int[3];
            var picked_up_cup_indices = new int[3];
            for (int round = 0; round < rounds; ++round)
            {
                var current_cup = data[current_cup_index];
                for (int i = 0; i < 3; ++i)
                {
                    var corrected_index = (current_cup_index + 1 + i) % data.Count;
                    picked_up_cups[i] = data[corrected_index];
                    picked_up_cup_indices[i] = corrected_index;
                }
                for (int i = 0; i < 3; ++i)
                {
                    data.RemoveAt(picked_up_cup_indices[i]);
                }

                var next_cup_index = -1;
                var next_cup = current_cup;
                while (next_cup_index == -1)
                {
                    next_cup--;
                    if (next_cup < 1)
                    {
                        next_cup = max_cup;
                    }
                    if (Array.IndexOf(picked_up_cups, next_cup) == -1)
                        next_cup_index = data.IndexOf(next_cup);
                }
                data.InsertRange(next_cup_index + 1, picked_up_cups);
                current_cup_index = data.IndexOf(current_cup) + 1;
                if (current_cup_index >= data.Count)
                    current_cup_index = 0;
            }
        }
    }
}
