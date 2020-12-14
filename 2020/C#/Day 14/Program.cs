//#define DEBUG_LOG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_14
{
    class Program
    {
        private static Regex lineParser = new Regex(@"mem\[([0-9]+)\] = ([0-9]+)");
        private static Dictionary<int, ulong> memoryTable = new Dictionary<int, ulong>();

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input/data.txt");

            ulong on_mask = 0;
            // 64 bit to 36 bit conversion..
            ulong off_mask = ulong.MaxValue >> 28;

            for(int i = 0; i < input.Length; ++i)
            {
#if DEBUG_LOG
                Console.WriteLine();
                Console.WriteLine(input[i]);
                Console.WriteLine();
#endif
                // mask
                if (input[i].StartsWith("mask = "))
                {
                    on_mask = 0;
                    // 64 bit to 36 bit conversion..
                    off_mask = ulong.MaxValue >> 28;
                    var bitmask = input[i].Split(" = ")[1];
                    for (int j = 0; j < bitmask.Length; ++j)
                    {
                        char bit = bitmask[j];
                        int bit_index = (bitmask.Length - j - 1);
                        switch (bit)
                        {
                            case '0':
                                off_mask -= 1uL << bit_index;
                                break;
                            case '1':
                                on_mask += 1uL << bit_index;
                                break;
                        }
                    }
                }
                else
                {
                    var result = lineParser.Match(input[i]);
                    Debug.Assert(result.Groups.Count == 3);
                    var index = int.Parse(result.Groups[1].Value);
                    var value = ulong.Parse(result.Groups[2].Value);
#if DEBUG_LOG
                    Console.WriteLine($"value:  {Convert.ToString((long)value, 2).PadLeft(36, '0')}");
                    Console.WriteLine($"mask:   {Convert.ToString((long)(off_mask + on_mask), 2).PadLeft(36, '0')}");
#endif
                    value &= off_mask;
                    value |= on_mask;
#if DEBUG_LOG
                    Console.WriteLine($"result: {Convert.ToString((long)value, 2).PadLeft(36, '0')}");
#endif
                     memoryTable[index] = value;
                }
            }

            var sum = memoryTable.Aggregate(0uL, (sum, kvp) => sum + kvp.Value);
            Console.WriteLine($"Sum of all values: {sum}");
        }
    }
}
