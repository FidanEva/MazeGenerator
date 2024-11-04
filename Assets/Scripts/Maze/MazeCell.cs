using System.Linq;
using UnityEngine;

namespace ProceduralMazeGeneration
{
    public class MazeCell : MonoBehaviour
    {
        [SerializeField] private MazeWall[] walls = new MazeWall[4];

        [SerializeField] private GameObject unvisitedWall;

        [Space] [SerializeField] private GameObject entranceDoor;
        [SerializeField] private GameObject exitDoor;

        public bool IsVisited { get; private set; }

        public void Visit()
        {
            unvisitedWall.SetActive(false);
            IsVisited = true;
        }

        public void IsEntrance()
        {
            entranceDoor.SetActive(false);
        }

        public void IsExit()
        {
            exitDoor.SetActive(false);
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
    }
}