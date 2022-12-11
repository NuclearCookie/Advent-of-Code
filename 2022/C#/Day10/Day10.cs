using Library;
using Library.Datastructures;
using System.Diagnostics;

var instructions = IO.ReadInputAsStringArray();
Queue<IInstruction> queue = new Queue<IInstruction>();
Array2D<bool> crtScreen = new Array2D<bool>(6, 40);

foreach(var instruction in instructions)
{
    if (instruction.Trim().Equals("noop"))
    {
        queue.Enqueue(new NoOp());
    }
    else
    {
        var addX = instruction.Split(' ');
        Debug.Assert(addX[0].Equals("addx"));
        queue.Enqueue(new AddX(int.Parse(addX[1])));
    }
}

int totalSignalStrength = ExecuteCPU(new[] {20, 60, 100, 140, 180, 220});
Console.WriteLine($"AddX Registry Value after executing: {Registry.X}. SignalStrengths: {totalSignalStrength}");
Console.WriteLine(crtScreen.ToString(pixel => pixel? "#" : "."));

int ExecuteCPU(int[] ticksToReadValue)
{
    var tick = 0;
    var totalValue = 0;
    while(queue.Count > 0)
    {
        var instruction = queue.Dequeue();
        do
        {
            tick++;
            if (ticksToReadValue.Contains(tick))
            {
                totalValue += tick * Registry.X;
            }
            if (Math.Abs(Registry.X - ((tick - 1)% crtScreen.ColumnLength)) <= 1) // overlapping registry value with CRT pixel.
            {
                crtScreen[tick - 1] = true;
            }
        }
        while (instruction.Update() == false);
    }
    return totalValue;
}

static class Registry
{
    public static int X = 1;
}

interface IInstruction
{
    bool Update();
}

class NoOp : IInstruction
{
    public bool Update()
    {
        return true;
    }
}

class AddX : IInstruction
{
    bool isReadyToUpdate = false;
    int addition = 0;

    public AddX(int _addition)
    {
        addition = _addition;
    }

    public bool Update()
    {
        if (isReadyToUpdate == false)
        {
            isReadyToUpdate = true;
            return false;
        }
        Registry.X += addition;
        return true;
    }
}
