using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day_20
{
    public enum EdgeOrientation
    { 
        Invalid = -1,
        Top = 0,
        TopFlipped,
        Left,
        LeftFlipped,
        Bottom,
        BottomFlipped,
        Right,
        RightFlipped,
        Count
    };

    public static class EdgeOrientationExtensions
    {
        public static bool IsFlipped(this EdgeOrientation edge)
        {
            return ((int)edge) % 2 == 1;
        }
        public static EdgeOrientation GetOppositeEdgeOrientation(this EdgeOrientation edge)
        {
            return (EdgeOrientation)((int)(edge + 4) % (int)EdgeOrientation.Count);
        }

        public static EdgeOrientation GetUnFlipped(this EdgeOrientation edge)
        {
            if ((int)edge % 2 == 1)
                return edge - 1;
            return edge;
        }

        public static int GetQuartersToMove(this EdgeOrientation edge, EdgeOrientation target)
        {
            var simplified_self = edge.GetUnFlipped();
            var simplified_target = target.GetUnFlipped();
            if (simplified_self == simplified_target) 
                return 0;

            switch (simplified_self)
            {
                case EdgeOrientation.Left:
                {
                    switch(simplified_target)
                    {
                        case EdgeOrientation.Top:
                            return 1;
                        case EdgeOrientation.Right:
                            return 2;
                        case EdgeOrientation.Bottom:
                            return 3;
                    }
                }
                break;
                case EdgeOrientation.Top:
                {
                    switch(simplified_target)
                    {
                        case EdgeOrientation.Right:
                            return 1;
                        case EdgeOrientation.Bottom:
                            return 2;
                        case EdgeOrientation.Left:
                            return 3;
                    }
                }
                break;
                case EdgeOrientation.Right:
                {
                    switch(simplified_target)
                    {
                        case EdgeOrientation.Bottom:
                            return 1;
                        case EdgeOrientation.Left:
                            return 2;
                        case EdgeOrientation.Top:
                            return 3;
                    }
                }
                break;
                case EdgeOrientation.Bottom:
                {
                    switch(simplified_target)
                    {
                        case EdgeOrientation.Left:
                            return 1;
                        case EdgeOrientation.Top:
                            return 2;
                        case EdgeOrientation.Right:
                            return 3;
                    }
                }
                break;
            }
            throw new Exception("Should not happen");
        }
    }

    class ImageData
    {
        public int Id { get; private set; }
        public List<ImageData> Connections { get; private set; }  = new List<ImageData>();
        public bool IsCorner => Connections.Count == 2;
        public bool IsEdge => Connections.Count == 3;
        public bool IsCenter => Connections.Count == 4;

        private int[][] Data
        {
            get => _data;
            set { _data = value; _EdgesCache = null; }
        }
        private int[][] _data;
        private static Regex tile_id_regex = new Regex(@"Tile ([0-9]+):");

        public int[,] GetSeaPart()
        {
            var result = new int[Data.Length - 2, Data.Length - 2];
            for (var x = 1; x < Data.Length - 1; ++x)
                for (var y = 1; y < Data.Length - 1; ++y)
                    result[x - 1, y - 1] = Data[x][y];
            return result;
        }
        public ImageData Parse(string[] data)
        {
            var match = tile_id_regex.Match(data[0]);
            Debug.Assert(match.Success && match.Groups.Count == 2);
            Id = int.Parse(match.Groups[1].Value);

            Data = new int[data.Length - 1][];
            for(int i = 1; i < data.Length; ++i)
            {
                Data[i - 1] = data[i].Select(x => x == '.' ? 0 : 1).ToArray();
            }
            return this;
        }

        private int[] _EdgesCache;
        public int[] Edges 
        { 
            get
            {
                if (_EdgesCache != null)
                    return _EdgesCache;

                _EdgesCache = new int[8];
                var top = Data[0].Select(x => x.ToString());
                var top_flipped = top.Reverse();
                var left = Data.Select(x => x[0].ToString());
                var left_flipped = left.Reverse();
                var bottom = Data[^1].Select(x => x.ToString());
                var bottom_flipped = bottom.Reverse();
                var right = Data.Select(x => x[^1].ToString());
                var right_flipped = right.Reverse();
                _EdgesCache[(int)EdgeOrientation.Top] = int.Parse(string.Join(string.Empty, top));
                _EdgesCache[(int)EdgeOrientation.TopFlipped] = int.Parse(string.Join(string.Empty, top_flipped));
                _EdgesCache[(int)EdgeOrientation.Left] = int.Parse(string.Join(string.Empty, left));
                _EdgesCache[(int)EdgeOrientation.LeftFlipped] = int.Parse(string.Join(string.Empty, left_flipped));
                _EdgesCache[(int)EdgeOrientation.Bottom] = int.Parse(string.Join(string.Empty, bottom));
                _EdgesCache[(int)EdgeOrientation.BottomFlipped] = int.Parse(string.Join(string.Empty, bottom_flipped));
                _EdgesCache[(int)EdgeOrientation.Right] = int.Parse(string.Join(string.Empty, right));
                _EdgesCache[(int)EdgeOrientation.RightFlipped] = int.Parse(string.Join(string.Empty, right_flipped));
                return _EdgesCache;
            } 
        }

        public List<EdgeOrientation> GetMatchingEdgesNoFlip()
        {
            var result = new List<EdgeOrientation>();
            foreach(var connection in Connections)
            {
                var matches_with_flip = GetMatchingEdges(connection);
                if ((int)matches_with_flip[0] % 2 == 0)
                {
                    result.Add(matches_with_flip[0]);
                }
                else
                {
                    result.Add(matches_with_flip[1]);
                }
            }
            return result;
        }

        public List<EdgeOrientation> GetMatchingEdges(ImageData other)
        {
            var result = new List<EdgeOrientation>();
            foreach(var edge in other.Edges)
            {
                var index = Array.IndexOf(Edges, edge);
                if (index > -1)
                    result.Add((EdgeOrientation)index);
            }
            return result;
        }

        
        public int GetOppositeEdge(EdgeOrientation edge)
        {
            return Edges[(int)edge.GetOppositeEdgeOrientation()];
        }
        public ImageData GetOppositePiece(EdgeOrientation edge)
        {
            var my_edge = Edges[(int)edge];
            var opposite_edge = edge.GetOppositeEdgeOrientation();
            foreach(var neighbour in Connections)
            {
                if (neighbour.Edges[(int)opposite_edge] == my_edge)
                {
                    return neighbour;
                }
            }
            return null;
        }

        public void FlipVertical()
        {
            Data = Data.Reverse().ToArray();
        }

        public void FlipHorizontal()
        {
            Data = Data.Select(row => row.Reverse().ToArray()).ToArray();
        }

        public ImageData GetConnection(EdgeOrientation direction)
        {
            var target_edge = Edges[(int)direction];
            ImageData matching_neighbour = null;
            EdgeOrientation neighbour_orientation = EdgeOrientation.Invalid;
            foreach(var neighbour in Connections)
            {
                int edge_index = Array.IndexOf(neighbour.Edges, target_edge);
                if (edge_index > -1)
                {
                    matching_neighbour = neighbour;
                    neighbour_orientation = (EdgeOrientation)edge_index;
                    break;
                }
            }
            if (matching_neighbour == null)
                return matching_neighbour;
            // rotate neighbour _until_ that connection matches
            // It would be cleaner to rotate directly to the matching side but f* it!
            var neighbour_target_orientation = direction.GetOppositeEdgeOrientation();
            matching_neighbour.ChangeOrientation(target_edge, neighbour_target_orientation);
            Debug.Assert(matching_neighbour.Edges[(int)neighbour_target_orientation] == target_edge);
            return matching_neighbour;
        }

        private void ChangeOrientation(int target_on_edge, EdgeOrientation target_orientation)
        {
            if (target_on_edge == Edges[(int)target_orientation])
                return;
            for(int i = 0; i < 4; ++i)
            {
                RotateClockwise(1);
                if (target_on_edge == Edges[(int)target_orientation])
                    return;
            }
            FlipVertical();
            for(int i = 0; i < 4; ++i)
            {
                RotateClockwise(1);
                if (target_on_edge == Edges[(int)target_orientation])
                    return;
            }
            throw new Exception("Should not get here.");
        }

        private void RotateClockwise(int quarters_to_move)
        {
            if (quarters_to_move == 0)
                return;

            if (quarters_to_move == 2)
            {
                FlipHorizontal();
                FlipVertical();
            }
            else
            {
                var length = Data.Length;
                Debug.Assert(length == Data[0].Length);
                var new_data = new int[length][];
                if (quarters_to_move == 1)
                {
                    for (int x = 0; x < length; ++x)
                    {
                        new_data[x] = new int[length];
                        for (int y = 0; y < length; ++y)
                        {
                            new_data[x][y] = Data[length - y - 1][x];
                        }
                    }
                }
                else
                {
                    for (int x = 0; x < length; ++x)
                    {
                        new_data[x] = new int[length];
                        for (int y = 0; y < length; ++y)
                        {
                            new_data[x][y] = Data[y][length - x - 1];
                        }
                    }
                }
                Data = new_data;
            }
        }

        public static string ToString(int[,] data)
        {
            var builder = new StringBuilder();
            for(var x = 0; x < data.GetLength(0); ++x)
            {
                for(var y = 0; y < data.GetLength(1); ++y )
                {
                    var index = data[x, y];
                    builder.Append(index == 1 ? '#' : '.');
                }
                builder.AppendLine();
            }
            return builder.ToString();

        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine($"Tile {Id}:");
            for(var x = 0; x < Data.Length; ++x)
            {
                var row = Data[x];
                for(var y = 0; y < row.Length; ++y )
                {
                    var index = row[y];
                    builder.Append(index == 1 ? '#' : '.');
                }
                builder.AppendLine();
            }
            return builder.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("Input/data.txt").Split(Environment.NewLine + Environment.NewLine);
            var pieces = new ImageData[input.Length];
            for(int i = 0; i < input.Length; ++i)
            {
                var data = input[i].Split(Environment.NewLine);
                pieces[i] = new ImageData().Parse(data);
            }
            for(int i = 0; i < pieces.Length; ++i)
            {
                var a = pieces[i];
                //for(int j = i + 1; j < pieces.Length; ++j)
                for (int j = 0; j < pieces.Length; ++j)
                {
                    var b = pieces[j];
                    if (a == b)
                        continue;
                    var overlapping_edges = a.GetMatchingEdges(b).Count;
                    //Console.WriteLine($"Overlapping edges between piece {a.Id} and {b.Id}: {overlapping_edges}");
                    // edges contain both flipped and normal, so we'll match twice for each side.
                    if (overlapping_edges > 0)
                    {
                        pieces[i].Connections.Add(pieces[j]);
                    }
                }
            }

            var corners = pieces.Where(piece => piece.IsCorner).ToArray();
            var corner_count = corners.Length;
            var multiplied_corner_ids = corners.Aggregate(1uL, (sum, piece) => sum * (ulong)piece.Id);
            Console.WriteLine($"Total corners: {corner_count}");
            Console.WriteLine($"Aggregated corner totals : {multiplied_corner_ids}");
            var edges = pieces.Where(piece => piece.IsEdge).ToArray();
            var edges_count = edges.Length;
            var center = pieces.Where(piece => piece.IsCenter).ToArray();
            var center_count = center.Length;
            Console.WriteLine($"Total edges: {edges_count}");
            Console.WriteLine($"Total center pieces: {center_count}");
            //Console.WriteLine()
            var width = (int)Math.Sqrt(pieces.Length);
            var grid = Puzzle(corners[0], width, width);
            Debug.Assert((ulong)grid[0, 0].Id * (ulong)grid[grid.GetLength(0) - 1, 0].Id * (ulong)grid[0, grid.GetLength(1) - 1].Id * (ulong)grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1].Id == multiplied_corner_ids);
            var sea = GenerateSea(grid);
            var sea_monster_count = FindSeaMonstersInAllOrientations(sea);
            Console.WriteLine($"Amount of seamonsters: {sea_monster_count}");
            var waves = (from int cell in sea
                         where cell == 1
                         select cell).Count();
            // a seamonster is 15 waves.
            waves -= (sea_monster_count * 15);
            Console.WriteLine($"Roughness of sea: {waves}");
        }

        private static int FindSeaMonstersInAllOrientations(int[,] sea)
        {
            // rotate clockwise
            var sea_length = sea.GetLength(0);
            for (int i = 0; i < 4; ++i)
            {
                sea = RotateSea(sea, sea_length);
                var monsters = FindSeaMonsters(sea);
                if (monsters > 0)
                    return monsters;
            }
            sea = MirrorSea(sea, sea_length);
            for (int i = 0; i < 4; ++i)
            {
                sea = RotateSea(sea, sea_length);
                var monsters = FindSeaMonsters(sea);
                if (monsters > 0)
                    return monsters;
            }

            return 0;

            static int[,] RotateSea(int[,] sea, int sea_length)
            {
                var new_sea = new int[sea_length, sea_length];
                for (int x = 0; x < sea_length; ++x)
                {
                    for (int y = 0; y < sea_length; ++y)
                    {
                        new_sea[x, y] = sea[sea_length - y - 1, x];
                    }
                }
                sea = new_sea;
                return sea;
            }

            static int[,] MirrorSea(int[,] sea, int sea_length)
            {
                var new_sea = new int[sea_length, sea_length];
                for (int x = 0; x < sea_length; ++x)
                    for (int y = 0; y < sea_length; ++y)
                    {
                        new_sea[x, y] = sea[x, sea_length - 1 - y];
                    }
                sea = new_sea;
                return sea;
            }
        }

        private static int FindSeaMonsters(int[,] sea)
        {
            var monsters = 0;
            for(int x = 0; x < sea.GetLength(0) - 20; ++x)
                for(int y = 0; y < sea.GetLength(1) - 3; ++y)
                {
                    var window = GetSeaSubSection(sea, x, y);
                    if (IsSeaMonster(window))
                        monsters++;
                }
            return monsters;
        }

        private static bool IsSeaMonster(int[,] window)
        {
            //                  # 
            //#    ##    ##    ###
            // #  #  #  #  #  #   
            if (window[0, 18] + window[1, 0] +
                window[1, 5] + window[1, 6] + window[1, 11] + window[1, 12] + window[1, 17] + window[1, 18] + window[1, 19] +
                window[2, 1] + window[2, 4] + window[2, 7] + window[2, 10] + window[2, 13] + window[2, 16] == 15)
                return true;
            return false;
        }

        private static int[,] GetSeaSubSection(int[,] sea, int start_x, int start_y)
        {
            var window = new int[3, 20];
            for (int x = 0; x < 20; ++x)
                for (int y = 0; y < 3; ++y)
                    window[y, x] = sea[y + start_y, x + start_x];
            return window;
        }
        private static int[,] GenerateSea(ImageData[,] grid)
        {
            var x_section = grid.GetLength(0);
            var y_section = grid.GetLength(1);
            var section_length = grid[0, 0].GetSeaPart().GetLength(0); // remove edges
            var result = new int[section_length * x_section, section_length * y_section];
            for(var x = 0; x < grid.GetLength(0); ++x)
                for(var y = 0; y < grid.GetLength(1); ++y)
                {
                    var data = grid[x, y];
                    var sea_part = data.GetSeaPart();
                    var part_size = sea_part.GetLength(0);
                    for (var sea_part_x = 0; sea_part_x < part_size; ++sea_part_x)
                        for (var sea_part_y = 0; sea_part_y < part_size; ++sea_part_y)
                            result[y * part_size + sea_part_x, x * part_size + sea_part_y] = sea_part[sea_part_x, sea_part_y];
                }

            return result;
        }

        private static ImageData[,] Puzzle(ImageData left_top_corner, int width, int height)
        {
            ImageData[,] puzzle_grid = new ImageData[width, height];
            // rotate the image data so that the top left corner has connecting edges on bottom and right.
            RotateTopLeftCorner(left_top_corner);
            puzzle_grid[0, 0] = left_top_corner;
            // fill top row:
            var current = left_top_corner;
            for(int x = 1; x < width; ++x)
            {
                Debug.Assert(current.GetConnection(EdgeOrientation.Top) == null && current.GetConnection(EdgeOrientation.TopFlipped) == null);
                current = current.GetConnection(EdgeOrientation.Right);
                puzzle_grid[x, 0] = current;
            }
            current = left_top_corner;
            for(int y = 1; y < height; ++y)
            {
                current = puzzle_grid[0, y - 1];
                current = current.GetConnection(EdgeOrientation.Bottom);
                puzzle_grid[0, y] = current;
                for(int x = 1; x < width; ++x)
                {
                    current = current.GetConnection(EdgeOrientation.Right);
                    puzzle_grid[x, y] = current;
                }
            }
            return puzzle_grid;
        }

        private static void RotateTopLeftCorner(ImageData left_top_corner)
        {
            var edges = left_top_corner.GetMatchingEdgesNoFlip();
            // we need to have edges on bottom and right, for the top left piece.
            foreach (var edge in edges)
            {
                switch (edge)
                {
                    case EdgeOrientation.Top:
                        left_top_corner.FlipVertical();
                        break;
                    case EdgeOrientation.Left:
                        left_top_corner.FlipHorizontal();
                        break;
                }
            }
            var corrected_edges = left_top_corner.GetMatchingEdgesNoFlip();
            Debug.Assert((corrected_edges[0] == EdgeOrientation.Right || corrected_edges[0] == EdgeOrientation.Bottom)
                && (corrected_edges[1] == EdgeOrientation.Right || corrected_edges[1] == EdgeOrientation.Bottom));
        }
    }
}
