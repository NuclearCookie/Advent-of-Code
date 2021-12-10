using Library;

char[] validOpeningCharacters = new char[] { '(', '[', '{', '<' };
char[] validClosingCharacters = new char[] { ')', ']', '}', '>' };
int[] pricePerCharacter = new int[] { 3, 57, 1197, 25137 };
var navigationEntries = IO.ReadInputAsStringArray();
Stack<int> currentOpenBracketIndex = new Stack<int>();

PartAB();

void PartAB()
{
    var syntaxScores = 0;
    List<long> autoCorrectScores = new List<long>();
    foreach (var entry in navigationEntries)
    {
        var currentSyntax = GetSyntaxScore(entry);
        if (currentSyntax > 0)
        {
            syntaxScores += currentSyntax;
        }
        else
        {
            var autoCorrectScore = 0L;
            while(currentOpenBracketIndex.Count > 0)
            {
                autoCorrectScore = checked(autoCorrectScore * 5L + currentOpenBracketIndex.Pop() + 1);
            }
            autoCorrectScores.Add(autoCorrectScore);
        }
    }
    Console.WriteLine($"Total syntax score: {syntaxScores}");
    autoCorrectScores.Sort();
    Console.WriteLine($"Middle autocorrect score: {autoCorrectScores[autoCorrectScores.Count / 2]}");
}

int GetSyntaxScore(string entry)
{
    int openingCharIndex = Array.IndexOf(validOpeningCharacters, entry[0]);
    int closingCharIndex = Array.IndexOf(validClosingCharacters, entry[0]);
    if (openingCharIndex == -1)
    {
        return pricePerCharacter[closingCharIndex];
    }
    currentOpenBracketIndex.Clear();

    currentOpenBracketIndex.Push(openingCharIndex);
    for(int i = 1; i < entry.Length; ++i)
    {
        var character = entry[i];   
        openingCharIndex = Array.IndexOf(validOpeningCharacters, character);
        if (openingCharIndex != -1)
        {
            currentOpenBracketIndex.Push(openingCharIndex);
        }
        else
        {
            closingCharIndex = Array.IndexOf(validClosingCharacters, character);
            if (currentOpenBracketIndex.TryPop(out var expectedIndex) == false || expectedIndex != closingCharIndex)
            {
                return pricePerCharacter[closingCharIndex];
            }
        }
    }

    return 0;
}


