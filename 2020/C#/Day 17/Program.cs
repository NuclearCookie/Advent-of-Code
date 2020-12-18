using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Day_17
{
    struct Coord4D
    {
        public int X;
        public int Y;
        public int Z;
        public int W;

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}, W: {W}";
        }
    };

    struct Coord3D
    {
        public int X;
        public int Y;
        public int Z;
        
        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    };

    class Dimension4D
    {
        private bool[,,,] Plane;
        private int count;

        public Dimension4D(int dimension_size)
        {
            Plane = new bool[dimension_size, dimension_size, dimension_size, dimension_size];
        }
        public Dimension4D(bool[,,,] plane)
        {
            Plane = new bool[plane.GetLength(0), plane.GetLength(1), plane.GetLength(2), plane.GetLength(3)];
            Array.Copy(plane, Plane, plane.Length);
        }

        public Dimension4D(bool[,,,] input, int expand_amount)
        {
            Plane = new bool[
                input.GetLength(0) + 2 * expand_amount,
                input.GetLength(1) + 2 * expand_amount,
                input.GetLength(2) + 2 * expand_amount,
                input.GetLength(3) + 2 * expand_amount
                ];
            for (int x = 0; x < input.GetLength(0); ++x)
                for (int y = 0; y < input.GetLength(1); ++y)
                    for (int z = 0; z < input.GetLength(2); ++z)
                        for (int w = 0; w < input.GetLength(3); ++w)
                        {
                            SetValue(input[x, y, z, w], new Coord4D { X = x, Y = y, Z = z, W = w }, expand_amount);
                        }
        }

        public void SetValue(bool value, Coord4D coord, int expand_amount)
        {
            Plane[coord.X + expand_amount, coord.Y + expand_amount, coord.Z + expand_amount, coord.W + expand_amount] = value;
        }

        public int CountActiveFields()
        {
            var count = 0;
            foreach(var field in Plane)
            {
                if (field) count++;
            }
            return count;
        }

        public Dimension4D Process()
        {
            var result = new Dimension4D(Plane, 1);
            // process the expanded plane, 
            int x_length = Plane.GetLength(0) + 1;
            int y_length = Plane.GetLength(1) + 1;
            int z_length = Plane.GetLength(2) + 1;
            int w_length = Plane.GetLength(3) + 1;
            Parallel.For(-1, z_length, (z) =>
            {
                Parallel.For(-1, x_length, (x) =>
                {
                    Parallel.For(-1, y_length, (y) =>
                    {
                        Parallel.For(-1, w_length, (w) =>
                        {
                            var window_dimension = GetNeighbourhoodDimension(new Coord4D { X = x, Y = y, Z = z, W = w });
                            var state = window_dimension.Plane[1, 1, 1, 1];
                            var active_neighbours_count = window_dimension.CountActiveFields();
                            if ((!state && active_neighbours_count == 3) ||
                                (state && active_neighbours_count != 3 && active_neighbours_count != 4))
                            {
                                result.SetValue(!state, new Coord4D { X = x, Y = y, Z = z, W = w }, expand_amount: 1);
                            }
                        });
                    });
                });
            });

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        Dimension4D GetNeighbourhoodDimension(Coord4D center)
        {
            var window_dimension = new Dimension4D(3);
            int x_window_length = window_dimension.Plane.GetLength(0);
            int x_plane_length = Plane.GetLength(0);
            int y_window_length = window_dimension.Plane.GetLength(1);
            int y_plane_length = Plane.GetLength(1);
            int z_window_length = window_dimension.Plane.GetLength(2);
            int z_plane_length = Plane.GetLength(2);
            int w_window_length = window_dimension.Plane.GetLength(3);
            int w_plane_length = Plane.GetLength(3);
            for (int result_x = 0; result_x < x_window_length; ++result_x)
            {
                var plane_x = center.X - 1 + result_x;
                if (plane_x < 0 || plane_x >= x_plane_length)
                    continue;
                for (int result_y = 0; result_y < y_window_length; ++result_y)
                {
                    var plane_y = center.Y - 1 + result_y;
                    if (plane_y < 0 || plane_y >= y_plane_length)
                        continue;
                    for (int result_z = 0; result_z < z_window_length; ++result_z)
                    {
                        var plane_z = center.Z - 1 + result_z;
                        if (plane_z < 0 || plane_z >= z_plane_length)
                            continue;
                        for (int result_w = 0; result_w < w_window_length; ++result_w)
                        {
                            var plane_w = center.W - 1 + result_w;
                            if (plane_w < 0 || plane_w >= w_plane_length)
                                continue;

                            window_dimension.Plane[result_x, result_y, result_z, result_w] = Plane[plane_x, plane_y, plane_z, plane_w];
                        }
                    }
                }
            }
            return window_dimension;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var z_count = Plane.GetLength(2);
            var w_count = Plane.GetLength(3);
            for (int w = 0; w < w_count; ++w)
            {
                for (int z = 0; z < z_count; ++z)
                {
                    builder.AppendLine($"z={z - z_count / 2}w={w - w_count / 2}");
                    for (int x = 0; x < Plane.GetLength(0); ++x)
                    {
                        for (int y = 0; y < Plane.GetLength(1); ++y)
                        {
                            //builder.Append($"{x}{y}{z}");
                            builder.Append(Plane[x, y, z, w] ? '#' : '.');
                        }
                        builder.AppendLine();
                    }
                }
            }
            return builder.ToString();
        }
    }

    class Dimension3D
    {
        private bool[,,] Plane;

        public Dimension3D(int dimension_size)
        {
            Plane = new bool[dimension_size, dimension_size, dimension_size];
        }
        public Dimension3D(bool[,,] plane)
        {
            Plane = new bool[plane.GetLength(0), plane.GetLength(1), plane.GetLength(2)];
            Array.Copy(plane, Plane, plane.Length);
        }

        public Dimension3D(bool[,,] input, int expand_amount)
        {
            Plane = new bool[input.GetLength(0) + 2 * expand_amount, input.GetLength(1) + 2 * expand_amount, input.GetLength(2) + 2 * expand_amount];
            for(int x = 0; x < input.GetLength(0); ++x)
                for(int y = 0; y < input.GetLength(1); ++y)
                    for(int z = 0; z < input.GetLength(2); ++z)
                    {
                        SetValue(input[x, y, z], new Coord3D { X = x, Y = y, Z = z }, expand_amount);
                    }
        }

        public void SetValue(bool value, Coord3D coord, int expand_amount)
        {
            Plane[coord.X + expand_amount, coord.Y + expand_amount, coord.Z + expand_amount] = value;
        }

        public int CountActiveFields()
        {
            var count = 0;
            foreach(var field in Plane)
            {
                if (field) count++;
            }
            return count;
        }

        public Dimension3D Process()
        {
            var result = new Dimension3D(Plane, 1);
            // process the expanded plane, 
            Parallel.For(-1, Plane.GetLength(2) + 1, (z) =>
            {
                Parallel.For(-1, Plane.GetLength(0) + 1, (x) =>
                {
                    Parallel.For(-1, Plane.GetLength(1) + 1, (y) =>
                    {
                        var window_dimension = GetNeighbourhoodDimension(new Coord3D { X = x, Y = y, Z = z });
                        var state = window_dimension.Plane[1, 1, 1];
                        var active_neighbours_count = window_dimension.CountActiveFields();
                        if ((!state && active_neighbours_count == 3) ||
                            (state && active_neighbours_count != 3 && active_neighbours_count != 4))
                        {
                            result.SetValue(!state, new Coord3D { X = x, Y = y, Z = z }, expand_amount: 1);
                        }
                    });
                });
            });
            return result;
        }

        Dimension3D GetNeighbourhoodDimension(Coord3D center)
        {
            var neighbour_dimension = new Dimension3D(3);
            int x_window_length = neighbour_dimension.Plane.GetLength(0);
            int x_plane_length = Plane.GetLength(0);
            int y_window_length = neighbour_dimension.Plane.GetLength(1);
            int y_plane_length = Plane.GetLength(1);
            int z_window_length = neighbour_dimension.Plane.GetLength(2);
            int z_plane_length = Plane.GetLength(2);
            for (int result_x = 0; result_x < x_window_length; ++result_x)
            {
                var plane_x = center.X - 1 + result_x;
                if (plane_x < 0 || plane_x >= x_plane_length)
                    continue;
                for (int result_y = 0; result_y < y_window_length; ++result_y)
                {
                    var plane_y = center.Y - 1 + result_y;
                    if (plane_y < 0 || plane_y >= y_plane_length)
                        continue;
                    for (int result_z = 0; result_z < z_window_length; ++result_z)
                    {
                        var plane_z = center.Z - 1 + result_z;
                        if (plane_z < 0 || plane_z >= z_plane_length)
                            continue;

                        neighbour_dimension.Plane[result_x, result_y, result_z] = Plane[plane_x, plane_y, plane_z];
                    }
                }
            }
            return neighbour_dimension;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var z_count = Plane.GetLength(2);
            for (int z = 0; z < z_count; ++z)
            {
                builder.AppendLine($"z={z - z_count / 2}");
                for (int x = 0; x < Plane.GetLength(0); ++x)
                {
                    for (int y = 0; y < Plane.GetLength(1); ++y)
                    {
                        //builder.Append($"{x}{y}{z}");
                        builder.Append(Plane[x, y, z] ? '#' : '.');
                    }
                    builder.AppendLine();
                }
            }
            return builder.ToString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var input = File.ReadAllLines("Input/data.txt");
            var steps = 6;
            var input_plane_3d = new bool[input[0].Length, input.Length, 1];
            var input_plane_4d = new bool[input[0].Length, input.Length, 1, 1];
            for (int x = 0; x < input.Length; ++x)
            {
                var row = input[x];
                for (int y = 0; y < row.Length; ++y)
                {
                    var element = row[y];
                    input_plane_3d[x, y, 0] = element == '#';
                    input_plane_4d[x, y, 0, 0] = element == '#';
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Setup time: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();
            stopwatch.Start();
            PartA(input_plane_3d);
            stopwatch.Stop();
            Console.WriteLine($"time: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();
            stopwatch.Start();
            PartB(input_plane_4d);
            stopwatch.Stop();
            Console.WriteLine($"time: {stopwatch.ElapsedMilliseconds}ms");
            stopwatch.Reset();
}

        private static void PartA(bool[,,] input_plane_3d)
        {
            Console.WriteLine("Part A:");
            var dimension = new Dimension3D(input_plane_3d);
            for (int i = 0; i < 6; ++i)
            {
                //Console.WriteLine(dimension);
                dimension = dimension.Process();
                // pause for user input
                //Console.ReadKey(true);
            }
            var total_fields_after_activation = dimension.CountActiveFields();
            Console.WriteLine($"Total active fields after activation: {total_fields_after_activation}");
        }
        private static void PartB(bool[,,,] input_plane_4d)
        {
            Console.WriteLine("Part B:");
            var dimension = new Dimension4D(input_plane_4d);
            for (int i = 0; i < 6; ++i)
            {
                //Console.WriteLine(dimension);
                dimension = dimension.Process();
                // pause for user input
                //Console.ReadKey(true);
            }
            var total_fields_after_activation = dimension.CountActiveFields();
            Console.WriteLine($"Total active fields after activation: {total_fields_after_activation}");
        }

    }
}
