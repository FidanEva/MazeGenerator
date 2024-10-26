using System;
using System.Collections.Generic;

namespace Labrynth
{
    [Flags]
    public enum WallState
    {
        LEFT = 1,
        RIGHT = 2,
        UP = 4,
        DOWN = 8,
        VISITED = 128,
    }

    public struct Position
    {
        public int X, Y;
    }

    public struct Neighbour
    {
        public Position Position;
        public WallState SharedWall;
    }

    public static class MazeGenerator
    {
        public static WallState[,] Generate(int width, int height)
        {
            WallState[,] maze = InitializeMaze(width, height);
            return ApplyRecursiveBackTracker(maze, width, height);
        }

        private static WallState[,] InitializeMaze(int width, int height)
        {
            var maze = new WallState[width, height];
            const WallState initial = WallState.LEFT | WallState.RIGHT | WallState.UP | WallState.DOWN;
            for (var i = 0; i < width; i++)
            for (var j = 0; j < height; j++)
                maze[i, j] = initial;
            return maze;
        }

        private static WallState GetOppositeWall(WallState wall)
        {
            return wall switch
            {
                WallState.RIGHT => WallState.LEFT,
                WallState.LEFT => WallState.RIGHT,
                WallState.UP => WallState.DOWN,
                WallState.DOWN => WallState.UP,
                _ => WallState.LEFT,
            };
        }

        private static WallState[,] ApplyRecursiveBackTracker(WallState[,] maze, int width, int height)
        {
            var rng = new System.Random();
            var stack = new Stack<Position>();
            var position = new Position { X = rng.Next(width), Y = rng.Next(height) };

            maze[position.X, position.Y] |= WallState.VISITED;
            stack.Push(position);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

                if (neighbours.Count > 0)
                {
                    stack.Push(current);
                    var randomNeighbour = neighbours[rng.Next(neighbours.Count)];
                    maze[current.X, current.Y] &= ~randomNeighbour.SharedWall;
                    maze[randomNeighbour.Position.X, randomNeighbour.Position.Y] &=
                        ~GetOppositeWall(randomNeighbour.SharedWall);
                    maze[randomNeighbour.Position.X, randomNeighbour.Position.Y] |= WallState.VISITED;
                    stack.Push(randomNeighbour.Position);
                }
            }

            return maze;
        }

        private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, int width, int height)
        {
            var neighbours = new List<Neighbour>();

            if (p.X > 0) AddNeighbour(p.X - 1, p.Y, WallState.LEFT);
            if (p.X < width - 1) AddNeighbour(p.X + 1, p.Y, WallState.RIGHT);
            if (p.Y > 0) AddNeighbour(p.X, p.Y - 1, WallState.DOWN);
            if (p.Y < height - 1) AddNeighbour(p.X, p.Y + 1, WallState.UP);

            return neighbours;

            void AddNeighbour(int x, int y, WallState sharedWall)
            {
                if (!maze[x, y].HasFlag(WallState.VISITED))
                    neighbours.Add(new Neighbour { Position = new Position { X = x, Y = y }, SharedWall = sharedWall });
            }
        }
    }
}