using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day_10
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("Input/data.txt").Select(x => int.Parse(x)).ToList();
            //var data = File.ReadAllLines("Input/test1.txt").Select(x => int.Parse(x)).ToList();
            //var data = File.ReadAllLines("Input/test2.txt").Select(x => int.Parse(x)).ToList();
            data.Add(0);
            data.Sort();
            data.Add(data[data.Count - 1] + 3); 
            PartA(data);
            PartB(data);
        }

        private static void PartA(List<int> data)
        {
            var differences = new int[data.Count];
            for (int i = 0; i < data.Count - 1; i++)
            {
                differences[i] = data[i + 1] - data[i];
            }
            differences[differences.Length - 1] = 3;
            var one_jolt = differences.Where(jolt => jolt == 1).Count();
            var three_jolt = differences.Where(jolt => jolt == 3).Count();
            Console.WriteLine($"1-jolt: {one_jolt}, 3-jolt: {three_jolt}. Result: {one_jolt * three_jolt}");
        }

        // I couldn't figure this out by myself. 
        // I thought about 
        // * reverse looping
        // * conditionally summing per branch
        // But I couldn't put it together. 
        // Inspiration taken here: 
        // https://github.com/JamieMagee/AdventOfCode/blob/master/src/2020/AdventOfCode.2020.Puzzles/Solutions/Day10.cs
        private static void PartB(List<int> data)
        {
            var permutations_per_index = new Dictionary<int, long>();
            permutations_per_index.Add(data.Count - 1, 1);
            for (int i = data.Count - 2; i >= 0; --i)
            {
                long permutations = 0;
                for (int j = i + 1; j < data.Count && data[j] - data[i] <= 3; ++j)
                {
                    permutations += permutations_per_index[j];
                }
                permutations_per_index[i] = permutations;
            }
            Console.WriteLine($"Permutations: {permutations_per_index[0]}");
        }
    }
}
