using Library;
using System.Diagnostics;
using System.Text.RegularExpressions;

var input = IO.ReadInputAsString(false);
var split = input.Split(Environment.NewLine + Environment.NewLine);
Debug.Assert(split.Length == 2, "Invalid input data");
var crateConfigString = split[0];
var instructionsString = split[1];

var crateStacks = ParseCrateConfig(crateConfigString);
FollowInstructions(instructionsString, crateStacks, false);
string topCrates = CountTopCrates(crateStacks);
Console.WriteLine($"Part1 {topCrates}");

crateStacks = ParseCrateConfig(crateConfigString);
FollowInstructions(instructionsString, crateStacks, true);
topCrates = CountTopCrates(crateStacks);
Console.WriteLine($"Part2 {topCrates}");

Stack<char>[] ParseCrateConfig(string crateConfigString)
{
    string[] crateConfigPerLine = crateConfigString.Split(Environment.NewLine);
    Array.Reverse(crateConfigPerLine);
    var totalNumberOfStacks = int.Parse(crateConfigPerLine[0].Trim()[^1].ToString());
    Stack<char>[] stacks = new Stack<char>[totalNumberOfStacks];
    for(int i = 0; i < stacks.Length; ++i)
    {
        stacks[i] = new Stack<char>();
    }
    var lineLength = crateConfigPerLine[0].Length;

    for(int i = 1; i < crateConfigPerLine.Length; ++i)
    {
        var cratesOnLine = crateConfigPerLine[i];
        var index = 0;
        for(int j = 1; j < lineLength; j += 4)
        {
            var crate = cratesOnLine[j];
            if (crate != ' ')
                stacks[index].Push(cratesOnLine[j]);
            index++;
        }
    }
    return stacks;
}

void FollowInstructions(string instructionsString, Stack<char>[] crateStacks, bool isNewModel)
{
    Regex regex = new Regex("move ([0-9]+) from ([0-9]+) to ([0-9]+)");
    var allMoves = regex.Matches(instructionsString);
    List<char> poppedElements = new List<char>();
    foreach(Match move in allMoves)
    {
        var groups = move.Groups;
        int count = int.Parse(groups[1].ValueSpan);
        int from = int.Parse(groups[2].ValueSpan) - 1;
        int to = int.Parse(groups[3].ValueSpan) - 1;

        if (isNewModel == false)
        {
            for(int i = 0; i < count; ++i)
            {
                crateStacks[to].Push(crateStacks[from].Pop());
            }
        }
        else
        {
            poppedElements.Clear();
            for(int i = 0; i < count; ++i)
            {
                poppedElements.Add(crateStacks[from].Pop());
            }
            for (int i = poppedElements.Count - 1; i >= 0; i--)
            {
                crateStacks[to].Push(poppedElements[i]);
            }
        }
    }
}

string CountTopCrates(Stack<char>[] crateStacks)
{
    string topCrates = "";
    foreach(var stack in crateStacks)
    {
        topCrates += stack.Peek();
    }
    return topCrates;
}
