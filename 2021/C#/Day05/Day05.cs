using Library;
using Library.Datastructures;
using Library.Extensions;

var input = IO.ReadInputAsStringArray();
var lines = ParseInput();
PartA();
PartB();

IEnumerable<Point2[]> ParseInput()
{
    return input.Select(x => x.Split(" -> ").Select(line =>
    {
        var pair = line.Split(",");
        var x = int.Parse(pair[0]);
        var y = int.Parse(pair[1]);
        return new Point2 { X = x, Y = y };
    }).ToArray());
}

void PartA()
{
    List<Point2> occurences = new List<Point2>();
    PlotStraightLines(occurences);
    var twoOrMore = occurences.GroupBy(x => x).Where(x => x.Count() > 1).Count();
    Console.WriteLine($"[PartA]: 2 or more occurences: {twoOrMore}");
}

void PlotStraightLines(List<Point2> occurences)
{
    var straightLines = lines.Where(line => line[0].X == line[1].X || line[0].Y == line[1].Y).ToArray();
    Console.WriteLine($"Amount of straight lines: {straightLines.Length}");

    // generate datapoints for each coord of each line. Flatten into 1 array so we can check for duplicates
    foreach (var line in straightLines)
    {
        foreach (var val in (line[0].X)..(line[1].X))
        {
            occurences.Add(new Point2 { X = val, Y = line[0].Y });
        }
        foreach (var val in (line[0].Y)..(line[1].Y))
        {
            occurences.Add(new Point2 { X = line[0].X, Y = val });
        }
        occurences.Add(line[1]);
    }
}

void PartB()
{
    List<Point2> occurences = new List<Point2>();
    PlotStraightLines(occurences);
    PlotDiagonalLines(occurences);
    var twoOrMore = occurences.GroupBy(x => x).Where(x => x.Count() > 1).Count();
    Console.WriteLine($"[PartB] 2 or more occurences: {twoOrMore}");
}


void PlotDiagonalLines(List<Point2> occurences)
{
    var diagonalLines = lines.Where(line => Math.Abs(line[0].X - line[1].X) == Math.Abs(line[0].Y - line[1].Y)).ToArray();
    Console.WriteLine($"Amount of diagonal lines: {diagonalLines.Length}");
    // generate datapoints for each coord of each line. Flatten into 1 array so we can check for duplicates
    foreach (var line in diagonalLines)
    {
        var currentLine = new List<Point2>();
        foreach (var val in (line[0].X)..(line[1].X))
        {
            currentLine.Add(new Point2 { X = val, Y = int.MinValue});
        }
        int index = 0;
        foreach (var val in (line[0].Y)..(line[1].Y))
        {
            currentLine[index] = new Point2 { X = currentLine[index].X, Y = val };
            index++;
        }
        currentLine.Add(line[1]);
        occurences.AddRange(currentLine);
    }
}
