using Library;
using Library.Datastructures;

var input = IO.ReadInputAsStringArray();
var commands = input.Select(x => x.Trim().Split(' '));
PartA(commands);
PartB(commands);

void PartA(IEnumerable<string[]> commands)
{
    var position = new Point2();
    foreach(var command in commands)
    {
        switch (command[0])
        {
            case "forward":
                position += new Point2 { X = int.Parse(command[1]) };
                break;
            case "down":
                position += new Point2 { Y = int.Parse(command[1]) };
                break;
            case "up":
                position -= new Point2 { Y = int.Parse(command[1]) };
                break;
            default:
                throw new Exception("Unknown command!");
        }
    }

    Console.WriteLine($"A: Forward: {position.X}, depth: {position.Y}. Result: {position.X * position.Y}");
}

void PartB(IEnumerable<string[]> commands)
{
    var position = new Point2();
    var aim = 0;
    foreach(var command in commands)
    {
        switch (command[0])
        {
            case "forward":
                var result = int.Parse(command[1]);
                position += new Point2 { X = result, Y = aim * result };
                break;
            case "down":
                aim += int.Parse(command[1]);
                break;
            case "up":
                aim -= int.Parse(command[1]);
                break;
            default:
                throw new Exception("Unknown command!");
        }
    }

    Console.WriteLine($"B: Forward: {position.X}, depth: {position.Y}. Result: {position.X * position.Y}");
}
