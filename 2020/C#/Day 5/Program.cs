using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day_5
{
    class Program
    {
        private static int[] Rows = null; 
        private static int[] Columns = null; 

        static void Main(string[] args)
        {
            Rows = Enumerable.Range(0, 128).ToArray();
            Columns = Enumerable.Range(0, 8).ToArray();

            var data = File.ReadAllLines("Input/data.txt");

            PartA(data);
            PartB(data);
        }

        private static void PartA(string[] data)
        {
            Console.WriteLine("Part A:");
            var result = -1;
            foreach (var seat in data)
            {
                result = Math.Max(result, GetSeatId(seat));
            }
            Console.WriteLine($"Highest seat ID: {result}");
        }
        private static void PartB(string[] data)
        {
            Console.WriteLine("Part B:");
            var results = new int[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                string seat = data[i];
                results[i] = GetSeatId(seat);
            }
            var free_seat = GetFreeSeat(results);
            Console.WriteLine($"Free seat: {free_seat}");
        }
 
        private static int GetFreeSeat(int[] taken_seats)
        {
            Array.Sort(taken_seats);
            //results.S
            var valid_seats = Enumerable.Range(taken_seats[0], taken_seats[taken_seats.Length - 1]);
            var free_seats = valid_seats.Except(taken_seats).ToArray();
            Debug.Assert(free_seats.Length == 1 || free_seats.Length > 2);

            if (free_seats.Length == 1 || free_seats[1] - free_seats[0] > 1)
            {
                return free_seats[0];
            }
            else if (free_seats[free_seats.Length - 1] - free_seats[free_seats.Length - 2] > 1)
            {
                return free_seats[free_seats.Length - 2];
            }
            for(int i = 1; i < free_seats.Length - 1; ++i)
            {
                if (free_seats[i] - free_seats[i-1] > 1 && free_seats[i+1] - free_seats[i] > 1)
                {
                    return free_seats[i];
                }
            }
            return -1; 
        }
        private static int GetSeatId(string seat_id)
        {
            var row = GetRow(seat_id.AsSpan().Slice(0, 7));
            var col = GetColumn(seat_id.AsSpan().Slice(7));
            return ComputeResult(row, col);
        }

        private static int ComputeResult(int row, int col)
        {
            return row * 8 + col;
        }

        private static int GetColumn(ReadOnlySpan<char> col_bsp)
        {
            Debug.Assert(col_bsp.Length == 3);
            var col_selection = Columns.AsSpan();
            foreach(var index in col_bsp)
            {
                if (index == 'L')
                {
                    col_selection = col_selection.Slice(0, col_selection.Length / 2);
                }
                else
                {
                    col_selection = col_selection.Slice(col_selection.Length / 2);
                }
            }
            Debug.Assert(col_selection.Length == 1);
            return col_selection[0];
        }

        private static int GetRow(ReadOnlySpan<char> row_bsp)
        {
            Debug.Assert(row_bsp.Length == 7);
            var row_selection = Rows.AsSpan();
            foreach(var index in row_bsp)
            {
                if (index == 'F')
                {
                    row_selection = row_selection.Slice(0, row_selection.Length / 2);
                }
                else
                {
                    row_selection = row_selection.Slice(row_selection.Length / 2);
                }
            }
            Debug.Assert(row_selection.Length == 1);
            return row_selection[0];        }
    }
}
