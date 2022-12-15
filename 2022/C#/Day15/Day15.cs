using Library;
using Library.Datastructures;
using System.Text.RegularExpressions;

var useTestFile = false;

Regex regex = new Regex(@"Sensor at x=([-\d]+), y=([-\d]+): closest beacon is at x=([-\d]+), y=([-\d]+)");
var inputData = IO.ReadInputAsStringArray(useTestFile).Select(line =>
{
    var match = regex.Match(line);
    return (new Point2(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)), new Point2(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value)));
}).ToArray();
var lineToSolve = useTestFile ? 10 : 2000000;
var maxCoordinateForDistressBeacon = useTestFile ? 20 : 4000000;

// the idea is to check for each sensor, how many points on line X there are,
// by finding the left most and right most point on that line, that match the manhattandistance of sensor to closest beacon.
HashSet<Point2> uniqueBeacons = new HashSet<Point2>();
foreach (var sensorData in inputData)
{
    var beacon = sensorData.Item2;
    uniqueBeacons.Add(beacon);
}
 
var segmentsForLine = SolveForLine(lineToSolve);

int totalOccupiedLines = 0;
foreach(var segment in segmentsForLine)
{
    totalOccupiedLines += segment.CountHorizontalRange();
}
foreach (var beacon in uniqueBeacons)
{
    if (beacon.Y == lineToSolve)
        totalOccupiedLines--;
}
Console.WriteLine($"Part A: Amount of points on line that are not occupied by beacon: {totalOccupiedLines}");
var tuningFrequency = GetTuningFrequency();
Console.WriteLine($"Part B: Tuning Frequency: {tuningFrequency}");

long GetTuningFrequency()
{
    for (int i = 0; i <= maxCoordinateForDistressBeacon; ++i)
    {
        var lineRange = SolveForLine(i);
        // let's just assume there's only 1 line with 2 ranges, and that's our answer
        if (lineRange.Count > 1)
        {
            var coord = new Point2(lineRange[0].right.X + 1, i);
            return (long)coord.X * 4000000 + coord.Y;
        }
        // let's just assume this case is not in AoC. We can handle it but no need for this challenge
        //else if (lineRange[0].left.X > 0 || lineRange[0].right.X < maxCoordinateForDistressBeacon)
        //{
        //    return ;
        //}
    }
    return 0;
}

List<SegmentOnLine> SolveForLine(int lineToSolve)
{
    List<SegmentOnLine> segmentsForLine = new List<SegmentOnLine>(16);
    var sorter = new SegmentSorter();
    foreach (var sensorData in inputData)
    {
        var sensor = sensorData.Item1;
        var beacon = sensorData.Item2;
        var manhattanDistance = sensor.GetManhattanDistance(beacon);
        var distanceFromSensorToLine = Math.Abs(lineToSolve - sensor.Y);
        if (manhattanDistance <= distanceFromSensorToLine) // sensor doesn't reach this line.
            continue;
        var maxSidewaysMovement = /*Math.Abs*/(manhattanDistance - distanceFromSensorToLine);
        segmentsForLine.Add(new SegmentOnLine
        {
            left = new Point2(sensor.X - maxSidewaysMovement, lineToSolve),
            right = new Point2(sensor.X + maxSidewaysMovement, lineToSolve)
        });
    }
    segmentsForLine.Sort(sorter);
    int amountOfSegments = -1;

    do
    {
        //Console.WriteLine("Start merging segments.");
        amountOfSegments = segmentsForLine.Count;
        for (int i = segmentsForLine.Count - 1; i >= 1; i--)
        {
            var higher = segmentsForLine[i];
            var lower = segmentsForLine[i - 1];
            //Console.WriteLine($" - Evaluating {lower} vs {higher}");
            if (lower.Overlaps(higher))
            {
                var merged = SegmentOnLine.Merge(lower, higher);
                segmentsForLine[i - 1] = merged;
                segmentsForLine.RemoveAt(i);
                //Console.WriteLine($"   - Segments Overlap. Merging into 1: {merged}");
            }
            //else
            //{
            //    Console.WriteLine("   - No overlap!");
            //}
        }
    }
    while (segmentsForLine.Count != amountOfSegments);

    return segmentsForLine;
}

class SegmentSorter : IComparer<SegmentOnLine>
{
    public int Compare(SegmentOnLine x, SegmentOnLine y)
    {
        return x.left.X - y.left.X;
    }
}

struct SegmentOnLine
{
    public Point2 left;
    public Point2 right;

    public bool Contains(SegmentOnLine other)
    {
        return left.X <= other.left.X && right.X >= other.right.X;
    }

    public bool Overlaps(SegmentOnLine other)
    {
        return left.X <= other.left.X && other.left.X <= right.X || right.X >= other.right.X && other.right.X >= left.X;
    }

    public static SegmentOnLine Merge(SegmentOnLine first, SegmentOnLine second)
    {
        return new SegmentOnLine 
        { 
            left = new Point2(Math.Min(first.left.X, second.left.X), first.left.Y), 
            right = new Point2(Math.Max(first.right.X, second.right.X), first.left.Y) 
        };
    }

    public int CountHorizontalRange()
    {
        return right.X - left.X + 1; // range is inclusive
    }

    public override string ToString()
    {
        return $"[{left.X} -> {right.X}]";
    }
}