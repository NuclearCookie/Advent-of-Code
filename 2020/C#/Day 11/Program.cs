using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

        private enum Direction : short
        {
            NorthWest,
            North,
            NorthEast,
            East,
            SouthEast,
            South,
            SouthWest,
            West
        };

        private struct Coord
        {
            public int X;
            public int Y;
        };

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input/data.txt");
            var row_length = input.Length;
            // All seats are empty at first
            var data = input.Select(x => x.ToCharArray().Select(x => x == 'L' ? SeatState.Empty : SeatState.Floor).ToArray()).ToArray();
            var data_a = CloneData(data);
            var data_b = CloneData(data);
            var timer = Stopwatch.StartNew();
            PartA(data_a);
            timer.Stop();
            Console.WriteLine($"Time Part A: {timer.ElapsedMilliseconds} ms");
            timer.Reset();
            timer.Start();
            PartB(data_b);
            timer.Stop();
            Console.WriteLine($"Time Part A: {timer.ElapsedMilliseconds} ms");
        }

        private static void PartA(SeatState[][] data)
        {
            Console.WriteLine("Part A:");
            while (PopulateSeats(data, max_occupied_seats: 4, count_floors: true))
            {
                //PrintSeats(data);
                //Console.Write("Next..");
                //var name = Console.Read();
            }
            var occupied_seats = data.Aggregate(0, (total, x) => total + x.Where(y => y == SeatState.Occupied).Count());
            Console.WriteLine($"Occupied seats after stabilizing: {occupied_seats}");
        }

        private static void PartB(SeatState[][] data)
        {
            Console.WriteLine("Part B:");
            while (PopulateSeats(data, max_occupied_seats: 5, count_floors: false))
            {
                //PrintSeats(data);
                //Console.Write("Next..");
                //var name = Console.Read();
            }
            var occupied_seats = data.Aggregate(0, (total, x) => total + x.Where(y => y == SeatState.Occupied).Count());
            Console.WriteLine($"Occupied seats after stabilizing: {occupied_seats}");

        }
        private static bool PopulateSeats(SeatState[][] data, int max_occupied_seats, bool count_floors)
        {
            SeatState[][] cloned_data = CloneData(data);
            var changed = false;
            for (int x = 0; x < cloned_data.Length; ++x)
            {
                for (int y = 0; y < cloned_data[x].Length; ++y)
                {
                    var occupied_seat = cloned_data[x][y];
                    if (occupied_seat == SeatState.Floor)
                        continue;
                    var occupied_neighbours_count = CalculateOccupiedNeighboursCount(cloned_data, new Coord { X = x, Y = y }, count_floors);
                    if (occupied_seat == SeatState.Occupied && occupied_neighbours_count >= max_occupied_seats)
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

        private static SeatState[][] CloneData(SeatState[][] data)
        {
            var cloned_data = new SeatState[data.Length][];
            for (int row = 0; row < cloned_data.Length; ++row)
            {
                cloned_data[row] = new SeatState[data[row].Length];
                Array.Copy(data[row], cloned_data[row], data[row].Length);
            }

            return cloned_data;
        }

        private static int CalculateOccupiedNeighboursCount(SeatState[][] data, Coord self_coord, bool count_floors)
        {
            var occupied_neighbours = Enumerable.Range((int)Direction.NorthWest, (int)Direction.West + 1)
                .Select(dir =>
                {
                    return GetFirstSeatStateInDirection(data, self_coord, (Direction)dir, count_floors);
                }).Where(neighbour_state => neighbour_state is SeatState.Occupied).Count();
            return occupied_neighbours;
        }

        private static IEnumerable<SeatState> GetNextSeatInDirection(SeatState[][] data, Coord self_coord, Direction dir)
        {
            switch (dir)
            {
                case Direction.West:
                    for (int y = self_coord.Y - 1; y >= 0; --y)
                        yield return data[self_coord.X][y];
                    yield break;
                case Direction.East:
                    for (int y = self_coord.Y + 1; y < data[self_coord.X].Length; ++y)
                        yield return data[self_coord.X][y];
                    yield break;
                case Direction.South:
                    for (int x = self_coord.X + 1; x < data.Length; ++x)
                        yield return data[x][self_coord.Y];
                    yield break;
                case Direction.North:
                    for (int x = self_coord.X - 1; x >= 0; --x)
                        yield return data[x][self_coord.Y];
                    yield break;
                case Direction.SouthWest:
                    for (int y = self_coord.Y - 1, x = self_coord.X + 1; y >= 0 && x < data.Length; --y, ++x)
                        yield return data[x][y];
                    yield break;
                case Direction.NorthWest:
                    for (int y = self_coord.Y - 1, x = self_coord.X - 1; y >= 0 && x >= 0; --y, --x)
                        yield return data[x][y];
                    yield break;
                case Direction.NorthEast:
                    for (int y = self_coord.Y + 1, x = self_coord.X -1; y < data[self_coord.X].Length && x >= 0; ++y, --x)
                        yield return data[x][y];
                    yield break;
                case Direction.SouthEast:
                    for (int y = self_coord.Y + 1, x = self_coord.X + 1; y < data[self_coord.X].Length && x < data.Length; ++y, ++x)
                        yield return data[x][y];
                    yield break;
            }
        }

        private static SeatState GetFirstSeatStateInDirection(SeatState[][] data, Coord self_coord, Direction dir, bool count_floor)
        {
            foreach (var state in GetNextSeatInDirection(data, self_coord, dir))
                if (count_floor || state != SeatState.Floor)
                    return state;
            return SeatState.Floor;
        }

        private static void PrintSeats(SeatState[][] data)
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
