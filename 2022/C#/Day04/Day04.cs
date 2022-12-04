using Library;

var sectionAssignmentPairs = IO.ReadInputAsStringArray(false).Select(
    line => line.Split(',').Select(
        range =>
        {
            var minMax = range.Split('-');
            return new Range(int.Parse(minMax[0]), int.Parse(minMax[1]));
        }));
var containedPairs = sectionAssignmentPairs.Aggregate(0, (redundantPairs, pair) => pair.ElementAt(0).Contains(pair.ElementAt(1)) || pair.ElementAt(1).Contains(pair.ElementAt(0)) ? redundantPairs + 1 : redundantPairs);
Console.WriteLine($"Part 1: Redundant Pairs: {containedPairs}");
var overlappingPairs = sectionAssignmentPairs.Aggregate(0, (redundantPairs, pair) => pair.ElementAt(0).Overlaps(pair.ElementAt(1)) || pair.ElementAt(1).Overlaps(pair.ElementAt(0)) ? redundantPairs + 1 : redundantPairs);
Console.WriteLine($"Part 2: Redundant Pairs: {overlappingPairs}");

class Range
{
    public int Min;
    public int Max;
    public Range(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public bool Contains(Range otherRange)
    {
        return Min <= otherRange.Min && Max >= otherRange.Max;
    }

    public bool Overlaps(Range otherRange)
    {
        return (Min >= otherRange.Min && Min <= otherRange.Max) || (Max >= otherRange.Min && Max <= otherRange.Max);
    }
};


