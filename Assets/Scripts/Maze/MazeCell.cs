using UnityEngine;

namespace ProceduralMazeGeneration
{
    public class MazeCell : MonoBehaviour
    {
        [SerializeField] private GameObject leftWall;
        [SerializeField] private GameObject rightWall;
        [SerializeField] private GameObject topWall;
        [SerializeField] private GameObject bottomWall;
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

        public void ClearLeftWall()
        {
            leftWall.SetActive(false);
        }

        public void ClearRightWall()
        {
            rightWall.SetActive(false);
        }

        public void ClearTopWall()
        {
            topWall.SetActive(false);
        }

        public void ClearBottomWall()
        {
            bottomWall.SetActive(false);
        }

        public void Reset()
        {
            IsVisited = false;
            
            leftWall.SetActive(true);
            rightWall.SetActive(true);
            topWall.SetActive(true);
            bottomWall.SetActive(true);
            unvisitedWall.SetActive(true);
        }
    }
}