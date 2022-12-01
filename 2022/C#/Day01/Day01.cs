using Library;

var calories = IO.ReadInputAsStringArray(false);
// unfortunately i'm too rusty to find an elegent LINQ algorithm for this. so let's go the oldschool way!
var caloriesPerElf = new List<int>();
caloriesPerElf.Add(0);
int index = 0;
foreach(var calory in calories)
{
    if (string.IsNullOrEmpty(calory))
    {
        index++;
        caloriesPerElf.Add(0);
        continue;
    }
    caloriesPerElf[index] += int.Parse(calory);
}
Console.WriteLine("Part one");
var maxCaloriesOnOneElf = caloriesPerElf.Max();
Console.WriteLine($"Max calories on 1 elf: {maxCaloriesOnOneElf}");

Console.WriteLine("Part two");
caloriesPerElf.Sort();
var maxCaloriesOnTop3Elfs = caloriesPerElf.TakeLast(3).Sum();
Console.WriteLine($"Max calories on top 3 elfs: {maxCaloriesOnTop3Elfs}");
