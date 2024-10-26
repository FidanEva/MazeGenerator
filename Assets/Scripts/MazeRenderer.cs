using System.Collections;
using UnityEngine;

namespace Labrynth
{
    public class MazeRenderer : MonoBehaviour
    {
        [SerializeField] [Range(1, 50)] private int width = 10;
        [SerializeField] [Range(1, 50)] private int height = 10;

        [SerializeField] private MazeCell horizontalWallPrefab = null;
        [SerializeField] private MazeCell verticalWallPrefab = null;
        [SerializeField] private MazeCell downWallPrefab = null;
        [SerializeField] private Transform floorPrefab = null;

        private const float VerticalOverlapOffset = 2 / 3f;

        public void Generate()
        {
            Clear();

            var maze = MazeGenerator.Generate(width, height);
            StartCoroutine(Draw(maze));
        }

        private void Clear()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                Generate();
            }
        }

        private IEnumerator Draw(WallState[,] maze)
        {
            var boundSize = verticalWallPrefab.size;

            var allWidth = (2 * width + 1) * boundSize.x;
            var allHeight = boundSize.y + (height - 1) * (boundSize.y * VerticalOverlapOffset);

            Instantiate(floorPrefab, transform);

            for (var i = 0; i < width; ++i)
            {
                for (var j = 0; j < height; ++j)
                {
                    var cell = maze[i, j];

                    var position = new Vector2(-allWidth / 2 + i * (boundSize.x),
                                       -allHeight / 2 + j * (boundSize.y * VerticalOverlapOffset)) + boundSize * 0.5f +
                                   (Vector2)transform.position;

                    if (cell.HasFlag(WallState.UP))
                    {
                        var yPos = (j == height - 1)
                            ? (-boundSize.y / 2 + horizontalWallPrefab.size.y / 2) + horizontalWallPrefab.size.y * 2
                            : -(-boundSize.y / 2 + horizontalWallPrefab.size.y / 2); // - 0.225f;

                        var topWall = Instantiate(horizontalWallPrefab, transform);
                        topWall.transform.position = position + new Vector2(boundSize.x + i * boundSize.x,
                            yPos);

                        if (!cell.HasFlag(WallState.LEFT))
                        {
                            var rightTopWall = Instantiate(horizontalWallPrefab, transform);
                            rightTopWall.transform.position = position + new Vector2(i * boundSize.x,
                                yPos);
                        }

                        if (j == height - 1 && i == width - 1)
                        {
                            topWall.IsExit();
                        }
                    }

                    if (cell.HasFlag(WallState.LEFT))
                    {
                        var leftWall = Instantiate(verticalWallPrefab, transform);
                        leftWall.transform.position = position + new Vector2(i * boundSize.x, 0);
                    }

                    if (i == width - 1 && cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(verticalWallPrefab, transform);
                        rightWall.transform.position = position + new Vector2((i + 1) * boundSize.x + boundSize.x, 0);
                    }

                    if (j == 0 && cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(downWallPrefab, transform);
                        bottomWall.transform.position = position + new Vector2(boundSize.x + i * boundSize.x,
                            -boundSize.y / 2 + horizontalWallPrefab.size.y / 2);
                        if (!cell.HasFlag(WallState.LEFT))
                        {
                            var rightBottomWall = Instantiate(downWallPrefab, transform);
                            rightBottomWall.transform.position = position + new Vector2(i * boundSize.x,
                                -boundSize.y / 2 + horizontalWallPrefab.size.y / 2);
                        }

                        if (i == 0)
                            bottomWall.IsEntrance();
                    }

                    yield return null;
                }
            }
        }
    }
}