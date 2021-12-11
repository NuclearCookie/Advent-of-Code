using Library;
using Library.Datastructures;

var octoInput = IO.ReadInputAsStringArray().Select(x => x.Select(x => x - '0').ToArray()).ToArray();
Array2D<int> octopusGrid = new Array2D<int>(octoInput);
int flashCount = 0;

PartA();
PartB();

void PartA()
{
    int firstSyncStepFlash = 0;
    for(int i = 0; i < 100; ++i)
    {
        var wasSynced = Step();
        if (wasSynced && firstSyncStepFlash == 0)
        {
            firstSyncStepFlash = i;
        }
    }
    Console.WriteLine($"Amount of flashes after 100 steps: {flashCount}. First synced flash occured at step: {firstSyncStepFlash}");
}

void PartB()
{
    int step = 101;
    while(Step() == false)
    {
        step++;
    }
    Console.WriteLine($"First synced flash occured at step: {step}");
}

bool Step()
{
    for (int i = 0; i < octopusGrid.Length; ++i)
    {
        IncreaseOctopusEnergy(i);
    }
    var flashedCount = 0;
    for (int i = 0; i < octopusGrid.Length; ++i)
    {
        if (octopusGrid[i] >= 10)
        {
            flashedCount++;
            octopusGrid[i] = 0;
        }
    }
    return flashedCount == octopusGrid.Length;
}

void IncreaseOctopusEnergy(int octoIndex)
{
    var currentValue = octopusGrid[octoIndex];
    if (currentValue >= 10)
    {
        return;
    }
    currentValue++;
    octopusGrid[octoIndex] = currentValue;
    if (currentValue == 10)
    {
        flashCount++;
        int[] neighbours = new int[8];
        var neighbourCount = octopusGrid.GetNeighbourIndices(octoIndex, neighbours);
        for(int i = 0; i < neighbourCount; ++i)
        {
            IncreaseOctopusEnergy(neighbours[i]);
        }
    }
}
