using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_09
{
    class Program
    {
        static void Main(string[] args)
        {
            //var data = File.ReadAllLines("Input/test.txt").Select(x => long.Parse(x)).ToArray();
            //var invalid_number = PartA(data, preamble: 5);
            var data = File.ReadAllLines("Input/data.txt").Select(x => long.Parse(x)).ToArray();
            var invalid_number = PartA(data, preamble: 25);
            PartB(data, invalid_number);
        }

        private static long PartA(long[] data, int preamble)
        {
            for(int i = preamble; i < data.Length; ++i)
            {
                var value = data[i];
                if (!CheckValidSumsInPreamble(data.AsSpan(i - preamble, preamble), value))
                {
                    Console.WriteLine($"First invalid number: {value}");
                    return value;
                }
            }
            Console.WriteLine($"Error: No invalid numbers found.");
            return long.MinValue;
        }
        private static void PartB(long[] data, long invalid_value)
        {
            int low = 0;
            int high = 0;
            long sum = 0;
            for(int i = 0; i < data.Length; ++i)
            {
                if (sum == invalid_value && low != high)
                {
                    break;
                }

                sum += data[i];
                high = i;
                
                while(sum > invalid_value)
                {
                    sum -= data[low];
                    low++;
                }
            }
            var subdata = new long[(high - low)];
            Array.Copy(data, low, subdata, 0, high - low);
            Array.Sort(subdata);
            var lowest_value = subdata[0];
            var highest_value = subdata[subdata.Length - 1];
            Console.WriteLine($"Lowest number: {lowest_value}, Heighest number: {highest_value}. Sum: {lowest_value + highest_value}");
        }

        private static bool CheckValidSumsInPreamble(ReadOnlySpan<long> span, long value)
        {
            for(int i = 0; i < span.Length; ++i)
            {
                for(int j = i; j < span.Length; ++j)
                {
                    if (span[i] == span[j])
                        continue;
                    if (span[i] + span[j] == value)
                        return true;
                }
            }
            return false;
        }
    }
}
