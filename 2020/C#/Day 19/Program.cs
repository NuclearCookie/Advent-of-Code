using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_19
{
    class Rule
    {
        public Rule[][] Branches;
        private HashSet<string> Possibilities;

        public string Value { get; set; }


        public int PossibilitySize => Possibilities == null ? -1 : Possibilities.First().Length;

        public bool HasFiniteResults()
        {
            if (Branches == null)
                return true;

            for(int i = 0; i < Branches.Length; ++i)
            {
                var branch = Branches[i];
                if (branch.Contains(this))
                    return false;
                for(int j = 0; j < branch.Length; ++j)
                {
                    if (!branch[j].HasFiniteResults())
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        
        public bool Matches(string value)
        {
            var cur_index = 0;
            // prime cache. This will skip the recursive calls
            ComputeValuesRecursiveWithCache();

            //ComputeValuesDepthFirstWithCache(ref cur_index, max_depth, value);
            return Possibilities.Contains(value);
        }

        public HashSet<string> ComputeValuesRecursiveWithCache()
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

            for (int i = 0; i < Branches.Length; i++)
            {
                Rule[] branch = Branches[i];
                var branch_results = new HashSet<string>();
                foreach(var rule in branch)
                {
                    var values = rule.ComputeValuesRecursiveWithCache();
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
                                new_result.Add(new_value);
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
            PartA();
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
            var matching_messages = messages.Where(message => rule_to_match.Matches(message)).Count();
            Console.WriteLine($"Matching messages: {matching_messages}");
        }

        private static void PartB()
        {
            Console.WriteLine("Part B:");
            var input = File.ReadAllText("Input/data2.txt").Split(Environment.NewLine + Environment.NewLine);
            var rules = input[0].Split(Environment.NewLine);
            var messages = input[1].Split(Environment.NewLine);

            var resolved_rules = ResolveRules(rules);
            foreach(var kvp in resolved_rules)
            {
                var rule = kvp.Value;
                if (rule.HasFiniteResults())
                {
                    rule.ComputeValuesRecursiveWithCache();
                }
            }
            // rule 0 == 8 11,
            // rule 8 = 42 | 42 8
            // rule 11 = 42 31 | 42 11 31
            // all matches must be a plural of the length of a possibility of rule 42 or 31.
            // solutions will always be 42 + (x*42, x*31). 
            // start backwards and count occurences of 31, if no longer 31, it must be 42 and total count of 42 must be > 31.

            var rule_42 = resolved_rules[42];
            var rule_31 = resolved_rules[31];
            var chunck_count = rule_42.PossibilitySize;
            Debug.Assert(chunck_count == rule_31.PossibilitySize);

            var matching_messages = new List<string>();
            foreach(var message in messages)
            {
                bool is_valid = true;
                var iterations = message.Length / chunck_count;
                var sections_31 = 0;
                bool past_section_31 = false;
                for(int i = iterations - 1; i >= 0; --i)
                {
                    var section = message.Substring(i * chunck_count, chunck_count);
                    if (rule_31.Matches(section))
                    {
                        if (past_section_31)
                        {
                            is_valid = false;
                            break;
                        }
                        sections_31++;
                    }
                    else if (rule_42.Matches(section))
                    {
                        if (!past_section_31 && sections_31 <= 0)
                        {
                            is_valid = false;
                            break;
                        }
                        past_section_31 = true;
                        sections_31--;
                    }
                    else
                    {
                        is_valid = false;
                        break;
                    }
                }
                if (sections_31 < 0 && is_valid)
                {
                    matching_messages.Add(message);
                }
            }
            Console.WriteLine($"Lines that match: {matching_messages.Count}");
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
