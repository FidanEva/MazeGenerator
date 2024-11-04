using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProceduralMazeGeneration
{
    public class ProceduralMazeGeneration : MonoBehaviour
    {
        [SerializeField] private MazeCell mazeCellPrefab;

        [SerializeField] private Vector2 cellSize;
        [SerializeField] private Vector2Int gridSize;

        private MazeCell[,] _grid;
        private Vector2 _startPosition;

        private void Start()
        {
            _grid = new MazeCell[gridSize.x, gridSize.y];

            _startPosition = new Vector2(-(cellSize.x * gridSize.x) * 0.5f + cellSize.x * 0.5f,
                -(cellSize.y * gridSize.y) * 0.5f + cellSize.y * 0.5f);

            GenerateMaze();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
                ReGenerateMaze();

            if (Input.GetKeyDown(KeyCode.P))
                SetPathFinding();
        }

        private void GenerateMaze()
        {
            PopulateCells();
            StartCoroutine(GenerateMaze(null, _grid[0, 0]));
        }

        private void ReGenerateMaze()
        {
            ClearGrid();
            StartCoroutine(GenerateMaze(null, _grid[0, 0]));
        }

        private void ClearGrid()
        {
            foreach (var mazeCell in _grid)
                mazeCell.Reset();
        }

        private void PopulateCells()
        {
            for (var i = gridSize.x - 1; i >= 0; i--)
            {
                for (var j = gridSize.y - 1; j >= 0; j--)
                {
                    var cellPosition = new Vector2(_startPosition.x + i * cellSize.x,
                        _startPosition.y + j * cellSize.y);

                    var mazeCell = Instantiate(mazeCellPrefab, cellPosition, Quaternion.identity);
                    mazeCell.SetGridIndices(i, j);
                    _grid[i, j] = mazeCell;
                    
                    if (i == 0 && j == 0)
                        mazeCell.InitDoor(Door.Entrance);

                    if (i == gridSize.x - 1 && j == gridSize.y - 1)
                        mazeCell.InitDoor(Door.Exit);
                }
            }
        }

        private IEnumerator GenerateMaze(MazeCell previousMazeCell, MazeCell currentMazeCell)
        {
            currentMazeCell.Visit();
            ClearWalls(previousMazeCell, currentMazeCell);

            yield return null;

            MazeCell nextMazeCell;
            do
            {
                nextMazeCell = GetNextUnvisitedMazeCell(currentMazeCell);
                if (nextMazeCell is not null)
                {
                    yield return GenerateMaze(currentMazeCell, nextMazeCell);
                }
            } while (nextMazeCell is not null);
        }

        private MazeCell GetNextUnvisitedMazeCell(MazeCell currentMazeCell)
        {
            var unvisitedNeighbours = GetUnvisitedNeighbours(currentMazeCell);
            return unvisitedNeighbours.Shuffle().FirstOrDefault();
        }

        private IEnumerable<MazeCell> GetUnvisitedNeighbours(MazeCell currentMazeCell)
        {
            var position = currentMazeCell.transform.position;

            var i = Mathf.RoundToInt((position.x - _startPosition.x) / cellSize.x);
            var j = Mathf.RoundToInt((position.y - _startPosition.y) / cellSize.y);

            if (i + 1 < gridSize.x)
                if (_grid[i + 1, j].IsVisited == false)
                    yield return _grid[i + 1, j];

            if (i - 1 >= 0)
                if (_grid[i - 1, j].IsVisited == false)
                    yield return _grid[i - 1, j];

            if (j + 1 < gridSize.y)
                if (_grid[i, j + 1].IsVisited == false)
                    yield return _grid[i, j + 1];

            if (j - 1 >= 0)
                if (_grid[i, j - 1].IsVisited == false)
                    yield return _grid[i, j - 1];
        }

        private void ClearWalls(MazeCell previousMazeCell, MazeCell currentMazeCell)
        {
            if (previousMazeCell is null) return;

            if (previousMazeCell.transform.position.x < currentMazeCell.transform.position.x)
            {
                previousMazeCell.ClearWall(Side.Right);
                currentMazeCell.ClearWall(Side.Left);
            }

            if (previousMazeCell.transform.position.x > currentMazeCell.transform.position.x)
            {
                previousMazeCell.ClearWall(Side.Left);
                currentMazeCell.ClearWall(Side.Right);
            }

            if (previousMazeCell.transform.position.y < currentMazeCell.transform.position.y)
            {
                previousMazeCell.ClearWall(Side.Top);
                currentMazeCell.ClearWall(Side.Bottom);
            }

            if (previousMazeCell.transform.position.y > currentMazeCell.transform.position.y)
            {
                previousMazeCell.ClearWall(Side.Bottom);
                currentMazeCell.ClearWall(Side.Top);
            }
        }

        private void SetPathFinding()
        {
            PathFinding.PathFinding pathfinding = new PathFinding.PathFinding(_grid);
            var pathNodes = pathfinding.FindPath(0, 0, gridSize.x - 1, gridSize.y - 1);
            Debug.Log(pathNodes);
            pathNodes.ForEach(n => Debug.Log($"{n.x} {n.y}"));
        }
    }
}