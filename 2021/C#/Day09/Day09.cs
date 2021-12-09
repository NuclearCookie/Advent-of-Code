using Library;
using Library.Datastructures;

var heightField = IO.ReadInputAsStringArray().Select(x => x.ToCharArray().Select(x => x - '0').ToArray()).ToArray();
Array2D<int> heightField2D = new Array2D<int>(heightField);
Array2D<bool> processedPoints = new Array2D<bool>(heightField2D.RowLength, heightField2D.ColumnLength);

int[] neighbours = new int[4];
List<int> lowPointIndices = new List<int>();

PartA();
PartB();

void PartA()
{
    var riskLevel = 0;
    for (int i = 0; i < heightField2D.Length; ++i)
    {
        var height = heightField2D[i];
        var validNeighbours = heightField2D.GetNeighbours(i, neighbours);
        if (neighbours[0..validNeighbours].Min() <= height)
        {
            continue;
        }

        lowPointIndices.Add(i);
        processedPoints[i] = true;
        riskLevel += 1 + height;
    }
    Console.WriteLine($"RiskLevel: {riskLevel}");
}

void PartB()
{
    for(int i = 0; i < heightField2D.Length; ++i)
    {
        if (heightField2D[i] == 9)
        {
            processedPoints[i] = true;
        }
    }
    List<int> slopeSizes = new List<int>();
    foreach(var lowPointIndex in lowPointIndices)
    {
        slopeSizes.Add(RecursiveFindSlopeSize(lowPointIndex));
    }
    slopeSizes.Sort((a, b) => b - a);
    Console.WriteLine($"Size of 3 biggest basins: {slopeSizes[0]}, {slopeSizes[1]}, {slopeSizes[2]}. Multiplied: {slopeSizes.GetRange(0, 3).Aggregate(1, (curr, val) => curr * val)}");
}

int RecursiveFindSlopeSize(int currentLowIndex)
{
    int result = 1;
    var currentLow = heightField2D[currentLowIndex];
    var localNeighbourIndices = new int[4];
    int neighbourCount = heightField2D.GetNeighbourIndices(currentLowIndex, localNeighbourIndices);
    for(int i = 0; i < neighbourCount; ++i)
    {
        var neighbourIndex = localNeighbourIndices[i];
        if (processedPoints[neighbourIndex] == false)
        {
            var neighbourValue = heightField2D[neighbourIndex];
            if (neighbourValue > currentLow)
            {
                processedPoints[neighbourIndex] = true;
                result += RecursiveFindSlopeSize(neighbourIndex);
            }
        }
    }
    return result;
}
