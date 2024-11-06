using UnityEngine;

namespace ProceduralMazeGeneration
{
    public class MazeWall : MonoBehaviour
    {
        public Side side;
        public bool isActive = true;
        [SerializeField] private GameObject wall;

        public void ClearWall()
        {
            isActive = false;
            wall.SetActive(false);
        }

        public void Reset()
        {
            isActive = true;
            wall.SetActive(true);
        }
    }

    public enum Side
    {
        Left,
        Right,
        Top,
        Bottom
    }
}