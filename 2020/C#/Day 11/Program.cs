using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day_11
{
    class Program
    {
        private enum SeatState : short
        {
            Floor,
            Empty,
            Occupied
        }
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input/data.txt");
            var row_length = input.Length;
            // All seats are empty at first
            var data = input.Select(x => x.ToCharArray().Select(x => x == 'L' ? SeatState.Empty : SeatState.Floor).ToArray()).ToArray();
            while (PopulateSeats(data)) 
            {
                //PrintSeats(data, row_length);
                //Console.Write("Next..");
                //var name = Console.Read();
            }
            var occupied_seats = data.Aggregate(0, (total, x) => total + x.Where(y => y == SeatState.Occupied).Count());
            Console.WriteLine($"Occupied seats after stabilizing: {occupied_seats}");
        }

        private static bool PopulateSeats(SeatState[][] data)
        {
            var cloned_data = new SeatState[data.Length][];
            for(int row = 0; row < cloned_data.Length; ++row)
            {
                cloned_data[row] = new SeatState[data[row].Length];
                Array.Copy(data[row], cloned_data[row], data[row].Length);
            }
            var changed = false;
            for(int x = 0; x < cloned_data.Length; ++x)
            {
                for (int y = 0; y < cloned_data[x].Length; ++y)
                {
                    var occupied_seat = cloned_data[x][y];
                    if (occupied_seat == SeatState.Floor)
                        continue;
                    var occupied_neighbours_count = CalculateOccupiedNeighboursCount(cloned_data, x, y);
                    if (occupied_seat == SeatState.Occupied && occupied_neighbours_count >= 4)
                    {
                        data[x][y] = SeatState.Empty;
                        changed = true;
                    }
                    else if (occupied_seat == SeatState.Empty && occupied_neighbours_count == 0)
                    {
                        data[x][y] = SeatState.Occupied;
                        changed = true;
                    }
                }
            }
            return changed;
        }

        private static int CalculateOccupiedNeighboursCount(SeatState[][] data, int self_coord_x, int self_coord_y)
        {
            var x_start = self_coord_x - 1;
            var x_end = self_coord_x + 1;
            var y_start = self_coord_y - 1;
            var y_end = self_coord_y + 1;
            var occupied_neighbour_count = 0;
            for(var x = x_start; x <= x_end; ++x)
            {
                if (x < 0 || x >= data.Length)
                    continue;
                for(var y = y_start; y <= y_end; ++y)
                {
                    if (y < 0 || y >= data[x].Length)
                        continue;

                    // skip self
                    if (x == self_coord_x && y == self_coord_y)
                        continue;

                    if (data[x][y] == SeatState.Occupied)
                    {
                        occupied_neighbour_count++;
                    }
                }
            }
            return occupied_neighbour_count;
        }

        private static void PrintSeats(SeatState[][] data, int row_length)
        {
            var builder = new StringBuilder();
            for(int x = 0; x < data.Length; ++x)
            {
                builder.AppendLine();
                for (int y = 0; y < data[x].Length; ++y)
                {
                    switch (data[x][y])
                    {
                        case SeatState.Empty:
                            builder.Append('L');
                            break;
                        case SeatState.Floor:
                            builder.Append('.');
                            break;
                        case SeatState.Occupied:
                            builder.Append('#');
                            break;
                    }
                }
            }
            Console.WriteLine(builder.ToString());
        }
    }
}
