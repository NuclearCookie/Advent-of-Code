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
            PartA();
            //PartB();
        }

        private static void PartA()
        {
            Console.WriteLine("Part A:");
            var data = File.ReadAllText("Input/data.txt").Select(x => (int)char.GetNumericValue(x)).ToList();
            var max_cup = data.Count;
            var linked_list = new LinkedList<int>(data);
            Play(linked_list, rounds: 100);
            var label_node = linked_list.Find(1);
            var result = new StringBuilder();
            for (var i = 1; i < data.Count; ++i)
            {
                label_node = label_node.Next;
                if (label_node == null)
                    label_node = linked_list.First;
                result.Append(label_node.Value);
            }
            Console.WriteLine($"Game result: {result.ToString()}");
        }

        private static void PartB()
        {
            Console.WriteLine("Part B:");
            var data = File.ReadAllText("Input/test.txt").Select(x => (int)char.GetNumericValue(x));
            var max_cup = data.Count();
            data = data.Concat(Enumerable.Range(max_cup + 1, 1000000 - max_cup));
            var linked_list = new LinkedList<int>(data);
            Play(linked_list, rounds: 10000000);
            var node_label = linked_list.Find(1);
            var result = 1uL;
            for (var i = 0; i < 2; ++i)
            {
                node_label = node_label.Next;
                result *= (ulong)node_label.Value;
            }
            Console.WriteLine($"Game result: {result}");
        }

        private static void Play(LinkedList<int> data, int rounds)
        {
            var max_cup = data.Count;
            var current_cup_node = data.First;
            var picked_up_cups = new LinkedListNode<int>[3];
            var picked_up_cup_values = new int[3];
            for (int round = 0; round < rounds; ++round)
            {
                var next = current_cup_node.Next;
                for (int i = 0; i < 3; ++i)
                {
                    if (next == null)
                        next = data.First;
                    picked_up_cups[i] = next;
                    picked_up_cup_values[i] = next.Value;
                    var to_remove = next;
                    next = next.Next;
                    data.Remove(to_remove);
                }

                LinkedListNode<int> next_cup_node = null;
                var next_cup = current_cup_node.Value;
                while (next_cup_node == null)
                {
                    next_cup--;
                    if (next_cup < 1)
                    {
                        next_cup = max_cup;
                    }
                    if (Array.IndexOf(picked_up_cup_values, next_cup) == -1)
                        next_cup_node = data.Find(next_cup);
                }
                for(int i = 0; i < 3; ++i)
                {
                    data.AddAfter(next_cup_node, picked_up_cups[i]);
                    next_cup_node = next_cup_node.Next;
                }
                current_cup_node = current_cup_node.Next;
                if (current_cup_node == null)
                    current_cup_node = data.First;
            }
        }
    }
}
