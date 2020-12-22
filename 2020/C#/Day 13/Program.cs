using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Day_13
{
    class Program
    {
        
        static int minimumTimestamp = 0;
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input/data.txt");
            PartA(input);
            PartBChineseRemainderTheorem(input); // nope... 
        }

        private static void PartA(string[] input)
        {
            Console.WriteLine("Part A:");
            minimumTimestamp = int.Parse(input[0]);
            var busses = input[1].Split(',').Where(id => id[0] != 'x').Select(id => int.Parse(id)).ToList();
            var first_bus_id = 0;
            int current_timestamp = FindFirstBus(busses, minimumTimestamp, ref first_bus_id);

            int time_since_arrival = current_timestamp - minimumTimestamp;
            Console.WriteLine($"The first bus to take is bus {first_bus_id}, leaving {time_since_arrival} minutes after you arrive. Result = {first_bus_id * time_since_arrival}");
        }

        private static void PartBBruteForce(string[] input)
        {
            Console.WriteLine("Part B:");
            var busses = input[1].Split(',').Select((id, index) =>
            {
                var parsed_id = -1;
                if (int.TryParse(id, out var result))
                    parsed_id = result;
                return ( Index: index, Id: parsed_id );
            }).Where(bus => bus.Id >= 0).ToList();
            var success = false;
            void SetSuccess(bool value)
            {
                success = value;
            }
            bool GetSuccess()
            {
                return success;
            }
            var highest_bus = busses.OrderBy((bus) => -bus.Id).First();
            var result = Parallel.ForEach(IterateUntilTrue(highest_bus, GetSuccess), (current_timestamp, state) =>
            {
                foreach (var bus in busses)
                {
                    if ((current_timestamp + bus.Index) % bus.Id != 0)
                    {
                        return;
                    }
                }
                state.Break();
                Console.WriteLine($"Earliest timestamp where all busses match requirements: {current_timestamp}");
                SetSuccess(true);
            });
        }
        private static IEnumerable<long> IterateUntilTrue((int Index, int Id) highest_bus, Func<bool> condition)
        {
            var current_timestamp = 100000000000000;
            while(true)
            {
                if (current_timestamp % highest_bus.Id == 0)
                {
                    current_timestamp -= highest_bus.Index;
                    break;
                }
                current_timestamp++;
            }

            //var current_timestamp = 0L;
            while (!condition()) yield return current_timestamp += highest_bus.Id;
        }

        private static int FindFirstBus(List<int> busses, int minimum_timestamp, ref int first_bus_id)
        {
            var current_timestamp = minimum_timestamp;
            while (true)
            {
                foreach (var bus in busses)
                {
                    if (current_timestamp % bus == 0)
                    {
                        first_bus_id = bus;
                        return current_timestamp;
                    }
                }
                current_timestamp++;
            }
        }
        private static void PartBChineseRemainderTheorem(string[] input)
        {
            Console.WriteLine("Part B:");
            var busses = input[1].Split(',').Select((id, index) =>
            {
                var parsed_id = -1;
                if (int.TryParse(id, out var result))
                    parsed_id = result;
                return (Index: index, Id: parsed_id);
            }).Where(bus => bus.Id >= 0).ToList();
            long product_of_all_numbers = busses.Aggregate(1L, (product, bus) => product * bus.Id);
            Console.WriteLine($" Total Product: {product_of_all_numbers}");
            long sum = 0;
            for (int i = 1; i < busses.Count; i++)
            {
                var bus = busses[i];
                Console.Write($"bus: {bus}");
                long partial_product = product_of_all_numbers / bus.Id;
                Console.Write($" Partial Product: {partial_product}");
                long inverse = ComputeInverse(partial_product, bus.Id);
                Console.WriteLine($" Inverse: {inverse}");
                sum += partial_product * inverse * (bus.Id - bus.Index);
            }

            Console.WriteLine($" Sum: {sum}");
            var smallest = sum % product_of_all_numbers;
            Console.WriteLine($"Timestamp: {smallest}");
        }

        // https://medium.com/free-code-camp/how-to-implement-the-chinese-remainder-theorem-in-java-db88a3f1ffe0
        public static long ComputeInverse(long a, long b)
        {
            long m = b;
            long t;
            long q;
            long x = 0;
            long y = 1;

            if (b == 1)
                return 0;

            //Extended Euclidian algorithm?
            while (a > 1)
            {
                // q is quotient
                q = a / b;
                t = b;

                // new proceed same as Euclid's algorithm
                b = a % b;
                a = t;
                t = x;
                x = y - q * x;
                y = t;
            }

            // Make x1 positive
            if (y < 0)
                y += m;
            return y;
        }

        private static int GCDExtended(int a, int b, out int x, out int y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            var gcd = GCDExtended(b % a, a, out var x1, out var y1);
            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }
    }
}
