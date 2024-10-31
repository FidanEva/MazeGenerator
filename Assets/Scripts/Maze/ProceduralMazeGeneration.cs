using System;
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
                    _grid[i, j] = mazeCell;

                    if (i == 0 && j == 0)
                        mazeCell.IsEntrance();

                    if (i == gridSize.x - 1 && j == gridSize.y - 1)
                        mazeCell.IsExit();
                }
            }
        }

        private IEnumerator GenerateMaze(MazeCell previousMazeCell, MazeCell currentMazeCell)
        {
            currentMazeCell.Visit();
            ClearWalls(previousMazeCell, currentMazeCell);

            yield return new WaitForSeconds(0.5f);

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
            var gridPoint = ((Vector2)currentMazeCell.transform.position - _startPosition) / cellSize;
            var (i, j) = ((int)gridPoint.x, (int)gridPoint.y);

            if (gridPoint.x + 1 < gridSize.x)
                if (!_grid[i + 1, j].IsVisited)
                    yield return _grid[i + 1, j];

            if (gridPoint.x - 1 > 0)
                if (!_grid[i - 1, j].IsVisited)
                    yield return _grid[i - 1, j];

            if (gridPoint.y + 1 < gridSize.y)
                if (!_grid[i, j + 1].IsVisited)
                    yield return _grid[i, j + 1];

            if (gridPoint.y - 1 > 0)
                if (!_grid[i, j - 1].IsVisited)
                    yield return _grid[i, j - 1];
        }

        private void ClearWalls(MazeCell previousMazeCell, MazeCell currentMazeCell)
        {
            if (previousMazeCell is null) return;

            if (previousMazeCell.transform.position.x < currentMazeCell.transform.position.x)
            {
                previousMazeCell.ClearRightWall();
                currentMazeCell.ClearLeftWall();
            }

            if (previousMazeCell.transform.position.x > currentMazeCell.transform.position.x)
            {
                previousMazeCell.ClearLeftWall();
                currentMazeCell.ClearRightWall();
            }

            if (previousMazeCell.transform.position.y < currentMazeCell.transform.position.y)
            {
                previousMazeCell.ClearTopWall();
                currentMazeCell.ClearBottomWall();
            }

            if (previousMazeCell.transform.position.y > currentMazeCell.transform.position.y)
            {
                previousMazeCell.ClearBottomWall();
                currentMazeCell.ClearTopWall();
            }
        }
    }
}