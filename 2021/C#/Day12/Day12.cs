using Library;

var connections = IO.ReadInputAsStringArray().ToArray();
Dictionary<Node, List<Node>> caveConnections = new Dictionary<Node, List<Node>>();
Node startNode = default;
Node endNode = default;
bool canVisitSingleCaveTwice = false;

foreach(var connection in connections)
{
    var link = connection.Split('-');
    var linkA = link[0];
    Node nodeA = new Node();
    switch (linkA)
    {
        case "start":
            startNode = nodeA;
            nodeA.Type = NodeType.Start;
            break;
        case "end":
            endNode = nodeA;
            nodeA.Type = NodeType.End;
            break;
        default:
            nodeA.Type = char.IsUpper(linkA[0]) ? NodeType.BigCave : NodeType.SmallCave;
            break;
    }
    nodeA.NodeId = linkA;

    var linkB = link[1];
    Node nodeB = new Node();
    switch (linkB)
    {
        case "start":
            startNode = nodeB;
            nodeB.Type = NodeType.Start;
            break;
        case "end":
            endNode = nodeB;
            nodeB.Type = NodeType.End;
            break;
        default:
            nodeB.Type = char.IsUpper(linkB[0]) ? NodeType.BigCave : NodeType.SmallCave;
            break;
    }
    nodeB.NodeId = linkB;
    if (caveConnections.TryGetValue(nodeA, out var caveConnectionForNodeA) == false)
    {
        caveConnectionForNodeA = new List<Node>();
        caveConnections[nodeA] = caveConnectionForNodeA;
    }
    if (caveConnections.TryGetValue(nodeB, out var caveConnectionForNodeB) == false)
    {
        caveConnectionForNodeB = new List<Node>();
        caveConnections[nodeB] = caveConnectionForNodeB;
    }
    caveConnectionForNodeA.Add(nodeB);
    caveConnectionForNodeB.Add(nodeA);
}

PartA();
PartB();

void PartA()
{
    int possiblePaths = CountAllPossiblePathsRecursive(startNode, default, new List<Node>(), false);
    Console.WriteLine($"Part A: Possible paths through the caves: {possiblePaths}");
}

void PartB()
{
    canVisitSingleCaveTwice = true;
    int possiblePaths = CountAllPossiblePathsRecursive(startNode, default, new List<Node>(), false);
    Console.WriteLine($"Part B: Possible paths through the caves: {possiblePaths}");
}

int CountAllPossiblePathsRecursive(Node currentCave, Node previousCave, List<Node> visitedNodes, bool hasFoundSmallCaveTwice)
{
    if (currentCave.Type == NodeType.End)
    {
        return 1;
    }
    if (currentCave.Type == NodeType.Start && visitedNodes.Contains(currentCave))
    {
        return 0;
    }
    if (currentCave.Type == NodeType.SmallCave && visitedNodes.Contains(currentCave))
    {
        if (!canVisitSingleCaveTwice || hasFoundSmallCaveTwice)
            return 0;
        hasFoundSmallCaveTwice = true;
    }
    visitedNodes.Add(currentCave);

    var result = 0;
    var possibleRoutes = caveConnections[currentCave];
    foreach(var possibleRoute in possibleRoutes)
    {
        List<Node> newVisitedNodes = new List<Node>(visitedNodes);
        result += CountAllPossiblePathsRecursive(possibleRoute, currentCave, newVisitedNodes, hasFoundSmallCaveTwice);
    }
    return result;
}


enum NodeType
{
    Start,
    BigCave,
    SmallCave,
    End
}

class Node : IEquatable<Node>
{
    public NodeType Type;
    public string NodeId;

    public bool Equals(Node? other)
    {
        if (other == null)
            return false;
        return other.Type == Type && other.NodeId == NodeId;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Node node)
            return this.Equals(node);
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type, NodeId);
    }

    public override string ToString()
    {
        return $"{NodeId}";
    }
}


