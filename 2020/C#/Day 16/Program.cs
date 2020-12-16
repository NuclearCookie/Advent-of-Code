using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_16
{
    class Program
    {
        private struct Bounds
        {
            public int Min;
            public int Max;

            public bool Contains(int value)
            {
                return value >= Min && value <= Max;
            }
        }

        private struct TicketEntry  : IEquatable<TicketEntry>
        {
            public string Name;
            public Bounds Bounds1;
            public Bounds Bounds2;

            public bool Equals(TicketEntry other)
            {
                return Name.Equals(other.Name);
            }

            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                if (obj is TicketEntry entry)
                {
                    return Equals(entry);
                }
                return false;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }

            public bool IsValidNumber(int value)
            {
                return Bounds1.Contains(value) || Bounds2.Contains(value);
            }
        }

        private static Regex ticketEntryRegex = new Regex(@"(?'id'[\w| ]+): (?'lb1'[\d]+)-(?'ub1'[\d]+) or (?'lb2'[\d]+)-(?'ub2'[\d]+)");
        static void Main(string[] args)
        {
            var data_groups = File.ReadAllText("Input/data.txt").Split(Environment.NewLine + Environment.NewLine);
            var ticket_fields = data_groups[0].Split(Environment.NewLine);
            var my_ticket = data_groups[1].Split(Environment.NewLine)[1].Split(',').Select(x => int.Parse(x)).ToArray();
            var nearby_tickets = data_groups[2].Split(Environment.NewLine).Skip(1).Select(line => line.Split(',').Select(x => int.Parse(x)).ToArray()).ToArray();
            var ticket_entries = new TicketEntry[ticket_fields.Length];
            for (int i = 0; i < ticket_fields.Length; i++)
            {
                string field = ticket_fields[i];
                var matches = ticketEntryRegex.Match(field);
                var groups = matches.Groups;
                Debug.Assert(groups.Count == 6);
                ticket_entries[i] = new TicketEntry
                {
                    Name = groups["id"].Value,
                    Bounds1 = new Bounds { Min = int.Parse(groups["lb1"].Value), Max = int.Parse(groups["ub1"].Value) },
                    Bounds2 = new Bounds { Min = int.Parse(groups["lb2"].Value), Max = int.Parse(groups["ub2"].Value) },
                };
            }
            PartA(nearby_tickets, ticket_entries);
            PartB(my_ticket, nearby_tickets, ticket_entries);
        }

        private static void PartA(int[][] nearby_tickets, TicketEntry[] ticket_entries)
        {
            Console.WriteLine("Part A:");
            var invalid_ticket_numbers = new List<int>();
            foreach (var ticket_entry in nearby_tickets)
            {
                foreach (var ticket_field in ticket_entry)
                {
                    var invalid_ticket_field = true;
                    foreach (var entry in ticket_entries)
                    {
                        if (entry.IsValidNumber(ticket_field))
                        {
                            invalid_ticket_field = false;
                            break;
                        }
                    }
                    if (invalid_ticket_field)
                    {
                        invalid_ticket_numbers.Add(ticket_field);
                    }
                }
            }
            Console.WriteLine($"ticket scanning error rate: {invalid_ticket_numbers.Sum()}");
        }
        private static void PartB(int[] my_ticket, int[][] nearby_tickets, TicketEntry[] ticket_entries)
        {
            Console.WriteLine("Part B:");
            var valid_tickets = ComputeValidTickets(my_ticket, nearby_tickets, ticket_entries);
            var transposed_ticket_data = new int[valid_tickets[0].Length][];
            for(int i = 0; i < transposed_ticket_data.Length; ++i)
            {
                var current_data = (transposed_ticket_data[i] = new int[valid_tickets.Length]);
                for(int j = 0; j < current_data.Length; ++j)
                {
                    current_data[j] = valid_tickets[j][i];
                }
            }
            var valid_ticket_entries_per_position = new List<TicketEntry>[transposed_ticket_data.Length];
            for(int i = 0; i < valid_ticket_entries_per_position.Length; i++)
            {
                valid_ticket_entries_per_position[i] = new List<TicketEntry>();
            }

            for (int i = 0; i < transposed_ticket_data.Length; i++)
            {
                var transposed_data = transposed_ticket_data[i];
                foreach (var entry in ticket_entries)
                {
                    var is_valid_entry_for_numbers = true;
                    foreach(var number in transposed_data)
                    {
                        if (!entry.IsValidNumber(number))
                        {
                            is_valid_entry_for_numbers = false;
                            break;
                        }
                    }
                    if (is_valid_entry_for_numbers)
                    {
                        valid_ticket_entries_per_position[i].Add(entry);
                    }
                }
            }

            int valid_tickets_length = valid_ticket_entries_per_position.Length;
            var sorted_valid_ticket_entries_per_position = new List<TicketEntry>[valid_tickets_length];
            // shallow copy to allow us to sort.
            Array.Copy(valid_ticket_entries_per_position, sorted_valid_ticket_entries_per_position, valid_tickets_length);
            Array.Sort(sorted_valid_ticket_entries_per_position, (x, y) => x.Count - y.Count);
            for(int i = 0; i < sorted_valid_ticket_entries_per_position.Length; i++)
            {
                Debug.Assert(sorted_valid_ticket_entries_per_position[i].Count == 1);
                var current = sorted_valid_ticket_entries_per_position[i][0];
                for(int j = i + 1; j < sorted_valid_ticket_entries_per_position.Length; j++)
                {
                    var next = sorted_valid_ticket_entries_per_position[j];
                    next.Remove(current);
                }
            }
            var indices = valid_ticket_entries_per_position.Select((x, index) => (Entry: x[0], Index: index)).Where(x => x.Entry.Name.StartsWith("departure")).Select(x => x.Index);
            var filtered_values_from_my_ticket = my_ticket.Where((x, index) => indices.Contains(index));
            var sum_of_values = filtered_values_from_my_ticket.Aggregate(1uL, (value, current) => value * (ulong)current);
            Console.WriteLine($"Sum of departure values: {sum_of_values}");
        }

        private static int[][] ComputeValidTickets(int[] my_ticket, int[][] nearby_tickets, TicketEntry[] ticket_entries)
        {
            var valid_tickets = new List<int[]>();
            valid_tickets.Add(my_ticket);
            foreach (var ticket_entry in nearby_tickets)
            {
                var invalid_ticket = false;
                foreach (var ticket_field in ticket_entry)
                {
                    var invalid_ticket_data = true;
                    foreach (var entry in ticket_entries)
                    {
                        if (entry.IsValidNumber(ticket_field))
                        {
                            invalid_ticket_data = false;
                            break;
                        }
                    }
                    if (invalid_ticket_data)
                    {
                        invalid_ticket = true;
                        break;
                    }
                }
                if (!invalid_ticket)
                {
                    valid_tickets.Add(ticket_entry);
                }

            }
            return valid_tickets.ToArray();
        }
    }
}
