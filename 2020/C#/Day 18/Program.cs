using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day_18
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input/data.txt");
            PartA(input);
            PartB(input);
        }

        private static void PartA(string[] input)
        {
            Console.WriteLine("Part A:");
            var results = new long[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                string expression = input[i];
                var result = ProcessExpression(expression.AsSpan(), out var end_index, with_operator_precedence: false);
                //Console.WriteLine($"Solution: {result}");
                results[i] = result;
                Debug.Assert(end_index == expression.Length);
            }
            Console.WriteLine($"Total sum of all expressions: {results.Sum()}");
        }

        private static void PartB(string[] input)
        {
            Console.WriteLine("Part B:");
            var results = new long[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                string expression = input[i];
                var result = ProcessExpression(expression.AsSpan(), out var end_index, with_operator_precedence: true);
                //Console.WriteLine($"Solution: {result}");
                results[i] = result;
                Debug.Assert(end_index == expression.Length);
            }
            Console.WriteLine($"Total sum of all expressions: {results.Sum()}");
        }

        private static long Sum(long lhs, long rhs)
        {
            return lhs + rhs;
        }

        private static long Multiply(long lhs, long rhs)
        {
            return lhs * rhs;
        }

        private static long ProcessExpression(ReadOnlySpan<char> readOnlySpan, out int end_index, bool with_operator_precedence)
        {
            var current_index = 0;
            long lhs = -1;
            Func<long, long, long> op = null;

            void AssignNumericValue(long number)
            {
                if (lhs > -1)
                {
                    Debug.Assert(op != null);
                    lhs = op(lhs, number);
                    op = null;
                }
                else
                {
                    Debug.Assert(op == null);
                    lhs = number;
                }
            };

            while (current_index < readOnlySpan.Length)
            {
                var character = readOnlySpan[current_index];
                current_index++;
                if (character == ' ')
                    continue;
                else if (character == '(')
                {
                    var result = ProcessExpression(readOnlySpan.Slice(current_index), out var sub_end_index, with_operator_precedence);
                    // skip the closing ) character
                    current_index += sub_end_index + 1;
                    AssignNumericValue(result);
                }
                else if (character == ')')
                {
                    current_index--;
                    break;
                }
                else if (character == '*')
                {
                    op = Multiply;
                    if (with_operator_precedence)
                    {
                        var result = ProcessExpression(readOnlySpan.Slice(current_index), out var sub_end_index, with_operator_precedence);
                        current_index += sub_end_index;
                        AssignNumericValue(result);
                    }
                }
                else if (character == '+')
                    op = Sum;
                else
                {
                    Debug.Assert(char.IsDigit(character));
                    AssignNumericValue(character - '0');
                }
            }
            end_index = current_index;
            return lhs;
        }
    }
}
