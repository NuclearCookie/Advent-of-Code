using System.Linq;
using System.IO;
using System;

namespace Day_1
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("Input/data.txt");
            var int_data = data.Select(x => int.Parse(x)).ToArray();
            PartA(int_data);
            PartB(int_data);
        }

        private static void PartA(int[] int_data)
        {
            Console.WriteLine("PART A:");
            for (int i = 0; i < int_data.Length; ++i)
            {
                var current = int_data[i];
                for (int j = i + 1; j < int_data.Length; ++j)
                {
                    var next = int_data[j];
                    if (current + next == 2020)
                    {
                        Console.WriteLine($"1: {current} 2: {next}. result: {current * next}");
                        return;
                    }
                }
            }
        }
        private static void PartB(int[] int_data)
        {
            Console.WriteLine("PART B:");
            Array.Sort(int_data);
            var ordered = int_data;
            for (int i = 0; i < ordered.Length; ++i)
            {
                var current = int_data[i];
                for (int j = i + 1; j < ordered.Length; ++j)
                {
                    var next = int_data[j];
                    for (int k = j + 1; k < ordered.Length; ++k)
                    {
                        var next_next = int_data[k];
                        if (current + next + next_next == 2020)
                        {
                            Console.WriteLine($"1: {current} 2: {next} 3: {next_next}. result: {current * next * next_next}");
                            return;
                        }
                     }
               }
            }
        }    
    }
}
