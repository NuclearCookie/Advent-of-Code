using Library;
using Library.Datastructures;

var inputArray = IO.ReadInputAsStringArray(false);
var rows = inputArray.Count();
var columns = inputArray.ElementAt(0).Length;
var forest = new Array2D<int>(rows, columns, inputArray.SelectMany(row => row.ToCharArray().Select(tree => int.Parse(tree.ToString()))).ToArray());
var visibleTreeMap = new Array2D<bool>(rows, columns);

Console.WriteLine("Quadcopter View:");
Console.WriteLine(forest.ToString());

// Count horizontal
for (int rowIndex = 0; rowIndex < rows; ++rowIndex)
{
    var row = forest.GetRow(rowIndex);
    // Count from the left
    var highestTree = -1;
    for (int columnIndex = 0; columnIndex < row.Length; ++columnIndex)
    {
        var tree = row[columnIndex];
        if (tree > highestTree)
        {
            visibleTreeMap[rowIndex, columnIndex] = true;
            highestTree = tree;
        }
    }

    // Count from the right
    highestTree = -1;
    for (int columnIndex = row.Length - 1; columnIndex >= 0; --columnIndex)
    {
        var tree = row[columnIndex];
        if (tree > highestTree)
        {
            visibleTreeMap[rowIndex, columnIndex] = true;
            highestTree = tree;
        }
    }
}

// Count vertical
for (int columnIndex = 0; columnIndex < columns; ++columnIndex)
{
    var column = forest.GetColumn(columnIndex);
    // Count from the top
    var highestTree = -1;
    for (int rowIndex = 0; rowIndex < column.Length; ++rowIndex)
    {
        var tree = column[rowIndex];
        if (tree > highestTree)
        {
            visibleTreeMap[rowIndex, columnIndex] = true;
            highestTree = tree;
        }
    }

    // Count from the right
    highestTree = -1;
    for (int rowIndex = column.Length - 1; rowIndex >= 0; --rowIndex)
    {
        var tree = column[rowIndex];
        if (tree > highestTree)
        {
            visibleTreeMap[rowIndex, columnIndex] = true;
            highestTree = tree;
        }
    }
}

Console.WriteLine("Visible tree map:");
Console.WriteLine(visibleTreeMap.ToString(result => result ? "x" : "o"));

var totalVisibleTrees = 0;
for(int i = 0; i < visibleTreeMap.Length; ++i)
{
    totalVisibleTrees += visibleTreeMap[i] ? 1 : 0;
}

Console.WriteLine($"Total visible trees: {totalVisibleTrees}");

// Part 2
int bestTreeScore = 0;
int bestTreeScoreIndex = -1;
for (int rowIndex = 1; rowIndex <= forest.RowLength - 1; ++rowIndex)
{
    for (int columnIndex = 1; columnIndex <= forest.ColumnLength - 1; ++columnIndex)
    {
        var currentTreeHeight = forest[rowIndex, columnIndex];
        // look left
        var leftTreeCount = 0;
        for (int leftIndex = columnIndex - 1; leftIndex >= 0; leftIndex--)
        {
            var leftTreeHeight = forest[rowIndex, leftIndex];
            leftTreeCount++;
            if (leftTreeHeight >= currentTreeHeight)
                break;
        }
        // look right
        var rightTreeCount = 0;
        for (int rightIndex = columnIndex + 1; rightIndex < forest.ColumnLength; rightIndex++)
        {
            var rightTreeHeight = forest[rowIndex, rightIndex];
            rightTreeCount++;
            if (rightTreeHeight >= currentTreeHeight)
                break;
        }
        // look up
        var upTreeCount = 0;
        for (int upIndex = rowIndex - 1; upIndex >= 0; upIndex--)
        {
            var upTreeHeight = forest[upIndex, columnIndex];
            upTreeCount++;
            if (upTreeHeight >= currentTreeHeight)
                break;
        }
        // look down
        var downTreeCount = 0;
        for (int downIndex = rowIndex + 1; downIndex < forest.RowLength; downIndex++)
        {
            var downTreeHeight = forest[downIndex, columnIndex];
            downTreeCount++;
            if (downTreeHeight >= currentTreeHeight)
                break;
        }
        var treeScore = leftTreeCount * rightTreeCount * upTreeCount * downTreeCount;
        if (treeScore > bestTreeScore)
        {
            bestTreeScore = treeScore;
            bestTreeScoreIndex = forest.RowColumnToIndex(rowIndex, columnIndex);
        }
    }
}

Console.WriteLine($"The best tree is at {forest.IndexToRowColumn(bestTreeScoreIndex)}, with a score of {bestTreeScore}");
