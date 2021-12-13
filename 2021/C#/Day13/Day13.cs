using Library;
using Library.Datastructures;

var lines = IO.ReadInputAsStringArray().ToArray();
List<Point2> pointList = new List<Point2>();
List<Point2> foldList = new List<Point2>();

ParseInput();
PartAB();

void ParseInput()
{
    int i = 0;
    for (; i < lines.Length; i++)
    {
        string line = lines[i];
        if (string.IsNullOrWhiteSpace(line))
            break;
        var split = line.Split(',');
        pointList.Add(new Point2 { X = int.Parse(split[0]), Y = int.Parse(split[1]) });
    }
    i++;
    for (; i < lines.Length; i++)
    {
        Point2 point = new Point2();
        string line = lines[i];
        var foldLine = line.Substring(13);
        if (line.StartsWith("fold along y="))
        {
            point.Y = int.Parse(foldLine);
        }
        else
        {
            point.X = int.Parse(foldLine);
        }
        foldList.Add(point);
    }
}

void PartAB()
{
    for (int foldIndex = 0; foldIndex < foldList.Count; foldIndex++)
    {
        Point2 fold = foldList[foldIndex];
        if (fold.X > 0)
        {
            for(int pointIndex = 0; pointIndex < pointList.Count; pointIndex++)
            {
                var point = pointList[pointIndex];
                if (point.X > fold.X)
                {
                    point.X = fold.X - (point.X - fold.X);
                }
                pointList[pointIndex] = point;
            }
        }
        else
        {
            for(int pointIndex = 0; pointIndex < pointList.Count; pointIndex++)
            {
                var point = pointList[pointIndex];
                if (point.Y > fold.Y)
                {
                    point.Y = fold.Y - (point.Y - fold.Y);
                }
                pointList[pointIndex] = point;
            }
        }
        pointList = pointList.Distinct().ToList();
        var amountOfPoints = pointList.Count();
        Console.WriteLine($"Amount of remaining points after fold {foldIndex}: {amountOfPoints}");
    }
    Plot();
}

void Plot()
{
    var maxX = pointList.MaxBy(point => point.X).X + 1;
    var maxY = pointList.MaxBy(point => point.Y).Y + 1;
    var initialData = Enumerable.Repeat(' ', maxX * maxY).ToArray();
    Array2D<char> grid = new Array2D<char>(maxY, maxX, initialData);
    foreach(var point in pointList)
    {
        grid[point.Y, point.X] = '#';
    }
    Console.WriteLine(grid);
}
