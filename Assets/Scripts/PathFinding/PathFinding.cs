using System.Collections.Generic;
using System.Linq;
using ProceduralMazeGeneration;
using UnityEngine;
using Zenject;

namespace PathFinding
{
    public class PathFinding
    {
        private const int MoveStraightCost = 10;
        private const int MoveDiagonalCost = 10;

        private List<MazeCell> _openList;
        private List<MazeCell> _closedList;

        private readonly ProceduralMazeGeneration.ProceduralMazeGeneration _mazeGeneration;

        [Inject]
        public PathFinding(ProceduralMazeGeneration.ProceduralMazeGeneration mazeGeneration)
        {
            Debug.Log("pathFinding cor");
            _mazeGeneration = mazeGeneration;
        }

        public List<MazeCell> FindPath(int startX, int startY, int endX, int endY)
        {
            MazeCell startNode = _mazeGeneration.Grid[startX, startY];
            MazeCell endNode = _mazeGeneration.Grid[endX, endY];

            if (startNode is null || endNode is null)
            {
                Debug.LogError("Start or end node is null");
                return null;
            }

            _openList = new List<MazeCell> { startNode };
            _closedList = new List<MazeCell>();

            for (var x = 0; x < _mazeGeneration.Grid.GetLength(0); x++)
            {
                for (var y = 0; y < _mazeGeneration.Grid.GetLength(1); y++)
                {
                    MazeCell cell = _mazeGeneration.Grid[x, y];
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
            {
                var leftNode = _mazeGeneration.Grid[currentNode.x - 1, currentNode.y];
                if (!currentNode.WallIsActive(Side.Left) && !leftNode.WallIsActive(Side.Right))
                    neighbourList.Add(leftNode);
            }

            if (currentNode.x + 1 < _mazeGeneration.Grid.GetLength(0))
            {
                var rightNode = _mazeGeneration.Grid[currentNode.x + 1, currentNode.y];
                if (!currentNode.WallIsActive(Side.Right) && !rightNode.WallIsActive(Side.Left))
                    neighbourList.Add(rightNode);
            }

            if (currentNode.y - 1 >= 0)
            {
                var bottomNode = _mazeGeneration.Grid[currentNode.x, currentNode.y - 1];
                if (!currentNode.WallIsActive(Side.Bottom) && !bottomNode.WallIsActive(Side.Top))
                    neighbourList.Add(bottomNode);
            }

            if (currentNode.y + 1 < _mazeGeneration.Grid.GetLength(1))
            {
                var topNode = _mazeGeneration.Grid[currentNode.x, currentNode.y + 1];
                if (!currentNode.WallIsActive(Side.Top) && !topNode.WallIsActive(Side.Bottom))
                    neighbourList.Add(topNode);
            }

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