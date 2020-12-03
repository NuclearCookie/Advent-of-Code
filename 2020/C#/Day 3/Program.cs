using System;
using System.IO;
using System.Linq;

namespace Day_3
{
    class Program
    {
        private struct Point2
        {
            public int X;
            public int Y;
        }

        private static Point2 coordinate = new Point2 { X = 0, Y = 0 };
        private static int segmentWidth = 0;
        static void Main(string[] args)
        {
            var data = File.ReadAllLines("Input/data.txt");
            segmentWidth = data[0].Length;
            PartA(data);
            PartB(data);
        }

        private static void PartA(string[] data)
        {
            void AdvanceCoordinates()
            {
                coordinate.Y += 1;
                coordinate.X += 3;
                coordinate.X %= segmentWidth;
            }     

            Console.WriteLine("PART A:");
            var total_trees = data.Aggregate(0, (count, row) =>
            {
                count = row[coordinate.X] == '#' ? ++count : count;
                AdvanceCoordinates();
                return count;
            });
            Console.WriteLine($"Total amount of trees encountered: {total_trees}");
        }
        private static void PartB(string[] data)
        {
            void AdvanceCoordinates(Point2 advance)
            {
                coordinate.Y += advance.Y;
                coordinate.X += advance.X;
                coordinate.X %= segmentWidth;
            }  

            long ProcessSlope(Point2 advance)
            {
                coordinate = new Point2 { X = 0, Y = 0 };
                return data
                    .Where((x, index) => index % advance.Y == 0)
                    .Aggregate(0, (count, row) =>
                    {
                        count = row[coordinate.X] == '#' ? ++count : count;
                        AdvanceCoordinates(advance);
                        return count;
                    });
            }

            Console.WriteLine("PART B:");

            var total_trees = ProcessSlope(new Point2 { X = 1, Y = 1 });
            total_trees *= ProcessSlope(new Point2 { X = 3, Y = 1 });
            total_trees *= ProcessSlope(new Point2 { X = 5, Y = 1 });
            total_trees *= ProcessSlope(new Point2 { X = 7, Y = 1 });
            total_trees *= ProcessSlope(new Point2 { X = 1, Y = 2 });
            Console.WriteLine($"Total amount of trees encountered: {total_trees}");
        }
    }
}
