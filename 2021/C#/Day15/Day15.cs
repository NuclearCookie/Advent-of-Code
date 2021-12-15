using Library;
using Library.Datastructures;
using Library.Algorithms;

Array2D<int> grid = new Array2D<int>(IO.ReadInputAsStringArray().Select(x => x.Select(x => x - '0').ToArray()).ToArray());
List<Point2> outPath = new List<Point2>();
//grid.Transpose();
Algorithms.AStarPath(grid, new Point2(0, 0), new Point2(grid.ColumnLength - 1, grid.RowLength - 1), outPath);

PartA();

void PartA()
{
    var totalRisk = outPath.Skip(1).Select(coord => grid[coord]).Sum();
    Console.WriteLine($"Total risk: {totalRisk}");
    for(int row = 0; row < grid.RowLength; ++row)
    {
        for (int col = 0; col < grid.ColumnLength; ++col)
        {
            if (outPath.FindIndex(x => x.X == col && x.Y == row) >= 0)
                Console.Write($"\x1b[36m{grid[row, col]}\x1b[0m");
            else
                Console.Write(grid[row, col]);
        }
        Console.WriteLine();
    }
}
