using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1.AI
{
    class Node
    {
        public bool walkable; // is node walkable?
        public int gridX;     // node index X
        public int gridY;     // node index Y  
        public float cost;    // node cost

        public int gCost;     // distance from starting node
        public int hCost;     // distance from end node
        public Node parent;

        public Node(float cost, int gridX, int gridY)
        {
            this.gridX = gridX;
            this.gridY = gridY;
            this.cost = cost;
            walkable = cost != 0.0f;
        }

        public Node (bool walkable, int gridX, int gridY)
        {
            this.walkable = walkable;
            this.cost = walkable ? 1f : 0f;
            this.gridX = gridX;
            this.gridY = gridY;
        }

        public int fCost
        {
            get
            {
                return gCost + hCost;
            }
        }
    }
}
