using Library;
using Library.Datastructures;
using Library.Algorithms;

Array2D<int> grid = new Array2D<int>(IO.ReadInputAsStringArray().Select(x => x.Select(x => x - '0').ToArray()).ToArray());
List<Point2> outPath = new List<Point2>();

PartA();
PartB();

void PartA()
{
    Algorithms.AStarPath(grid, new Point2(0, 0), new Point2(grid.ColumnLength - 1, grid.RowLength - 1), outPath);
    var totalRisk = outPath.Skip(1).Select(coord => grid[coord]).Sum();
    Console.WriteLine($"Part A: Total risk: {totalRisk}");
}

void PartB()
{
    Array2D<int> extendedGrid = new Array2D<int>(grid.RowLength * 5, grid.ColumnLength * 5);
    for(int row = 0; row < grid.ColumnLength; ++row)
    {
        int[] extendedRow = new int[extendedGrid.ColumnLength];
        for (int col = 0; col < grid.RowLength; ++col)
        {
            var currentValue = grid[row, col];
            for (int i = 0; i < 5; ++i)
            {
                extendedRow[col + i * grid.ColumnLength] = currentValue;
                currentValue = (currentValue % 9) + 1;
            }
        }

        for (int i = 0; i < 5; ++i)
        {
            extendedGrid.SetRow(row + i * grid.RowLength, extendedRow);
            for(int col = 0; col < extendedRow.Length; ++col)
            {
                var currentValue = extendedRow[col];
                currentValue = (currentValue % 9) + 1;
                extendedRow[col] = currentValue;
            }
        }
    }
    outPath.Clear();
    Algorithms.AStarPath(extendedGrid, new Point2(0, 0), new Point2(extendedGrid.ColumnLength - 1, extendedGrid.RowLength - 1), outPath);
    var totalRisk = outPath.Skip(1).Select(coord => extendedGrid[coord]).Sum();
    //DrawSolution(extendedGrid);
    Console.WriteLine($"Part B: Total risk: {totalRisk}");

}

void DrawSolution(Array2D<int> grid)
{
    for (int row = 0; row < grid.RowLength; ++row)
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
