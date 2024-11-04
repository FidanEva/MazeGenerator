using UnityEngine;

namespace ProceduralMazeGeneration
{
    public class MazeWall : MonoBehaviour
    {
        public Side side;
        public bool isAqctive = true;
        [SerializeField] private GameObject wall;

        public void ClearWall()
        {
            isAqctive = false;
            wall.SetActive(false);
        }

        public void Reset()
        {
            isAqctive = true;
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