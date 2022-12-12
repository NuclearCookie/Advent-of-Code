using Library.Datastructures;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Library.Algorithms
{
    public static class Algorithms
    {
        private struct AStarNode
        {
            public int parentIndex;
            public int selfIndex;
            public int startCost;
            public int endCost;
            public int cost => startCost + endCost;
        }

        private class AStarNodeComparer : IComparer<AStarNode>
        {
            public int Compare(AStarNode x, AStarNode y)
            {
                var cost = x.cost - y.cost;
                if (cost == 0)
                {
                    return x.selfIndex - y.selfIndex;
                }
                return cost;
            }
        }

        public delegate int HeuristicFunctionDelegate(Point2 parentNode, Point2 currentNode, Point2 end);

        public static bool AStarPath(Array2D<int> weightTable, Point2 start, Point2 end, List<Point2> outPath, HeuristicFunctionDelegate? heuristic = null, int maximumCostToTraverseNode = int.MaxValue)
        {
            if (heuristic == null)
                heuristic = (parent, current, end) => current.GetManhattanDistance(end);

            var startIndex = weightTable.RowColumnToIndex(start);
            var endIndex = weightTable.RowColumnToIndex(end);
            var startNode = new AStarNode { startCost = 0, endCost = heuristic(start, start, end), parentIndex = startIndex, selfIndex = startIndex};
            SortedSet<AStarNode> openList = new SortedSet<AStarNode>(new AStarNodeComparer()) { startNode };
            HashSet<int> closedIndexList = new HashSet<int> ();
            Array2D<AStarNode> nodeGrid = new Array2D<AStarNode>(weightTable.RowLength, weightTable.ColumnLength, Enumerable.Repeat(new AStarNode {  startCost = -1 }, weightTable.Length).ToArray());
            nodeGrid[start] = startNode;
            int[] neighbourIndexCache = new int[4];

            while(openList.Count > 0)
            {
                AStarNode lowestCost = openList.First();
                openList.Remove(lowestCost);
                closedIndexList.Add(lowestCost.selfIndex);

                int neighbourCount = weightTable.GetNeighbourIndices(lowestCost.selfIndex, neighbourIndexCache);
                for(int i = 0; i < neighbourCount; ++i)
                {
                    var neighbourIndex = neighbourIndexCache[i];
                    AStarNode neighbourNode = new AStarNode { parentIndex = lowestCost.selfIndex, selfIndex = neighbourIndex };
                    var currentCoordinate = weightTable.IndexToRowColumn(lowestCost.selfIndex);
                    var neighbourCoordinate = weightTable.IndexToRowColumn(neighbourIndex);
                    if (neighbourIndex == endIndex && heuristic(currentCoordinate, neighbourCoordinate, end) < maximumCostToTraverseNode)
                    {
                        AStarNode current = neighbourNode;
                        while(current.selfIndex != startIndex)
                        {
                            outPath.Add(nodeGrid.IndexToRowColumn(current.selfIndex));
                            current = nodeGrid[current.parentIndex];
                        }
                        outPath.Add(nodeGrid.IndexToRowColumn(current.selfIndex));
                        outPath.Reverse();
                        return true;
                    }
                    else if (closedIndexList.Contains(neighbourIndex) == false)
                    {
                        // Note to future self. Don't try to mess with the startCost, however tempting it may be.
                        // If you need control, find a good heuristic to use as endCost
                        neighbourNode.startCost = lowestCost.startCost + weightTable[neighbourIndex];
                        neighbourNode.endCost = heuristic(currentCoordinate, neighbourCoordinate, end);
                        if (neighbourNode.endCost < maximumCostToTraverseNode && nodeGrid[neighbourIndex].cost < 0 || nodeGrid[neighbourIndex].cost > neighbourNode.cost)
                        {
                            var success = openList.Add(neighbourNode);
                            nodeGrid[neighbourIndex] = neighbourNode;
                        }
                    }
                }
            }

            return false;
        }
    }

    public static class Math
    {
        public static int GreatestCommonDivisor(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static int LeastCommonMultiple(int a, int b)
        {
            return (a / GreatestCommonDivisor(a, b)) * b;
        }

        public static float Lerp(float from, float to, float amount)
        {
            return from + amount * (to - from);
        }

        public static double Lerp(double from, double to, double amount)
        {
            return from + amount * (to - from);
        }
    }
}
