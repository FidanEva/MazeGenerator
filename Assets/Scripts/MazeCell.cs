using UnityEngine;

namespace Labrynth
{
    public class MazeCell : MonoBehaviour
    {
        [SerializeField] private  Collider2D wallCollider;
        [SerializeField] private SpriteRenderer wallImage;
        
        public Vector2 size;

        public void IsEntrance()
        {
            wallImage.enabled = false;
        }

        public void IsExit()
        {
            wallImage.enabled = false;
            wallCollider.isTrigger = true;
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}