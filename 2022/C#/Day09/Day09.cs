using Library;
using Library.Datastructures;

var ropeDirections = IO.ReadInputAsStringArray(false).Select(
    line => 
    { 
        var split = line.Split(' ');
        return (split[0], int.Parse(split[1]));
    });

Point2[] shortRope = new Point2[2];
Point2[] longRope = new Point2[10];

//var visitedShortTailPositions = new HashSet<Point2>(); // MoveRope(shortRope);
var visitedShortTailPositions = MoveRope(shortRope);
var visitedLongTailPositions = MoveRope(longRope);

HashSet<Point2> MoveRope(Point2[] rope)
{
    Point2 headPosition = new Point2(0, 0);
    HashSet<Point2> visitedTailPositions = new HashSet<Point2>();

    foreach (var direction in ropeDirections)
    {
        var moves = direction.Item2;
        var moveDelta = new Point2(0, 0);
        switch (direction.Item1)
        {
            case "U": // move UP
                moveDelta.Y++;
                break;
            case "D": // move DOWN
                moveDelta.Y--;
                break;
            case "R": // move RIGHT
                moveDelta.X++;
                break;
            case "L": // move LEFT
                moveDelta.X--;
                break;
        }
        for (int i = 0; i < moves; ++i)
        {
            headPosition += moveDelta;
            rope[0] = headPosition;
            // move tail to keep up.
            for (int j = 1; j < rope.Length; ++j)
            {
                var previousRopePosition = rope[j - 1];
                var currentRopePosition = rope[j];
                var diff = previousRopePosition - currentRopePosition;
                if (Math.Abs(diff.X) > 1) // needs to move!
                {
                    currentRopePosition.X += diff.X > 0 ? 1 : -1;
                    if (diff.Y != 0)
                        currentRopePosition.Y += diff.Y > 0 ? 1 : -1;
                }
                diff = previousRopePosition - currentRopePosition;
                if (Math.Abs(diff.Y) > 1) // needs to move!
                {
                    currentRopePosition.Y += diff.Y > 0 ? 1 : -1;
                    if (diff.X != 0)
                        currentRopePosition.X += diff.X > 0 ? 1 : -1;
                }
                rope[j] = currentRopePosition; // not sure if we have to assign here but just do it to be sure.
            }
            visitedTailPositions.Add(rope[^1]);
        }
    }
    return visitedTailPositions;
}

DebugDraw(visitedLongTailPositions);

Console.WriteLine($"Visited tailpositions for short rope: {visitedShortTailPositions.Count}");
Console.WriteLine($"Visited tailpositions for long rope: {visitedLongTailPositions.Count}");

void DebugDraw(HashSet<Point2> tailPositions)
{
    // find bounds
    var lowestX = 0;
    var lowestY = 0;
    var highestX = 0;
    var highestY = 0;

    foreach(var position in tailPositions)
    {
        if (position.X < lowestX)
        {
            lowestX = position.X;
        }
        else if (position.X > highestX)
        {
            highestX = position.X;
        }
        if (position.Y < lowestY)
        {
            lowestY = position.Y;
        }
        else if (position.Y > highestY)
        {
            highestY = position.Y;
        }
    }

    Array2D<char> grid = new Library.Datastructures.Array2D<char>((highestY - lowestY) + 2, (highestX - lowestX) + 2);

    for(int i = 0; i < grid.Length; ++i)
    {
        var point = grid.IndexToRowColumn(i);
        point.X += lowestX - 1;
        point.Y += lowestY - 1;
        if (tailPositions.Contains(point))
            grid[i] = '#';
        else
            grid[i] = '.';
    }

    Console.WriteLine(grid.ToString());
}
