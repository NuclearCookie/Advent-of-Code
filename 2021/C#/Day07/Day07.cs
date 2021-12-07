using Library;

// with some sorting and counting.
//var horizontalCrabPositions = IO.ReadInputAsStringArray().First().Split(",").Select(int.Parse).GroupBy(x => x).OrderByDescending(x => x.Count()).Select(x => (pos: x.Key, count: x.Count())).ToList();
var horizontalCrabPositions = IO.ReadInputAsStringArray().First().Split(",").Select(int.Parse).ToList();
horizontalCrabPositions.Sort();
var min = horizontalCrabPositions[0];
var max = horizontalCrabPositions[^1];
PartA();
PartB();
void PartA()
{
    var lowest = int.MaxValue;
    for (int i = min; i < max; i++)
    {
        var currentCost = 0;
        foreach (var position in horizontalCrabPositions)
        {
            currentCost += Math.Abs(i - position);
        }
        if (currentCost < lowest)
        {
            lowest = currentCost;
        }
    }
    Console.WriteLine($"[Part A] Lowest cost: {lowest}");
}

void PartB()
{
    var lowest = int.MaxValue;
    for (int i = min; i < max; i++)
    {
        var currentCost = 0;
        foreach (var position in horizontalCrabPositions)
        {
            currentCost += ComputeNonLinearCost(Math.Abs(i - position));
        }
        if (currentCost < lowest)
        {
            lowest = currentCost;
        }
    }
    Console.WriteLine($"[Part B] Lowest cost: {lowest}");
}

int ComputeNonLinearCost(int cost)
{
    // n + (n+1) + (n+2) + ..
    return cost * (cost + 1) / 2; 
}
