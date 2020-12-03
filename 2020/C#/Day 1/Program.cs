#define PROFILING
#if !PROFILING
#define NO_PROFILING
#endif

using System.Linq;
using System.IO;
using System;
using System.Diagnostics;

namespace Day_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("Input/data.txt");
            var int_data = data.Select(x => int.Parse(x)).ToArray();
            // It's much more likely to find a match in the lower numbers, presort for speed.
            Array.Sort(int_data);
            Run("PART A", PartA, int_data);
            Run("PART A LINQ", PartALINQ, int_data);
            Run("PART B", PartB, int_data);
            Run("PART B LINQ", PartBLINQ, int_data);
        }

        private static void Run(string identifier, Action<int[]> action, int[] int_data)
        {
#if PROFILING
            var watch = Stopwatch.StartNew();
            for(int i = 0; i < 10000; i++)
#endif
                action(int_data);
#if PROFILING
            watch.Stop();
            Console.WriteLine($"{identifier} Time: {watch.ElapsedMilliseconds}");
#endif
 }

        private static void PartALINQ(int[] int_data)
        {
            WriteLine("PART A (LINQ):");

            var value1 = 0;
            var value2 = 0;
            int_data
                .Where((x, i) =>
                {
                    return int_data.Skip(i + 1).Where((y) =>
                    {
                        if (x + y == 2020)
                        {
                            value1 = x;
                            value2 = y;
                            return true;
                        }
                        return false;
                    }).Any();
                }).Single();
            WriteLine($"1: {value1} 2: {value2}. result: {value1 * value2}.");
        }

        private static void PartB(int[] int_data)
        {
            WriteLine("PART B:");
            for (int i = 0; i < int_data.Length; ++i)
            {
                var current = int_data[i];
                for (int j = i + 1; j < int_data.Length; ++j)
                {
                    var next = int_data[j];
                    for (int k = j + 1; k < int_data.Length; ++k)
                    {
                        var next_next = int_data[k];
                        if (current + next + next_next == 2020)
                        {
                            WriteLine($"1: {current} 2: {next} 3: {next_next}. result: {current * next * next_next}.");
                            return;
                        }
                     }
               }
            }
        }    
        private static void PartBLINQ(int[] int_data)
        {
            WriteLine("PART B (LINQ):");

            var value1 = 0;
            var value2 = 0;
            var value3 = 0;
            var result = int_data
                .Where((x, i) =>
                {
                    return int_data.Skip(i + 1).Where((y, j) =>
                    {
                        return int_data.Skip(j + 1).Where((z) =>
                        {
                            if (x + y + z == 2020)
                            {
                                value1 = x;
                                value2 = y;
                                value3 = z;
                                return true;
                            }
                            return false;
                        }).Any();
                    }).Any();
                }).First();
            WriteLine($"1: {value1} 2: {value2} 3: {value3}. result: {value1 * value2 * value3}.");
        }


        [Conditional("NO_PROFILING")]
        private static void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
}
