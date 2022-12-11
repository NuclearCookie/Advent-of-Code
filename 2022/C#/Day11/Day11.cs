using Library;
using Library.Algorithms;
using System.Diagnostics;
using System.Text.RegularExpressions;

Day11.Run();

class Day11
{
    static Monkey[] monkeys;

    public static void Run()
    {
        var input = IO.ReadInputAsString(false);
        var monkeyDescriptors = input.Split("Monkey ", StringSplitOptions.RemoveEmptyEntries);
        PerformPart1(monkeyDescriptors);
        PerformPart2(monkeyDescriptors);
    }

    private static void PerformPart1(string[] monkeyDescriptors)
    {
        monkeys = new Monkey[monkeyDescriptors.Length];

        for(int i = 0; i < monkeyDescriptors.Length; i++)
        {
            monkeys[i] = new Monkey(monkeyDescriptors[i]);
        }

        var leastCommonMultiple = monkeys[0].Divisor;
        for(int i = 1; i < monkeys.Length; i++)
        {
            leastCommonMultiple = Library.Algorithms.Math.LeastCommonMultiple(monkeys[i].Divisor, leastCommonMultiple);
        }

        for(int turn = 0; turn < 20; ++turn)
        {
            for(int i = 0; i < monkeys.Length; i++)
            {
                monkeys[i].PerformTurn(true, (ulong)leastCommonMultiple);
            }           
        }

        Array.Sort(monkeys, (a, b) => (int)(b.totalInspectedItems - a.totalInspectedItems));

        Console.WriteLine($"Part 1: Amount of monkeybusiness: {monkeys[0].totalInspectedItems * monkeys[1].totalInspectedItems}");
    }

    private static void PerformPart2(string[] monkeyDescriptors)
    {
        monkeys = new Monkey[monkeyDescriptors.Length];

        for(int i = 0; i < monkeyDescriptors.Length; i++)
        {
            monkeys[i] = new Monkey(monkeyDescriptors[i]);
        }

        var leastCommonMultiple = monkeys[0].Divisor;
        for(int i = 1; i < monkeys.Length; i++)
        {
            leastCommonMultiple = Library.Algorithms.Math.LeastCommonMultiple(monkeys[i].Divisor, leastCommonMultiple);
        }


        for(int turn = 0; turn < 10000; ++turn)
        {
            for(int i = 0; i < monkeys.Length; i++)
            {
                monkeys[i].PerformTurn(false, (ulong)leastCommonMultiple);
            }           
        }

        Array.Sort(monkeys, (a, b) => (int)(b.totalInspectedItems - a.totalInspectedItems));

        Console.WriteLine($"Part 2: Amount of monkeybusiness: {(ulong)monkeys[0].totalInspectedItems * (ulong)monkeys[1].totalInspectedItems}");
    }

    class Monkey
    {
        public List<ulong> worryLevelForHeldItems = new List<ulong>();

        private string operation;
        private (int, int, int) test;

        public int totalInspectedItems = 0;

        public int Divisor => test.Item1;

        public Monkey(string monkeyDescriptor)
        {
            Regex monkeyRegex = new Regex(@"\w*Starting items: (?'items'[\d, ]+)\s+Operation: new = (?'operation'[\w*+\d ]+)\s+Test: divisible by (?'divisor'\d+)\s+If true: throw to monkey (?'trueTest'\d+)\s+If false: throw to monkey (?'falseTest'\d+)");
            var match = monkeyRegex.Match(monkeyDescriptor);
            var groups = match.Groups;
            var itemsString = groups[1].Value;
            worryLevelForHeldItems = itemsString.Split(',').Select(item => ulong.Parse(item)).ToList();
            operation = groups[2].Value;
            test = (int.Parse(groups[3].Value), int.Parse(groups[4].Value), int.Parse(groups[5].Value));
        }

        public void PerformTurn(bool divideWorryLevel, ulong leastCommonMultiple)
        {
            for (int i = 0; i < worryLevelForHeldItems.Count; i++)
            {
                totalInspectedItems++;
                ulong heldItem = worryLevelForHeldItems[i];
                heldItem = PerformOperation(heldItem);
                if (divideWorryLevel)
                    heldItem /= 3;
                var before = heldItem;
                heldItem %= leastCommonMultiple;
                //Debug.Assert(before == heldItem);

                var modulo = heldItem % (ulong)test.Item1;
                if (modulo == 0)
                {
                    monkeys[test.Item2].worryLevelForHeldItems.Add(heldItem);
                }
                else
                {
                    monkeys[test.Item3].worryLevelForHeldItems.Add(heldItem);
                }
            }
            worryLevelForHeldItems.Clear();
        }

        private ulong PerformOperation(ulong oldWorryLevel)
        {
            var operands = operation.Split(' ');
            Debug.Assert(operands[0].Equals("old"));
            ulong rightHandOperand;
            if (operands[2].Equals("old"))
            {
                rightHandOperand = oldWorryLevel;
            }
            else
            {
                rightHandOperand = ulong.Parse(operands[2]);
            }

            if (operands[1].Equals("*"))
            {
                checked
                {
                    return oldWorryLevel * rightHandOperand;
                }
            }
            else if (operands[1].Equals("+"))
            {
                return oldWorryLevel + rightHandOperand;
            }
            throw new NotImplementedException("Unsupported operation.");
        }
    }
}
