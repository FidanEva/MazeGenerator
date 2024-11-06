using System.Linq;
using UnityEngine;

namespace ProceduralMazeGeneration
{
    public class MazeCell : MonoBehaviour
    {
        #region Maze Generation

        [SerializeField] private MazeWall[] walls = new MazeWall[4];
        [SerializeField] private GameObject unvisitedWall;
        [SerializeField] private MazeDoor[] doors = new MazeDoor[2];

        public bool IsVisited { get; private set; }

        public void Visit()
        {
            unvisitedWall.SetActive(false);
            IsVisited = true;
        }

        public void InitDoor(Door argDoor)
        {
            var door = doors.FirstOrDefault(d => d.door == argDoor);
            if (door != null) door.Init();
        }

        public bool WallIsActive(Side argSide)
        {
            var wall = walls.FirstOrDefault(w => w.side == argSide);
            return wall != null && wall.isActive;
        }

        public void ClearWall(Side argSide)
        {
            var wall = walls.FirstOrDefault(w => w.side == argSide);
            if (wall is not null)
                wall.ClearWall();
        }

        public void Reset()
        {
            IsVisited = false;

            foreach (var mazeWall in walls)
                mazeWall.Reset();

            unvisitedWall.SetActive(true);
        }

        #endregion

        #region A Star

        public int x;
        public int y;

        public int gCost = int.MaxValue;
        public int hCost;
        public int fCost;

        public MazeCell cameFromNode = null;

        public void SetGridIndices(int x, int y)
        {
            this.x = x;
            this.y = y;

            CalculateFCost();
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        #endregion
    }
}