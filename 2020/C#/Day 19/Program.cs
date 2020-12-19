using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_19
{
    class Rule
    {
        public Rule[][] Branches;
        public string Value { get; set; }

        private HashSet<string> Possibilities;
        
        public bool Matches(string value, int max_depth)
        {
            ComputeValuesRecursiveWithCache(max_depth);
            return Possibilities.Contains(value);
        }

        private HashSet<string> ComputeValuesRecursiveWithCache(int max_depth)
        {
            if (Possibilities != null)
                return Possibilities;

            var result = new HashSet<string>();
            if (Branches == null)
            {
                result.Add(Value);
                Possibilities = result;
                return result;
            }

            foreach(var branch in Branches)
            {
                var branch_results = new HashSet<string>();
                foreach(var rule in branch)
                {
                    var values = rule.ComputeValuesRecursiveWithCache(max_depth);
                    if (branch_results.Count == 0)
                    {
                        branch_results = values;
                    }
                    else
                    {
                        var new_result = new HashSet<string>(result.Count * values.Count);
                        foreach (string entry in branch_results)
                        {
                            foreach (var value in values)
                            {
                                string new_value = entry + value;
                                if (new_value.Length > max_depth)
                                {
                                    // too deep... don't count these and just return from this branch.
                                    return new HashSet<string>();
                                }
                                else
                                {
                                    new_result.Add(new_value);
                                }
                            }
                        }
                        branch_results = new_result;
                    }
                }
                result.UnionWith(branch_results);
            }
            Possibilities = result;
            return Possibilities;
        }
    }

    class Program
    {
        private static Regex number_regex = new Regex(@"[0-9]+");

        static void Main(string[] args)
        {
            //PartA();
            PartB();
        }

        private static void PartA()
        {
            Console.WriteLine("Part A:");
            var input = File.ReadAllText("Input/data.txt").Split(Environment.NewLine + Environment.NewLine);
            var rules = input[0].Split(Environment.NewLine);
            var messages = input[1].Split(Environment.NewLine);

            var resolved_rules = ResolveRules(rules);

            var rule_to_match = resolved_rules[0];
            var matching_messages = messages.Where(message => rule_to_match.Matches(message, int.MaxValue)).Count();
            Console.WriteLine($"Matching messages: {matching_messages}");
        }

        private static void PartB()
        {
            Console.WriteLine("Part B:");
            var input = File.ReadAllText("Input/test2.txt").Split(Environment.NewLine + Environment.NewLine);
            var rules = input[0].Split(Environment.NewLine);
            var messages = input[1].Split(Environment.NewLine);

            var resolved_rules = ResolveRules(rules);

            var rule_to_match = resolved_rules[0];
            var highest_char_count = messages.OrderBy(message => message.Length).Last().Length;
            var matching_messages = messages.Where(message => rule_to_match.Matches(message, highest_char_count)).Count();
            Console.WriteLine($"Matching messages: {matching_messages}");
        }
        private static Dictionary<int, Rule> ResolveRules(string[] rules)
        {
            var result = new Dictionary<int, Rule>();
            for (int i = 0; i < rules.Length; i++)
            {
                string rule_str = rules[i].Replace("\"", string.Empty);
                var colon = rule_str.IndexOf(':');
                var id = int.Parse(rule_str.AsSpan().Slice(0, colon));
                // there's a space after the colon, ignore it..
                var value = rule_str.Substring(colon + 2);
                Rule rule = null;
                if (!result.TryGetValue(id, out rule))
                {
                    rule = new Rule();
                    result[id] = rule;
                }
                rule.Value = value;
                ParseValue(result, rule, value);
            }
            return result;
        }

        private static void ParseValue(Dictionary<int, Rule> result, Rule rule, string value, int branch_index = 0)
        {
            var split_index = value.IndexOf('|');
            if (split_index > -1)
            {
                rule.Branches = new Rule[2][];
                ParseValue(result, rule, value.Substring(0, split_index), 0);
                ParseValue(result, rule, value.Substring(split_index + 1), 1);
            }
            else
            {
                var matches = number_regex.Matches(value);
                // end point.
                if (matches.Count == 0)
                {
                    rule.Branches = null;
                    return;
                }
                if (rule.Branches == null)
                {
                    rule.Branches = new Rule[1][];
                }
                rule.Branches[branch_index] = new Rule[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                {
                    var match = matches[i];
                    var id = int.Parse(match.Value);
                    Rule sub_rule = null;
                    if (!result.TryGetValue(id, out sub_rule))
                    {
                        sub_rule = new Rule();
                        result[id] = sub_rule;
                    }
                    rule.Branches[branch_index][i] = sub_rule;
                }
            }
        }
    }
}
