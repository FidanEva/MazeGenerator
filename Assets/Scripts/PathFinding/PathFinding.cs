using System.Collections.Generic;

namespace PathFinding
{
    public class PathFinding
    {
        private const int MOVE_STRAIHGT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 10;

        private List<PathNode> openList;
        private List<PathNode> closedList;
    }
}