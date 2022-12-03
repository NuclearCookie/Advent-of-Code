using Library;

var rucksackContents = IO.ReadInputAsStringArray(false);

var rucksacksDivided = rucksackContents.Select(rucksack => rucksack.Chunk(rucksack.Length / 2)); // split in 2
var misplacedItems = rucksacksDivided.Select(rucksack => rucksack.ElementAt(0).Intersect(rucksack.ElementAt(1)).First()); // find intersection of left and right. Assume 1 match
var sum1 = SumByPriority(misplacedItems); // sum
Console.WriteLine($"Part 1: Sum of all misplaced items based on priority: {sum1}");

var rucksacksPerGroup = rucksackContents.Chunk(3); // split by group (3)
var itemsPerRucksackPerGroup = rucksacksPerGroup.Select(group => group.Select(content => content.ToCharArray())); // split each rucksack in individual items
var badgesPerGroup = itemsPerRucksackPerGroup.Select(group => group.Aggregate(group.First(), (intersection, itemList) => intersection.Intersect(itemList).ToArray()).First()); // find the unique item across the 3 rucksacks
var sum2 = SumByPriority(badgesPerGroup); // sum
Console.WriteLine($"Part 2: Sum of all badges based on priority: {sum2}");

int SumByPriority(IEnumerable<char> items)
{
    var priorities = items.Select(item => 'a' <= item && item <= 'z' ? (int)item - 'a' + 1 : (int)item - 'A' + 27 ); // convert item to sum logic
    var sum = priorities.Aggregate(0, (sum, item) => sum + item); // sum
    return sum;
}
