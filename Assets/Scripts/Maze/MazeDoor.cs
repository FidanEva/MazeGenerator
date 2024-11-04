using UnityEngine;

namespace ProceduralMazeGeneration
{
    public class MazeDoor : MonoBehaviour
    {
        public Door door;
        [SerializeField] private GameObject doorGO;

        public void Init()
        {
            doorGO.SetActive(false);
        }
    }

    public enum Door
    {
        Entrance,
        Exit
    }
}