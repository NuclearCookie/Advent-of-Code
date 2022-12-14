using Library;
using Library.Datastructures;

var sandEntryPoint = new Point2(500, 0);
var rockLines = IO.ReadInputAsStringArray(false).Select(line => line.Split(" -> ").Select(coord =>
{
    var split = coord.Split(',');
    return new Point2(int.Parse(split[0]), int.Parse(split[1]));
}).ToArray());
var allPoints = rockLines.SelectMany(line => line).Append(sandEntryPoint);
var smallestX = allPoints.OrderBy(point => point.X).First().X;
var largestX = allPoints.OrderByDescending(point => point.X).First().X;
var smallestY = allPoints.OrderBy(point => point.Y).First().Y;
var largestY = allPoints.OrderByDescending(point => point.Y).First().Y;

var totalX = (largestX - smallestX) + 1;
var totalY = (largestY - smallestY) + 1;
var pointOffset = new Point2(smallestX, smallestY);

var grid = new Array2D<char>(totalY, totalX, Enumerable.Repeat('.', totalX * totalY).ToArray());
grid[sandEntryPoint - pointOffset] = '+';
foreach(var line in rockLines)
{
    for(int i = 1; i < line.Length; ++i)
    {
        var from = line[i - 1];
        var to = line[i];

        if (from.X > to.X || from.Y > to.Y)
            (from, to) = (to, from);

        for( int x = from.X; x <= to.X; ++x)
        {
            grid[new Point2(x, from.Y) - pointOffset] = '#';
        }

        for( int y = from.Y; y <= to.Y; ++y)
        {
            grid[new Point2(from.X, y) - pointOffset] = '#';
        }
    }
}

Console.WriteLine(grid);
