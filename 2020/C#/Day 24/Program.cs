using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day_24
{
    struct Coord : IEquatable<Coord>
    {
        public int x;
        public int y;
        public int z;

        public bool Equals(Coord other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (obj is Coord coord)
                return Equals(coord);
            return false;
        }

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord { x = a.x + b.x, y = a.y + b.y, z = a.z + b.z };
        }

        public static bool operator ==(Coord a, Coord b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Coord a, Coord b)
        {
            return !(a == b);
        }

        public override string ToString()
        {
            return $"x: {x}, y: {y}, z: {z}";
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }
    }

    class Program
    {
        private static Regex navigation_regex = new Regex("(e|se|sw|w|nw|ne)");

        private enum Direction
        {
            East,
            SouthEast,
            SouthWest,
            West,
            NorthWest,
            NorthEast
        };

        private enum Color
        {
            Black,
            White
        }

        static Coord[] cube_directions = new Coord[]
        {
            new Coord{x=1, y=-1, z=0},
            new Coord{x=1, y=0, z=-1},
            new Coord{ x=0, y=1, z=-1 },
            new Coord{x=-1, y=1, z=0},
            new Coord{x=-1, y=0, z=1 },
            new Coord{x=0, y=-1, z=1},
        };

        private static List<List<Direction>> direction_list = new List<List<Direction>>();
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("Input/data.txt");
            direction_list.Capacity = lines.Length;
            foreach(var line in lines)
            {
                var matches = navigation_regex.Matches(line);
                Debug.Assert(matches.Count > 0);
                var directions = new List<Direction>(matches.Count- 1);
                for(var i = 0; i < matches.Count; ++i)
                {
                    var match = matches[i];
                    Debug.Assert(match.Groups.Count == 2);
                    var direction_str = match.Groups[1].Value;
                    var direction = Direction.East;
                    switch(direction_str)
                    {
                        case "e":
                            direction = Direction.East;
                            break;
                        case "se":
                            direction = Direction.SouthEast;
                            break;
                        case "sw":
                            direction = Direction.SouthWest;
                            break;
                        case "w":
                            direction = Direction.West;
                            break;
                        case "nw":
                            direction = Direction.NorthWest;
                            break;
                        case "ne":
                            direction = Direction.NorthEast;
                            break;
                        default:
                            Debug.Assert(false, "Should not get here!");
                            break;
                    }
                    directions.Add(direction);
                }
                direction_list.Add(directions);
            }
            Dictionary<Coord, Color> tiles = new Dictionary<Coord, Color>();
            foreach(var direction_set in direction_list)
            {
                var coord = GetTargetCoordinates(direction_set);
                var color = GetTileColor(tiles, coord);
                tiles[coord] = color == Color.Black ? Color.White : Color.Black;
            }
            Console.WriteLine("Part A:");
            var black_tiles = tiles.Where(kvp => kvp.Value == Color.Black).Count();
            Console.WriteLine($"Black tiles: {black_tiles}");
            for(int i = 0; i < 100; ++i)
            {
                var tiles_copy = tiles.SelectMany(kvp => GetNeighbours(kvp.Key).Append(kvp.Key)).Distinct().ToDictionary(coord => coord, coord => GetTileColor(tiles, coord));
                foreach(var tile in tiles_copy)
                {
                    var neighbours = GetNeighbours(tile.Key);
                    var black_neighbours = neighbours.Where(cube =>
                        {
                            return GetTileColor(tiles_copy, cube) == Color.Black;
                        }).Count();

                    if (tile.Value == Color.Black && (black_neighbours == 0 || black_neighbours > 2))
                    {
                        tiles[tile.Key] = Color.White;
                    }
                    else if  (tile.Value == Color.White && black_neighbours == 2)
                    {
                        tiles[tile.Key] = Color.Black;
                    }
                }
            }
            Console.WriteLine("Part B:");
            black_tiles = tiles.Where(kvp => kvp.Value == Color.Black).Count();
            Console.WriteLine($"Black tiles: {black_tiles}");
        }

        private static Color GetTileColor(Dictionary<Coord, Color> tiles, Coord cube)
        {
            var color = Color.White;
            if (tiles.TryGetValue(cube, out var _color))
            {
                color = _color;
            }
            return color;
        }

        private static Coord[] GetNeighbours(Coord tile)
        {
            var neighbours = new Coord[cube_directions.Length];
            for (int i = 0; i < cube_directions.Length; i++)
            {
                neighbours[i] = tile + cube_directions[i];
            }
            return neighbours;
        }

        // use box coords, easier for neighbouring: https://www.redblobgames.com/grids/hexagons/
        private static Coord GetTargetCoordinates(List<Direction> direction_set)
        {
            var coord = new Coord();
            foreach(var dir in direction_set)
            {
                switch (dir)
                {
                    case Direction.East:
                        coord.x += 1;
                        coord.y -= 1;
                        break;
                    case Direction.West:
                        coord.x -= 1;
                        coord.y += 1;
                        break;
                    case Direction.SouthEast:
                        coord.y -= 1;
                        coord.z += 1;
                        break;
                    case Direction.NorthEast:
                        coord.x += 1;
                        coord.z -= 1;
                        break;
                    case Direction.SouthWest:
                        coord.x -= 1;
                        coord.z += 1;
                        break;
                    case Direction.NorthWest:
                        coord.y += 1;
                        coord.z -= 1;
                        break;
                }
            }
            return coord;
        }
    }
}
