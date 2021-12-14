using Library;

var input = IO.ReadInputAsStringArray().ToArray();
var template = input[0];
// This could be cleaned up a bit. Ideally we'd avoid converting from string to PolymerPair and directly 
// map children to PolymerPair (like a linked list)
// Additionally, characterOccurences can be deduced from pairOccurenceCount but we need to add the initial values too. 
// Those changes make the loop a lot simpler but make the setup and processing harder. So let's skip it.
Dictionary<char, long> characterOccurence = new Dictionary<char, long>();
Dictionary<string, PolymerPair> pairMapping = new Dictionary<string, PolymerPair>();
Dictionary<PolymerPair, long> pairOccurenceCount = new Dictionary<PolymerPair, long>();

for(int i = 2; i < input.Length; ++i)
{
    var rule = input[i];
    var splitRule = rule.Split(" -> ");
    var startPair = splitRule[0];
    var insertChar = splitRule[1][0];
    var polymerPair = new PolymerPair
    {
        originalPair = startPair,
        leftPair = $"{startPair[0]}{insertChar}",
        rightPair = $"{insertChar}{startPair[1]}",
        insertedValue = insertChar
    };
    pairMapping[startPair] = polymerPair;

}

PartA();
PartB();

void PartA()
{
    Console.Write("PartA: ");
    BuildPolymer(10);
}

void PartB()
{
    Console.Write("PartB: ");
    BuildPolymer(40);
}

void BuildPolymer(int count)
{
    ResetCharacterOccurences();

    for(int i = 0; i < template.Length - 1; i++)
    {
        var currentPair = template.Substring(i, 2);
        var mappedPair = pairMapping[currentPair];
        var currentValue = pairOccurenceCount.GetValueOrDefault(mappedPair, 0);
        pairOccurenceCount[mappedPair] = ++currentValue;
    }

    for(int i = 0; i < count; ++i)
    {
        var workingCopy = new Dictionary<PolymerPair, long>();
        foreach(var pair in pairOccurenceCount)
        {
            var currentPolyPairCount = pair.Value;
            var currentPolyPair = pair.Key;
            var leftPair = pairMapping[currentPolyPair.leftPair];
            var rightPair = pairMapping[currentPolyPair.rightPair];
            var currentLeftCount = workingCopy.GetValueOrDefault(leftPair, 0);
            var currentRightCount = workingCopy.GetValueOrDefault(rightPair, 0);
            workingCopy[leftPair] = currentLeftCount + currentPolyPairCount;
            workingCopy[rightPair] = currentRightCount + currentPolyPairCount;
            characterOccurence[currentPolyPair.insertedValue] += currentPolyPairCount;
        }
        pairOccurenceCount = workingCopy;
    }
    var sorted = characterOccurence.Select(x => x.Value).OrderBy(x => x);
    var lowestOccurence = sorted.First();
    var heighestOccurence = sorted.Last();
    Console.WriteLine($"HeighestOccurence {heighestOccurence} - {lowestOccurence} = {heighestOccurence - lowestOccurence}");
}

void ResetCharacterOccurences()
{
    characterOccurence.Clear();
    pairOccurenceCount.Clear();
    foreach(var pair in pairMapping.Values)
    {
        characterOccurence[pair.insertedValue] = 0;
    }
    foreach(var elem in template)
    {
        characterOccurence[elem]++;
    }
}
class PolymerPair
{
    public string originalPair;
    public string leftPair;
    public string rightPair;
    public char insertedValue;

    public override string ToString()
    {
        return originalPair;
    }
}