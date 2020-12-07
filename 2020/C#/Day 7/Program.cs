using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace Day_7
{
    class Program
    {
        private class ColoredBag : IEquatable<ColoredBag>
        {
            public string color;
            public Dictionary<ColoredBag, int> contents;

            public override int GetHashCode()
            {
                return color.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if (obj is ColoredBag bag)
                    return Equals(bag);
                return false;
            }

            public bool Equals(ColoredBag other)
            {
                if (other == null)
                    return false;
                return color.Equals(other.color);
            }

            public bool ContainsRecursive(ColoredBag bag)
            {
                if (bag.Equals(this))
                    return true;
                if (contents == null)
                    return false;
                foreach(var entry in contents)
                {
                    if (entry.Key.ContainsRecursive(bag))
                    {
                        return true;
                    }
                }
                return false;
            }

            public int CountChildBagsRecursive()
            {
                var result = CountChildBagsRecursiveInternal();
                // exclude ourselves
                result -= 1;
                return result;
            }
            public int CountChildBagsRecursiveInternal()
            {
                if (contents == null)
                    return 1;

                var count = 1;
                foreach(var entry in contents)
                {
                    count += entry.Value * entry.Key.CountChildBagsRecursiveInternal();
                }
                return count;
            }
        }

        static void Main(string[] args)
        {
            var data = File.ReadAllLines("Input/data.txt");
            var bag_contents = ParseData(data);
            var bag_to_find = "shiny gold bag";
            PartA(bag_contents, bag_to_find);
            PartB(bag_contents, bag_to_find);
        }

        private static void PartA(HashSet<ColoredBag> bag_contents, string bag_to_find)
        {
            Console.WriteLine("Part A:");
            var count = CountBagsContaining(bag_contents, bag_to_find);
            Console.WriteLine($"{count} bags contain shiny gold bag");
        }
        private static void PartB(HashSet<ColoredBag> bag_contents, string bag_to_find)
        {
            Console.WriteLine("Part B:");
            if (bag_contents.TryGetValue(new ColoredBag { color = bag_to_find }, out var bag))
            {
                var count = bag.CountChildBagsRecursive();
                Console.WriteLine($"{bag_to_find} contains {count} other bags.");
            }
            else
            {
                Console.WriteLine("Something went wrong.");
            }
        }

        private static HashSet<ColoredBag> ParseData(string[] data)
        {
            HashSet<ColoredBag> result = new HashSet<ColoredBag>();
            foreach(var rule in data)
            {
                var index = rule.IndexOf(" contain no other bags.");
                if ( index > -1)
                {
                    result.Add(new ColoredBag { color = rule.Substring(0, index).Trim(), contents = null });
                    continue;
                }
                index = rule.IndexOf(" contain ");
                Debug.Assert(index > -1);
                // bags are plural, we want singular forms
                var bagName = rule.Substring(0, index -1).Trim();
                // get the contents, minus the . at the end.
                var contents = rule.Substring(index + 9, rule.Length - (index + 9) -1);
                var split = contents.Split(',');
                var dict = new Dictionary<ColoredBag, int>(split.Length);
                foreach(var content_entry in split)
                {
                    var trimmed_entry = content_entry.Trim();
                    // Assume that we will have less than 10 content bags.
                    var number = int.Parse(trimmed_entry.Substring(0, 1));
                    var name = String.Empty;
                    // strip the plural
                    if (number > 1)
                        name = trimmed_entry.Substring(2, trimmed_entry.Length - 3);
                    else
                        name = trimmed_entry.Substring(2);
                    if (result.TryGetValue(new ColoredBag { color = name }, out var existing_element))
                    {
                        dict.Add(existing_element, number);
                    }
                    else
                    {
                        var sub_entry = new ColoredBag { color = name };
                        result.Add(sub_entry);
                        dict.Add(sub_entry, number);
                    }
                }
                if (result.TryGetValue( new ColoredBag {  color = bagName }, out var element))
                {
                    Debug.Assert(element.contents == null);
                    element.contents = dict;
                }
                else
                {
                    result.Add(new ColoredBag { color = bagName, contents = dict });
                }
            }
            return result;
        }
        private static int CountBagsContaining(HashSet<ColoredBag> bag_contents, string bag_name)
        {
            var bag_to_find = new ColoredBag { color = bag_name };
            return bag_contents.Where(bag => !bag.Equals(bag_to_find) && bag.ContainsRecursive(bag_to_find)).Count();
        }

    }
}
