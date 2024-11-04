namespace PathFinding
{
    public class PathNode
    {
        public int x;
        public int y;
        
        public int gCost;
        public int hCost;
        public int fCost;

        public PathNode cameFromNode;

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
    }
}