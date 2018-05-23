using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1.AI
{
    class PathFinding
    {
        // Starting function 
        // grid = converted game map to array of nodes
        public static List<Point> FindPath(Grid grid, Point startPoint, Point targetPoint)
        {
            List<Node> pathList = AStarAlgorithm(grid, startPoint, targetPoint);

            // convert into list of points
            List<Point> pointList = new List<Point>();
            if(pathList != null)
            {
                foreach(Node node in pathList)
                {
                    pointList.Add(new Point(node.gridX, node.gridY));
                }
            }

            return pointList;
        }

        // main function
        private static List<Node> AStarAlgorithm(Grid grid, Point startPoint, Point targetPoint)
        {
            Node startNode = grid.nodes[startPoint.X,startPoint.Y];
            Node targetNode = grid.nodes[targetPoint.X,targetPoint.Y];
            Node currentNode;

            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();
            openList.Add(startNode);

            while(openList.Count > 0)
            {
                currentNode = openList[0];

                // Pick node from openList with lowest fCost and set it as current node
                for(int i = 0; i < openList.Count; i++)
                {
                    if(openList[i].fCost < currentNode.fCost)
                    {
                        currentNode = openList[i];
                    }
                }

                // Add current node to closedList and remove it from openList
                closedList.Add(currentNode);
                openList.Remove(currentNode);

                // check if current node is targetNode
                if(currentNode == targetNode)
                {
                    return RetracePath(startNode, targetNode);
                }

                // chceck all neighbours of current node
                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    // if is unwalkabe or is in closed list skip it
                    if(!neighbour.walkable || closedList.Contains(neighbour))
                    {
                        continue;
                    }

                    int newPathCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                    // Add neighbour to open list or if new path cost is lower set new values
                    if(!openList.Contains(neighbour) || newPathCostToNeighbour < neighbour.gCost)
                    {
                        neighbour.gCost = newPathCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if(!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }

            // There is no existing path
            return null;
        }

        private static List<Node> RetracePath(Node StartNode, Node targetNode)
        {
            List<Node> path = new List<Node>();
            Node currentNode = targetNode;

            while(currentNode != StartNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            path.Reverse();

            return path;
        }

        // return cost of all tiles between nodes
        // 1 tile vertically or horizontal cost 10
        // 1 tile diagonally cost 14
        private static int GetDistance(Node nodeA, Node nodeB)
        {
            // calculate difference between nodes
            int dstX = nodeA.gridX - nodeB.gridX;
            int dstY = nodeA.gridY - nodeB.gridY;

            // if value is negative make it positive 
            if (dstX < 0)
            {
                dstX *= -1;
            }

            // if value is negative make it positive 
            if(dstY < 0)
            {
                dstY *= -1;
            }

            if (dstX > dstY)
                return 14 * dstY + 10 * (dstX - dstY); // dstY = vertically tiles, (dstX - dstY) = horizonatl/vertical tiles
            return 14 * dstX + 10 * (dstY - dstX);     // dstX = vertically tiles, (dstY - dstX) = horizonatl/vertical tiles
        }
    }
}
