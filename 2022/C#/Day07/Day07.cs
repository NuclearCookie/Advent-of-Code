using Library;
using Day07;
using System.Diagnostics;

var commands = IO.ReadInputAsString(false).Split('$', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(command => command.Split(Environment.NewLine));
Day07.DirectoryInfo rootDirectory = new("/", null);
Day07.DirectoryInfo currentDirectory = rootDirectory;

foreach(var command in commands.Skip(1))
{
    if (command[0].StartsWith("cd"))
    {
        ChangeDirectory(command);
    }
    else if (command[0].StartsWith("ls"))
    {
        PopulateDirectory(command);
    }
}

List<Day07.DirectoryInfo> topDirsUnderThreshold = new();
RecursiveFindDirectoriesUnderThreshold(rootDirectory, 100000, topDirsUnderThreshold);

Console.WriteLine($"Total size of directories under threshold: {topDirsUnderThreshold.Aggregate(0, (sum, current) => current.ComputeSize() + sum)}");

var totalSizeInUse = rootDirectory.ComputeSize();
var totalSizeToClear = 30000000 - (70000000 - totalSizeInUse);
List<Day07.DirectoryInfo> allDirsOverThreshold = new();
RecursiveFindDirectoriesOverThreshold(rootDirectory, totalSizeToClear, allDirsOverThreshold);
allDirsOverThreshold.Sort((a, b) => a.ComputeSize() - b.ComputeSize());
Console.WriteLine($"The best directory to clear is {allDirsOverThreshold[0].Name}, with a size of {allDirsOverThreshold[0].ComputeSize()}");

void ChangeDirectory(string[] command)
{
    Debug.Assert(command.Length == 1, "Unhandled CD command");
    var dir = command[0].Substring(3);
    if (dir == "..")
    {
        currentDirectory = currentDirectory.Parent;
    }
    else
    {
        var existingDir = currentDirectory.Directories.Find(subDir => subDir.Name.Equals(dir));
        if (existingDir == null)
            currentDirectory = new Day07.DirectoryInfo(dir, currentDirectory);
        else
            currentDirectory = existingDir;
    }
}

void PopulateDirectory(string[] command)
{
    for(int i = 1; i < command.Length; ++i)
    {
        var entry = command[i].Split(' ');
        if (entry[0].StartsWith("dir"))
        {
            var dir = entry[1];
            var existingDir = currentDirectory.Directories.Find(subDir => subDir.Name.Equals(dir));
            if (existingDir != null)
                continue;
            new Day07.DirectoryInfo(dir, currentDirectory);
        }
        else
        {
            var size = int.Parse(entry[0]);
            var name = entry[1];
            var existingFile = currentDirectory.Files.Find(file => file.Name.Equals(name));
            if (existingFile != null)
                continue;
            currentDirectory.Files.Add(new Day07.File { Name = name, Size = size });
        }
    }
}

void RecursiveFindDirectoriesUnderThreshold(Day07.DirectoryInfo currentDirectory, int threshold, List<Day07.DirectoryInfo> outputList)
{
    var dirSize = currentDirectory.ComputeSize();
    if (dirSize <= threshold)
    {
        outputList.Add(currentDirectory);
    }
    foreach(var directory in currentDirectory.Directories)
    {
        RecursiveFindDirectoriesUnderThreshold(directory, threshold, outputList);
    }
}

void RecursiveFindDirectoriesOverThreshold(Day07.DirectoryInfo currentDirectory, int threshold, List<Day07.DirectoryInfo> outputList)
{
    var dirSize = currentDirectory.ComputeSize();
    if (dirSize > threshold)
    {
        outputList.Add(currentDirectory);
    }
    foreach(var directory in currentDirectory.Directories)
    {
        RecursiveFindDirectoriesOverThreshold(directory, threshold, outputList);
    }
}
