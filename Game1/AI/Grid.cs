using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1.AI
{
    class Grid
    {
        public Node[,] nodes;
        int gridSizeX;
        int gridSizeY;

        // New grid with tile cost
        // Cost: 0.0f unwalkable
        //       1.0f normal tile
        //       > 1.0f costly tile
        //       < 1.0f cheap tile
        public Grid(int width, int height, float[,] tilesCost)
        {
            this.gridSizeX = width;
            this.gridSizeY = height;
            nodes = new Node[width,height];

            // fill nodes with cost
            for(int x = 0; x < width; x++)
            {
                for(int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(tilesCost[x, y], x, y);
                }
            }
        }

        // New grid without tile cost
        // Cost is set to 1 if walkable and 0 if not
        public Grid(int width, int height, bool[,] walkableTiles)
        {
            gridSizeX = width;
            gridSizeY = height;
            nodes = new Node[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    nodes[x, y] = new Node(walkableTiles[x, y] ? 1.0f : 0.0f, x, y);
                }
            }
        }
        
        // return list of neighbours of given node
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> neighbours = new List<Node>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                        continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                    {
                        neighbours.Add(nodes[checkX, checkY]);
                    }
                }
            }

            return neighbours;
        }

        // convert point on the screen to point in the grid
        public Point ConvertPointToGrid(Point point, int gridWidht, int gridHeight)
        {
            Point convPoint;

            convPoint.X = (int)point.X / gridWidht;
            convPoint.Y = (int)point.Y / gridHeight;

            if (convPoint.X < 0)
                convPoint.X = 0;
            if (convPoint.X > 39)
                convPoint.X = 39;
            if (convPoint.Y < 0)
                convPoint.Y = 0;
            if (convPoint.Y > 29)
                convPoint.Y = 29;

            return convPoint;
        }

        // convert point in grid to point on the screen
        public Point ConvertGridToPoint(Point point, int gridWidht, int gridHeight)
        {
            Point convPoint;

            convPoint.X = point.X * gridWidht;
            convPoint.Y = point.Y * gridHeight;

            return convPoint;
        }
    }
}
