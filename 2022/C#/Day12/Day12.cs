using Library;
using Library.Datastructures;
using Library.Algorithms;
using Library.Extensions;
using System.Text;
using Pastel;
using System.Drawing;

var input = IO.ReadInputAsStringArray(false);
// map input string to height values.
var heightmapInput = input.Select(line => line.Select(x =>
{
    if (x == 'S')
    {
        return 'a' - 'a';
    }
    else if (x == 'E')
    {
        return 'z' - 'a';
    }
    return x - 'a';
}).ToArray()).ToArray();

Array2D<int> heightMap = new Array2D<int>(heightmapInput);
// we need a dummymap with 1 values for A* computations. We won't use the height as a weighttable but as a heuristic!
Array2D<int> dummyMap = new Array2D<int>(heightmapInput);
for(int i = 0; i < dummyMap.Length; ++i)
{
    dummyMap[i] = 1;
}

// get the index of the start and end point
var inputFlat = string.Join("", input);
var startPoint = heightMap.IndexToRowColumn(inputFlat.IndexOf('S'));
var endPoint = heightMap.IndexToRowColumn(inputFlat.IndexOf('E'));

List<Point2> ComputeTrail(Point2 startPoint)
{
    // perform A* with a maximum path cost and a custom heuristic.
    // climbs of more than 1 unit are invalid, so should be > maximum path cost.
    List<Point2> outPath = new List<Point2>();
    int invalidPathCost = 100000;
    Algorithms.AStarPath(dummyMap, startPoint, endPoint, outPath, (parent, current, end) =>
    {
        var manhattanDistance = current.GetManhattanDistance(end);
        var parentHeight = heightMap[parent];
        var currentHeight = heightMap[current];
        // unwalkable because the path is too steep
        if (currentHeight - parentHeight > 1)
            return invalidPathCost + manhattanDistance;
        return manhattanDistance;
    }, invalidPathCost);

    return outPath;
}

// Part 1
var outPath = ComputeTrail(startPoint);
DebugDrawHeightMap(outPath, startPoint);
Console.WriteLine($"Path length: {outPath.Count - 1}");

// Part 2: finding shortest path of all paths at height 0.
List<(Point2, List<Point2>)> _pathLengthToStartPointMapping = new();
for(int i = 0; i < heightMap.Length; ++i)
{
    if (heightMap[i] != 0)
        continue;
    var potentialShortPath = ComputeTrail(heightMap.IndexToRowColumn(i));
    if (potentialShortPath.Count <= 0) // no path found.
        continue;
    _pathLengthToStartPointMapping.Add(new (startPoint, potentialShortPath)); // we'll assume we'll only have 1 shortest path so just overwrite the dictionary.
}
_pathLengthToStartPointMapping.Sort((a, b) => a.Item2.Count - b.Item2.Count);
Console.WriteLine($"Shortest path for making hiking trail starts at {_pathLengthToStartPointMapping[0].Item1} and is {_pathLengthToStartPointMapping[0].Item2.Count - 1} units long!");
DebugDrawHeightMap(_pathLengthToStartPointMapping[0].Item2, _pathLengthToStartPointMapping[0].Item1);

void DebugDrawHeightMap(List<Point2> path, Point2 startPoint)
{
    Array2D<char> debugDraw = new Array2D<char>(heightMap.RowLength, heightMap.ColumnLength, Enumerable.Repeat('.', heightMap.Length).ToArray());
    Array2D<string> heightMapOverlay = new Array2D<string>(heightMap.RowLength, heightMap.ColumnLength);
    for(int i = 0; i < heightMap.Length; ++i)
    {
        heightMapOverlay[i] = $"{heightMap[i],2}";
    }
    for(int i = 0; i < path.Count - 1; ++i)
    {
        var currentPos = path[i];
        var nextPos = path[i + 1];
        var diff = nextPos - currentPos;
        if (diff == new Point2(1, 0))
            debugDraw[currentPos] = '>';
        else if (diff == new Point2(-1, 0))
            debugDraw[currentPos] = '<';
        else if (diff == new Point2(0, 1))
            debugDraw[currentPos] = 'v';
        else if (diff == new Point2(0, -1))
            debugDraw[currentPos] = '^';
        heightMapOverlay[currentPos] = heightMapOverlay[currentPos].Pastel(Color.White);
    }
    debugDraw[endPoint] = 'E';
    heightMapOverlay[endPoint] = " E";
    debugDraw[startPoint] = 'S';
    heightMapOverlay[startPoint] = " S";


    StringBuilder heightMapBuilder = new StringBuilder();
    StringBuilder debugDrawBuilder = new StringBuilder();
    for(int row = 0; row < debugDraw.RowLength; row++)
    {
        for(int col = 0; col < debugDraw.ColumnLength; col++)
        {
            var heightValue = heightMap[row, col];
            var heightPercentage = heightValue / 26.0f;
            Extensions.ColorToHSV(Color.LightSeaGreen, out var greenHue, out var greenSaturation, out var greenValue);
            Extensions.ColorToHSV(Color.DarkRed, out var redHue, out var redSaturation, out var redValue);
            var colorHue = Library.Algorithms.Math.Lerp(greenHue, redHue, heightPercentage);
            var colorSaturation = Library.Algorithms.Math.Lerp(greenSaturation, redSaturation, heightPercentage);
            var colorValue = Library.Algorithms.Math.Lerp(greenValue, redValue, heightPercentage);
            var colorToDraw = Extensions.ColorFromHSV(colorHue, colorSaturation, colorValue);
            heightMapBuilder.Append(heightMapOverlay[row, col].PastelBg(colorToDraw).Pastel(Color.Black));
            debugDrawBuilder.Append(debugDraw[row, col].ToString().PastelBg(colorToDraw).Pastel(Color.Black));
        }
        heightMapBuilder.AppendLine();
        debugDrawBuilder.AppendLine();
    }
    Console.WriteLine(heightMapBuilder.ToString());
    Console.WriteLine(debugDrawBuilder.ToString());
}
