using Library;

var depths = IO.ReadInputAsIntArray().ToArray();
var totalIncreases = 0;
Console.WriteLine("Part one");
for(int i = 1; i < depths.Length; ++i)
{
    if (depths[i - 1] < depths[i])
    {
        totalIncreases++;
    }
}
Console.WriteLine($"Total increases: {totalIncreases}");
Console.WriteLine("Part two");
totalIncreases = 0;
for(int i = 3; i < depths.Length; ++i)
{
    var prevSum = depths[i - 3] + depths[i - 2] + depths[i - 1];
    var currSum = depths[i - 2] + depths[i - 1] + depths[i];
    if (prevSum < currSum)
    {
        totalIncreases++;
    }
}
Console.WriteLine($"Total increases: {totalIncreases}");
