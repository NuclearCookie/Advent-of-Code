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

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input/data.txt");
            PartA(input);
            PartB(input);
        }

        private static void PartA(string[] input)
        {
            Console.WriteLine("Part A:");
            Dictionary<int, ulong> memoryTable = new Dictionary<int, ulong>();
            ulong on_mask = 0;
            // 64 bit to 36 bit conversion..
            ulong off_mask = ulong.MaxValue >> 28;
            string mask_string = "";
            for (int i = 0; i < input.Length; ++i)
            {
                // mask
                if (input[i].StartsWith("mask = "))
                {
                    mask_string = ComputeBitMask(input[i], out on_mask, out off_mask, out _);
                }
                else
                {
                    var result = lineParser.Match(input[i]);
                    Debug.Assert(result.Groups.Count == 3);
                    var index = int.Parse(result.Groups[1].Value);
                    var value = ulong.Parse(result.Groups[2].Value);
#if DEBUG_LOG
                    Console.WriteLine($"value:  {Convert.ToString((long)value, 2).PadLeft(36, '0')}");
                    Console.WriteLine($"mask:   {mask_string}");
#endif
                    value &= off_mask;
                    value |= on_mask;
#if DEBUG_LOG
                    Console.WriteLine($"result: {Convert.ToString((long)value, 2).PadLeft(36, '0')}");
                    Console.WriteLine();
#endif
                    memoryTable[index] = value;
                }
            }

            var sum = memoryTable.Aggregate(0uL, (sum, kvp) => sum + kvp.Value);
            Console.WriteLine($"Sum of all values: {sum}");
        }

        private static void PartB(string[] input)
        {
            Console.WriteLine("Part B:");
            Dictionary<ulong, ulong> memoryTable = new Dictionary<ulong, ulong>();
            ulong on_mask = 0;
            ulong floating_mask = 0;
            // 64 bit to 36 bit conversion..
            ulong off_mask = ulong.MaxValue >> 28;
            string mask_string = "";

            for (int i = 0; i < input.Length; ++i)
            {
                // mask
                if (input[i].StartsWith("mask = "))
                {
                    mask_string = ComputeBitMask(input[i], out on_mask, out _, out floating_mask);
                }
                else
                {
                    var result = lineParser.Match(input[i]);
                    Debug.Assert(result.Groups.Count == 3);
                    var index = ulong.Parse(result.Groups[1].Value);
                    var value = ulong.Parse(result.Groups[2].Value);
#if DEBUG_LOG
                    Console.WriteLine($"address:  {Convert.ToString((long)index, 2).PadLeft(36, '0')} (decimal {index})");
                    Console.WriteLine($"mask:     {mask_string}");
#endif
                    index |= on_mask;
#if DEBUG_LOG
                    var string_value = new System.Text.StringBuilder(Convert.ToString((long)index, 2).PadLeft(36, '0'));
                    for (int bit_i = 0; bit_i < 36; ++bit_i)
                    {
                        var bit_to_test = (1uL << bit_i);
                        if ((floating_mask & bit_to_test) == bit_to_test)
                        {
                            string_value[string_value.Length - 1 - bit_i] = 'X';
                        }
                    }
                    Console.WriteLine($"result:   {string_value}");
                    Console.WriteLine($"Modified memory addresses:");
#endif
                    AddFloatingPermutation(memoryTable, floating_mask, value, index, 0);
}
            }
            var sum = memoryTable.Aggregate(0uL, (sum, kvp) => sum + kvp.Value);
            Console.WriteLine($"Sum of all values: {sum}");
        }

        // Yes.. we're processing too many elements, but I don't care, it's fast!
        private static void AddFloatingPermutation(in Dictionary<ulong, ulong> memoryTable, in ulong floating_mask, in ulong value, ulong index, int bit_i)
        {
#if DEBUG_LOG
            if (!memoryTable.TryGetValue(index, out var cached_value) || cached_value != value)
                Console.WriteLine($"{Convert.ToString((long)index, 2).PadLeft(36, '0')} (decimal {index})");
#endif
            memoryTable[index] = value;
            for (; bit_i < 36; ++bit_i)
            {
                var bit_to_test = (1uL << bit_i);
                if ((floating_mask & bit_to_test) == bit_to_test)
                {
                    var bit_state_at_index = index & bit_to_test;
                    if (bit_state_at_index != 0)
                    {
                        AddFloatingPermutation(memoryTable, floating_mask, value, index - bit_to_test, bit_i + 1);
                        AddFloatingPermutation(memoryTable, floating_mask, value, index, bit_i + 1);
                    }
                    else
                    {
                        AddFloatingPermutation(memoryTable, floating_mask, value, index + bit_to_test, bit_i + 1);
                        AddFloatingPermutation(memoryTable, floating_mask, value, index, bit_i + 1);
                    }
                }
            }
        }

        private static string ComputeBitMask(string mask_line, out ulong on_mask, out ulong off_mask, out ulong floating_mask)
        {
            on_mask = 0;
            // 64 bit to 36 bit conversion..
            off_mask = ulong.MaxValue >> 28;
            floating_mask = 0;
            var bitmask = mask_line.Split(" = ")[1];
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
                    case 'X':
                        floating_mask += 1uL << bit_index;
                        break;
                }
            }
            return bitmask;
        }
    }
}
