using Library;

var fish = IO.ReadInputAsStringArray().First().Split(',').Select(int.Parse).ToList();

PartA();

void PartA()
{
    for(int i = 0; i < 80; ++i)
    {
        ProcessDay();
    }
    Console.WriteLine($"After 80 days, {fish.Count} lanternfishes appeared.");
}
void ProcessDay()
{
    int fishesToAppend = 0;
    for (int i = 0; i < fish.Count; i++)
    {
        int fishy = fish[i];
        fishy--;
        if (fishy < 0)
        {
            fishesToAppend++;
            fishy = 6;
        }
        fish[i] = fishy;
    }
    fish.AddRange(Enumerable.Repeat(8, fishesToAppend));
}

