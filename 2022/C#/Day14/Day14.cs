using Library;
using Library.Datastructures;

var sandEntryPoint = new Point2(500, 0);
var rockLines = IO.ReadInputAsStringArray(false).Select(line => line.Split(" -> ").Select(coord =>
{
    var split = coord.Split(',');
    return new Point2(int.Parse(split[0]), int.Parse(split[1]));
}).ToArray());

void GenerateGrid(out int totalX, out int totalY, out int largestY, out Array2D<char> grid)
{
    var allPoints = rockLines.SelectMany(line => line).Append(sandEntryPoint);
    var smallestX = allPoints.OrderBy(point => point.X).First().X;
    var largestX = allPoints.OrderByDescending(point => point.X).First().X;
    var smallestY = allPoints.OrderBy(point => point.Y).First().Y;
    largestY = allPoints.OrderByDescending(point => point.Y).First().Y;

    totalX = (largestX - smallestX) + 1;
    totalY = (largestY - smallestY) + 1;
    var pointOffset = new Point2(smallestX, smallestY);
    var offsettedLines = rockLines.Select(line => line.Select(point => point - pointOffset).ToArray());
    sandEntryPoint = new Point2(500, 0);
    sandEntryPoint -= pointOffset;
    grid = new Array2D<char>(totalY, totalX, Enumerable.Repeat('.', totalX * totalY).ToArray());
    grid[sandEntryPoint] = '+';

    foreach(var line in offsettedLines)
    {
        for(int i = 1; i < line.Length; ++i)
        {
            var from = line[i - 1];
            var to = line[i];

            if (from.X > to.X || from.Y > to.Y)
                (from, to) = (to, from);

            for( int x = from.X; x <= to.X; ++x)
            {
                grid[new Point2(x, from.Y)] = '#';
            }

            for( int y = from.Y; y <= to.Y; ++y)
            {
                grid[new Point2(from.X, y)] = '#';
            }
        }
    }
}

GenerateGrid(out var totalX, out var totalY, out var largestY, out var grid);

// Part A
Start(true);

// Part B
rockLines = rockLines.Append(new Point2[] { new Point2(200, largestY + 2), new Point2(1000, largestY + 2) });
GenerateGrid(out totalX, out totalY, out largestY, out grid);
Start(false); 

void Start(bool drawGrid)
{
    bool interactiveDraw = false;
    var totalAmountOfSandDrops = 1;
    var currentSandPosition = sandEntryPoint;
    var down = new Point2(0, 1);
    var diagonalLeft = new Point2(-1, 1);
    var diagonalRight = new Point2(1, 1);

    var originalCursorRow = Console.CursorTop;
    var originalCursorCol = Console.CursorLeft;

    //Console.SetBufferSize(Math.Max(grid.ColumnLength + 10, Console.BufferWidth), Math.Max(grid.RowLength + 10, Console.BufferHeight));
    while (true)
    {
        if (interactiveDraw)
            Console.SetCursorPosition(originalCursorCol, originalCursorRow);
        grid[currentSandPosition] = '.'; // move the sand down, so clear it here.
        try
        {
            if (grid[currentSandPosition + down] == '.')
                currentSandPosition += down;
            else if (grid[currentSandPosition + diagonalLeft] == '.')
                currentSandPosition += diagonalLeft;
            else if (grid[currentSandPosition + diagonalRight] == '.')
                currentSandPosition += diagonalRight;
            else
            {
                if (currentSandPosition == sandEntryPoint) // we reached the top!
                {
                    if (drawGrid)
                        Console.WriteLine(grid);
                    Console.WriteLine($"Reached the top! (amountOfPackes dropped: {totalAmountOfSandDrops})");
                    break;
                }
                totalAmountOfSandDrops++;
                grid[currentSandPosition] = 'o';
                currentSandPosition = sandEntryPoint;
            }
        } 
        catch(Exception e)
        {
            totalAmountOfSandDrops--; // the current one couldn't actually be placed, so don't count it.
            if (drawGrid)
                Console.WriteLine(grid);
            //Console.WriteLine(e);
            Console.WriteLine($"We're done! Amount of sand packets dropped: {totalAmountOfSandDrops}");
            break;
        }


        grid[currentSandPosition] = 'o';
        if (interactiveDraw)
        {
            Console.WriteLine(grid);
            Thread.Sleep(10);
        }
    }
}
