using Library;

var fish = IO.ReadInputAsStringArray().First().Split(',').Select(long.Parse);
long fish0 = fish.Where(x => x == 0).Count();
long fish1 = fish.Where(x => x == 1).Count();
long fish2 = fish.Where(x => x == 2).Count();
long fish3 = fish.Where(x => x == 3).Count();
long fish4 = fish.Where(x => x == 4).Count();
long fish5 = fish.Where(x => x == 5).Count();
long fish6 = fish.Where(x => x == 6).Count();
long fish7 = fish.Where(x => x == 7).Count();
long fish8 = fish.Where(x => x == 8).Count();

var watch = System.Diagnostics.Stopwatch.StartNew();
PartA();
PartB();
watch.Stop();
Console.WriteLine($"Time: {watch.ElapsedMilliseconds}ms");

void PartA()
{
    for(int i = 0; i < 80; ++i)
    {
        ProcessDay();
    }
    Console.WriteLine($"After 80 days, {fish0 + fish1 + fish2 + fish3 + fish4 + fish5 + fish6 + fish7 + fish8} lanternfishes appeared.");
}

void PartB()
{
    for(int i = 80; i < 256; ++i)
    {
        ProcessDay();
    }
    Console.WriteLine($"After 256 days, {fish0 + fish1 + fish2 + fish3 + fish4 + fish5 + fish6 + fish7 + fish8} lanternfishes appeared.");
}

void ProcessDay()
{
    var temp = fish0; // this will go to fish9
    fish0 = fish1;
    fish1 = fish2;
    fish2 = fish3;
    fish3 = fish4;
    fish4 = fish5;
    fish5 = fish6;
    fish6 = fish7 + temp;
    fish7 = fish8;
    fish8 = temp;
}

