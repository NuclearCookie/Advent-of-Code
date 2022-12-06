using Library;

var dataStream = IO.ReadInputAsString(false);
var markerPos = FindSequentialUniqueCharacters(4);
var messagePos = FindSequentialUniqueCharacters(14);

Console.WriteLine($"Marker position found at index {markerPos}");
Console.WriteLine($"Message position found at index {messagePos}");

int FindSequentialUniqueCharacters(int count)
{
    for (int i = count - 1; i < dataStream.Length; i++)
    {
        var lastFourChars = dataStream.AsSpan(i - (count - 1), count);
        if (lastFourChars.ToArray().Distinct().Count() == count)
        {
            return i + 1;
        }
    }
    return 0;
}
