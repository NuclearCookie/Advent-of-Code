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
            var data = input.SelectMany(x => x.ToCharArray().Select(x => x == 'L' ? SeatState.Empty : SeatState.Floor)).ToArray();
            while (PopulateSeats(data, row_length)) 
            {
                //PrintSeats(data, row_length);
                //Console.Write("Next..");
                //var name = Console.Read();
            }
            var occupied_seats = data.Where(x => x == SeatState.Occupied).Count();
            Console.WriteLine($"Occupied seats after stabilizing: {occupied_seats}");
        }

        private static bool PopulateSeats(SeatState[] data, int row_length)
        {
            var cloned_data = new SeatState[data.Length];
            var changed = false;
            Array.Copy(data, cloned_data, data.Length);
            for(int index = 0; index < cloned_data.Length; ++index)
            {
                var occupied_seat = cloned_data[index];
                if (occupied_seat == SeatState.Floor)
                    continue;
                var occupied_neighbours_count = CalculateOccupiedNeighboursCount(cloned_data, index, row_length);
                if (occupied_seat == SeatState.Occupied && occupied_neighbours_count >= 4)
                {
                    data[index] = SeatState.Empty;
                    changed = true;
                }
                else if (occupied_seat == SeatState.Empty && occupied_neighbours_count == 0)
                {
                    data[index] = SeatState.Occupied;
                    changed = true;
                }
            }
            return changed;
        }

        private static int CalculateOccupiedNeighboursCount(SeatState[] data, int index, int row_length)
        {
            var self_coord_x = index % row_length;
            var self_coord_y = index / row_length;
            var x_start = self_coord_x - 1;
            var x_end = self_coord_x + 1;
            var y_start = self_coord_y - 1;
            var y_end = self_coord_y + 1;
            var occupied_neighbour_count = 0;
            for(var x = x_start; x <= x_end; ++x)
            {
                if (x < 0 || x >= row_length)
                    continue;
                for(var y = y_start; y <= y_end; ++y)
                {
                    if (y < 0 || y >= data.Length / row_length)
                        continue;

                    // skip self
                    if (x == self_coord_x && y == self_coord_y)
                        continue;

                    if (data[x+(row_length * y)] == SeatState.Occupied)
                    {
                        occupied_neighbour_count++;
                    }
                }
            }
            return occupied_neighbour_count;
        }

        private static void PrintSeats(SeatState[] data, int row_length)
        {
            var builder = new StringBuilder();
            for(int i = 0; i < data.Length; ++i)
            {
                if (i % row_length == 0)
                    builder.AppendLine();
                switch (data[i])
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
            Console.WriteLine(builder.ToString());
        }
    }
}
