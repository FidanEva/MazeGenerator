using System.Collections.Generic;
using System.Linq;
using ProceduralMazeGeneration;
using UnityEngine;

namespace PathFinding
{
    public class PathFinding
    {
        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 10;

        private readonly MazeCell[,] _grid;
        private List<MazeCell> _openList;
        private List<MazeCell> _closedList;

        public PathFinding(MazeCell[,] argGrid)
        {
            _grid = argGrid;
        }

        public List<MazeCell> FindPath(int startX, int startY, int endX, int endY)
        {
            MazeCell startNode = _grid[startX, startY];
            MazeCell endNode = _grid[endX, endY];

            if (startNode is null || endNode is null)
            {
                Debug.LogError("Start or end node is null");
                return null;
            }

            _openList = new List<MazeCell> { startNode };
            _closedList = new List<MazeCell>();

            for (var x = 0; x < _grid.GetLength(0); x++)
            {
                for (var y = 0; y < _grid.GetLength(1); y++)
                {
                    MazeCell cell = _grid[x, y];
                    cell.gCost = int.MaxValue;
                    cell.CalculateFCost();
                    cell.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (_openList.Count > 0)
            {
                MazeCell currentNode = _openList.OrderBy(cell => cell.fCost).First();

                if (currentNode == endNode)
                    return RetracePath(startNode, endNode);

                _openList.Remove(currentNode);
                _closedList.Add(currentNode);

                foreach (MazeCell neighbourNode in GetNeighbourList(currentNode))
                {
                    if (_closedList.Contains(neighbourNode)) continue;

                    int newGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    if (newGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = newGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        if (!_openList.Contains(neighbourNode))
                        {
                            _openList.Add(neighbourNode);
                        }
                    }
                }
            }

            Debug.LogError("No path found");
            return null;
        }

        private List<MazeCell> GetNeighbourList(MazeCell currentNode)
        {
            List<MazeCell> neighbourList = new List<MazeCell>();

            if (currentNode.x - 1 >= 0)
                neighbourList.Add(_grid[currentNode.x - 1, currentNode.y]);

            if (currentNode.x + 1 < _grid.GetLength(0))
                neighbourList.Add(_grid[currentNode.x + 1, currentNode.y]);

            if (currentNode.y - 1 >= 0)
                neighbourList.Add(_grid[currentNode.x, currentNode.y - 1]);

            if (currentNode.y + 1 < _grid.GetLength(1))
                neighbourList.Add(_grid[currentNode.x, currentNode.y + 1]);

            return neighbourList;
        }

        private int CalculateDistanceCost(MazeCell a, MazeCell b)
        {
            var xDistance = Mathf.Abs(a.x - b.x);
            var yDistance = Mathf.Abs(a.y - b.y);
            var remaining = Mathf.Abs(xDistance - yDistance);
            return MoveDiagonalCost * Mathf.Min(xDistance, yDistance) + MoveStraightCost * remaining;
        }

        private List<MazeCell> RetracePath(MazeCell start, MazeCell end)
        {
            var path = new List<MazeCell>();
            var current = end;

            while (current != start)
            {
                path.Add(current);
                current = current.cameFromNode;
            }

            path.Reverse();
            return path;
        }
    }
}