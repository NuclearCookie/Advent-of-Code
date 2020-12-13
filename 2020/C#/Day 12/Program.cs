using System;
using System.Collections.Generic;
using System.IO;

namespace Day_12
{
    class Program
    {
        private struct Coord
        {
            public int X;
            public int Y;

            public static readonly Coord East = new Coord { X = 1, Y = 0 };
            public static readonly Coord West = new Coord { X = -1, Y = 0 };
            public static readonly Coord North = new Coord { X = 0, Y = -1 };
            public static readonly Coord South = new Coord { X = 0, Y = 1 };

            public static Coord operator +(Coord a, Coord b)
            {
                return new Coord { X = a.X + b.X, Y = a.Y + b.Y };
            }

            public static Coord operator *(int a, Coord b)
            {
                return new Coord { X = a * b.X, Y = a * b.Y };
            }

            public static bool operator ==(Coord a, Coord b)
            {
                return a.X == b.X && a.Y == b.Y;
            }

            public static bool operator !=(Coord a, Coord b)
            {
                return !(a == b);
            }
            
            public static Coord GetNavigationMapping(char dir)
            {
                if (dir == 'N') return North;
                else if (dir == 'S') return South;
                else if (dir == 'E') return East;
                else if (dir == 'W') return West;
                throw new Exception("Unsupported direction");
            }

            public static char GetCoordMapping(Coord coord)
            {
                if (coord == North) return 'N';
                else if (coord == South) return 'S';
                else if (coord == East) return 'E';
                else if (coord == West) return 'W';
                throw new Exception("Unsupported direction");
            }

            public static Coord TurnRight(Coord coord, int degrees)
            {
                var amount = degrees / 90;
                var temp = new Coord();
                for(int i = 0; i < amount; ++i)
                {
                    temp.X = -1 * coord.Y;
                    temp.Y = coord.X;
                    coord = temp;
                }
                return coord;
            }
  
            public static Coord TurnLeft(Coord coord, int degrees)
            {
                var amount = degrees / 90;
                var temp = new Coord();
                for(int i = 0; i < amount; ++i)
                {
                    temp.X = coord.Y;
                    temp.Y = -1 * coord.X;
                    coord = temp;
                }
                return coord;
            }

            public int GetManhattanCoordinates()
            {
                return Math.Abs(X) + Math.Abs(Y);
            }

            public override string ToString()
            {
                return $"X: {X}, Y: {Y}";
            }
        };


        static void Main(string[] args)
        {
            var input = File.ReadAllLines("Input/data.txt");
            PartA(input);
            PartB(input);
        }

        private static void PartA(string[] input)
        {
            Console.WriteLine("Part A:");
            Coord shipPosition = new Coord { X = 0, Y = 0 };
            Coord shipOrientation = Coord.East;
            
            foreach (var nagivation_command in input)
            {
                var command = nagivation_command[0];
                var amount = int.Parse(nagivation_command.AsSpan(1));
                if (command == 'R')
                {
                    shipOrientation = Coord.TurnRight(shipOrientation, amount);
                    continue;
                }
                else if (command == 'L')
                {
                    shipOrientation = Coord.TurnLeft(shipOrientation, amount);
                    continue;
                }

                if (command == 'F')
                {
                    command = Coord.GetCoordMapping(shipOrientation);
                }
                shipPosition += amount * Coord.GetNavigationMapping(command);
            }
            Console.WriteLine($"Ship manhattan pos: {shipPosition.GetManhattanCoordinates()}");
        }
        private static void PartB(string[] input)
        {
            Console.WriteLine("Part B:");
            Coord waypoint = new Coord { X = 10, Y = -1 };
            Coord shipPosition = new Coord { X = 0, Y = 0 };

            foreach (var nagivation_command in input)
            {
                var command = nagivation_command[0];
                var amount = int.Parse(nagivation_command.AsSpan(1));
                if (command == 'F')
                {
                    shipPosition += amount * waypoint;
                }
                else if (command == 'N' || command == 'E' || command == 'S' || command == 'W')
                {
                    waypoint += amount * Coord.GetNavigationMapping(command);
                }
                else if (command == 'R')
                {
                    waypoint = Coord.TurnRight(waypoint, amount);
                }
                else if (command == 'L')
                {
                    waypoint = Coord.TurnLeft(waypoint, amount);
                }
            }
            Console.WriteLine($"Ship manhattan pos: {shipPosition.GetManhattanCoordinates()}");
        }
    }
}
