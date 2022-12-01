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

        public static bool AStarPath(Array2D<int> weightTable, Point2 start, Point2 end, List<Point2> outPath)
        {
            var startIndex = weightTable.RowColumnToIndex(start);
            var endIndex = weightTable.RowColumnToIndex(end);
            var startNode = new AStarNode { startCost = 0, endCost = end.GetManhattanDistance(start), parentIndex = startIndex, selfIndex = startIndex};
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

                    if (neighbourIndex == endIndex)
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
                        neighbourNode.startCost = lowestCost.startCost + weightTable[neighbourIndex];
                        neighbourNode.endCost = weightTable.IndexToRowColumn(neighbourIndex).GetManhattanDistance(end);
                        if (nodeGrid[neighbourIndex].cost < 0 || nodeGrid[neighbourIndex].cost > neighbourNode.cost)
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
}
