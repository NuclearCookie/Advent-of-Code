using Library;

var binaryNumbers = IO.ReadInputAsStringArray().ToArray();
PartA(binaryNumbers);
PartB(binaryNumbers);

static void PartA(string[] binaryNumbers)
{
    var lines = binaryNumbers.Length;
    var lineLength = binaryNumbers[0].Length;

    var onCount = new int[lineLength];
    var gammaRate = 0;
    for (int i = 0; i < lineLength; i++)
    {
        for (int j = 0; j < lines; j++)
        {
            onCount[i] += binaryNumbers[j][i] - '0'; // char to int
        }
        if (onCount[i] * 2 > lines) // 1 is significant
        {
            gammaRate |= 1 << lineLength - i - 1;
        }
    }
    var epsilonMask = int.MaxValue >> sizeof(int) * 8 - lineLength - 1;
    var epsilonRate = ~gammaRate & epsilonMask;
    Console.WriteLine($"GammaRate: {gammaRate}, EpsilonRate: {epsilonRate}. Result: {gammaRate * epsilonRate}");
}

static void PartB(string[] binaryNumbers)
{
    var lineLength = binaryNumbers[0].Length;
    var oxigenRatings = new List<string>(binaryNumbers);
    var co2Ratings = new List<string>(binaryNumbers);


    var index = 0;
    List<string> startsOn = new List<string>();
    List<string> startsOff = new List<string>();
    while ((oxigenRatings.Count > 1 || co2Ratings.Count > 1) && index < lineLength)
    {
        if (oxigenRatings.Count > 1)
        {
            startsOn.Clear();
            startsOff.Clear();
            foreach (string rating in oxigenRatings)
            {
                if (rating[index] - '0' == 1)
                {
                    startsOn.Add(rating);
                }
                else
                {
                    startsOff.Add(rating);
                }
            }
            oxigenRatings.Clear();
            if (startsOn.Count - startsOff.Count >= 0)
            {
                oxigenRatings.AddRange(startsOn);
            }
            else
            {
                oxigenRatings.AddRange(startsOff);
            }
        }
        if (co2Ratings.Count > 1)
        {
            startsOn.Clear();
            startsOff.Clear();
            foreach (string rating in co2Ratings)
            {
                if (rating[index] - '0' == 1)
                {
                    startsOn.Add(rating);
                }
                else
                {
                    startsOff.Add(rating);
                }
            }
            co2Ratings.Clear();
            if (startsOn.Count - startsOff.Count >= 0)
            {
                co2Ratings.AddRange(startsOff);
            }
            else
            {
                co2Ratings.AddRange(startsOn);
            }
        }
        index++;
    }

    int oxigenRating = Convert.ToInt32(oxigenRatings[0], 2);
    int co2Rating = Convert.ToInt32(co2Ratings[0], 2);
    Console.WriteLine($"Oxigen Rating: {oxigenRating}, CO2 Rating: {co2Rating}, result: {oxigenRating * co2Rating}");


}
