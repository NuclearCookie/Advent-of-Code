using Library;

var pairs = IO.ReadInputAsStringArray(false).Chunk(3).Select(chunk => chunk.Take(2).ToArray()).ToArray();
int sumOfValidPairs = 0;
bool debugLog = false;

for (int i = 0; i < pairs.Length; i++)
{
    if (debugLog)
        Console.WriteLine($"=== Pair {i + 1} ===");
    string[] pair = pairs[i];
    var result = ProcessPairRecursive(pair[0], pair[1], 0);
    if (result)
        sumOfValidPairs += (i + 1);
    if (debugLog)
        Console.WriteLine("\n");
    
}
Console.WriteLine($"Sum of valid pairs: {sumOfValidPairs}");
// Now sort it! This is cool stuff!
// collapse all pairs into a single array
var flatList = pairs.SelectMany(p => p).ToList();
flatList.Add("[[2]]");
flatList.Add("[[6]]");
flatList.Sort((a, b) => ProcessPairRecursive(a, b, 0) ? -1 : 1);
Console.WriteLine($"Ordered packets: ");
foreach(var sortedPacket in flatList)
{
    Console.WriteLine(sortedPacket);
}

var decoderKey = (flatList.IndexOf("[[2]]") + 1) * (flatList.IndexOf("[[6]]") + 1);
Console.WriteLine($"Found the decoder key: {decoderKey}");

bool ProcessPairRecursive(ReadOnlySpan<char> left, ReadOnlySpan<char> right, int amountOfListDepth)
{
    var debugLogDepth = string.Concat(Enumerable.Repeat("  ", amountOfListDepth));
    // skip commas
    while (left[0] == ',')
        left = left.Slice(1);
    while (right[0] == ',')
        right = right.Slice(1);

    var leftIsListEnd = left[0] == ']';
    var rightIsListEnd = right[0] == ']';
    if (leftIsListEnd && rightIsListEnd == false)
    {
        if (debugLog)
            Console.WriteLine($"{debugLogDepth} - Left side ran out of items, so inputs are in the right order");
        return true;
    }
    else if (rightIsListEnd && leftIsListEnd == false)
    {
         if (debugLog)
            Console.WriteLine($"{debugLogDepth} - Right side ran out of items, so inputs are not in the right order");
        return false;
    }
    else if (leftIsListEnd && rightIsListEnd)
        return ProcessPairRecursive(left.Slice(1), right.Slice(1), --amountOfListDepth);


    var leftIsListStart = left[0] == '[';
    var rightIsListStart = right[0] == '[';

    // Insert [] around int to list comparison
    if (leftIsListStart && rightIsListStart == false)
    {
        var rightAmountOfNumbers = 0;
        while (right[rightAmountOfNumbers] >= '0' && right[rightAmountOfNumbers] <= '9')
        {
            rightAmountOfNumbers++;
        }
        var rightNumber = int.Parse(right.Slice(0, rightAmountOfNumbers));

        if (debugLog)
        { 
            Console.WriteLine($"{debugLogDepth} - Compare {GetMatchingBrackets(left)} vs {rightNumber}");
            Console.WriteLine($"{debugLogDepth} - Mixed types; convert right to [{rightNumber}] and retry comparison");
        }
        right = right.ToString().Insert(0, "[").Insert(rightAmountOfNumbers + 1, "]").AsSpan();
        rightIsListStart = true;
    }
    else if (rightIsListStart && leftIsListStart == false)
    {
        var leftAmountOfNumbers = 0;
        while (left[leftAmountOfNumbers] >= '0' && left[leftAmountOfNumbers] <= '9')
        {
            leftAmountOfNumbers++;
        }
        var leftNumber = int.Parse(left.Slice(0, leftAmountOfNumbers));

        if (debugLog)
        {
            Console.WriteLine($"{debugLogDepth} - Compare {leftNumber} vs {GetMatchingBrackets(right)}");
            Console.WriteLine($"{debugLogDepth} - Mixed types; convert left to [{leftNumber}] and retry comparison");
        }
        left = left.ToString().Insert(0, "[").Insert(leftAmountOfNumbers + 1, "]").AsSpan();
        leftIsListEnd = true;
    }

    if (leftIsListStart == false && rightIsListStart == false)
    {
        var leftAmountOfNumbers = 0;
        while (left[leftAmountOfNumbers] >= '0' && left[leftAmountOfNumbers] <= '9')
        {
            leftAmountOfNumbers++;
        }

        var rightAmountOfNumbers = 0;
        while (right[rightAmountOfNumbers] >= '0' && right[rightAmountOfNumbers] <= '9')
        {
            rightAmountOfNumbers++;
        }
        var leftNumber = int.Parse(left.Slice(0, leftAmountOfNumbers));
        var rightNumber = int.Parse(right.Slice(0, rightAmountOfNumbers));
        if (debugLog)
            Console.WriteLine($"{debugLogDepth} - Compare {leftNumber} vs {rightNumber}");

        if (leftNumber < rightNumber)
        {
            if (debugLog)
                Console.WriteLine($"{debugLogDepth}   - Left side is smaller, so inputs are in the right order");

            return true;
        }
        else if (leftNumber > rightNumber)
        {
            if (debugLog)
                Console.WriteLine($"{debugLogDepth}   - Right side is smaller, so inputs are not in the right order");

            return false;
        }
        else
            return ProcessPairRecursive(left.Slice(leftAmountOfNumbers), right.Slice(leftAmountOfNumbers), amountOfListDepth);
    }
    else
    {
        if (debugLog)
            Console.WriteLine($"{debugLogDepth} - Compare {GetMatchingBrackets(left)} vs {GetMatchingBrackets(right)}");
        return ProcessPairRecursive(left.Slice(1), right.Slice(1), ++amountOfListDepth);
    }
}

ReadOnlySpan<char> GetMatchingBrackets(ReadOnlySpan<char> packet)
{
    Stack<int> stack = new Stack<int>();
    for(int i = 0; i < packet.Length; ++i)
    {
        switch(packet[i])
        {
            case '[':
                stack.Push(i);
                break;
            case ']':
                stack.Pop();
                if (stack.Count == 0)
                    return packet.Slice(0, i+1);
                break;
        }
    }
    return packet;
}