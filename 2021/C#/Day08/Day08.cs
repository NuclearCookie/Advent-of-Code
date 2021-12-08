using Library;
using System.Diagnostics;

var entries = IO.ReadInputAsStringArray().Select(x => new ScrambledSignalDecoder(x));
PartA();
PartB();

void PartA()
{
    var simpleDigits = entries.Aggregate(0, (acc, signal) => acc + signal.CountSimpleDigitsInOutputValues());
    Console.WriteLine($"Simple digits in output: {simpleDigits}");
}

void PartB()
{
    var summedOutputValues = entries.Aggregate(0, (acc, signal) => acc + signal.GetOutputValue());
    Console.WriteLine($"sum of output values: {summedOutputValues}");
}

class ScrambledSignalDecoder
{
    private string[] uniqueSignalPatterns;
    private string[] digitalOutputValues;
    private string[] sortedSignalPatterns;

    public ScrambledSignalDecoder(string entry)
    {
        var split = entry.Split('|');
        uniqueSignalPatterns = split[0].Trim().Split(' ');
        Array.Sort(uniqueSignalPatterns, (a, b) => a.Length - b.Length);
        digitalOutputValues = split[1].Trim().Split(' ');
        sortedSignalPatterns = new string[uniqueSignalPatterns.Length];
        Decode();
    }

    private void Decode()
    {
        // index 0 -> nr 1
        // index 1 -> nr 7
        // index 2 -> nr 4
        // index 3..5 -> nr 2, 5, 3
        // index 6..8 -> nr 6, 0, 9
        // index 9 -> nr 8

        sortedSignalPatterns[1] = uniqueSignalPatterns[0];
        sortedSignalPatterns[4] = uniqueSignalPatterns[2];
        sortedSignalPatterns[7] = uniqueSignalPatterns[1];
        sortedSignalPatterns[8] = uniqueSignalPatterns[9];
        // find top part by masking 7 with 1
        var topDigit = sortedSignalPatterns[7].Except(sortedSignalPatterns[1]).First();
        // find number 3 by masking 7 with all 5 digit numbers. if we masked 3 characters, it's 3.
        var checkForNumber3 = uniqueSignalPatterns.Where(x => x.Length == 5).Where(x => x.Intersect(sortedSignalPatterns[7]).Count() == 3);
        Debug.Assert(checkForNumber3.Count() == 1);
        sortedSignalPatterns[3] = checkForNumber3.First();
        var number3UniqueIndex = Array.FindIndex(uniqueSignalPatterns, x => x == sortedSignalPatterns[3]);
        // find top left by masking 4 with 3
        var checkForTopLeft = sortedSignalPatterns[4].Except(sortedSignalPatterns[3]);
        Debug.Assert(checkForTopLeft.Count() == 1);
        var topLeftDigit = checkForTopLeft.First().ToString();
        // find 9
        var checkFor9 = uniqueSignalPatterns[6..9].Where(x => x.Intersect(sortedSignalPatterns[4]).Count() == 4);
        Debug.Assert(checkFor9.Count() == 1);
        sortedSignalPatterns[9] = checkFor9.First();
        var checkFor6Or0 = uniqueSignalPatterns.Except(sortedSignalPatterns).Where(x => x.Length == 6);
        Debug.Assert(checkFor6Or0.Count() == 2);
        var checkTopRight = checkFor6Or0.First().Intersect(sortedSignalPatterns[1]);
        if (checkTopRight.Count() == 1)
        {
            sortedSignalPatterns[6] = checkFor6Or0.First();
            sortedSignalPatterns[0] = checkFor6Or0.Last();
        }
        else
        {
            sortedSignalPatterns[0] = checkFor6Or0.First();
            sortedSignalPatterns[6] = checkFor6Or0.Last();
        }

        sortedSignalPatterns[5] = uniqueSignalPatterns[3..6].Where(x => x.Intersect(topLeftDigit).Any()).First();
        sortedSignalPatterns[2] = uniqueSignalPatterns.Except(sortedSignalPatterns).First();
    }

    public int GetOutputValue()
    {
        int sum = 0;
        int outputLength = digitalOutputValues.Length;
        for(int i = 0; i < outputLength; i++)
        {
            var outputValue = digitalOutputValues[i];
            sum += Array.FindIndex(sortedSignalPatterns, x => x.Length == outputValue.Length && x.Intersect(outputValue).Count() == outputValue.Length) * (int)Math.Pow(10, outputLength - (i + 1));
        }
        return sum;
    }

    public int CountSimpleDigitsInOutputValues()
    {
        int simpleDigits = 0;
        foreach(var outputValue in digitalOutputValues)
        {
            if (outputValue.Length == 2 || outputValue.Length == 3 || outputValue.Length == 4 || outputValue.Length == 7)
                simpleDigits++;
        }
        return simpleDigits;
    }
}


