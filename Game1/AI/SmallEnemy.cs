using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.AI

{
    class SmallEnemy : Enemies
    {
        public SmallEnemy()
        {

        }

        public override void Update(GameTime gameTime, Rectangle clientBounds, Vector2 spritePosition)
        {
            speed = 1;

            // set start point to current position
            Point startPoint = new Point((int)position.X, (int)position.Y);
            // set finish point to player position
            Point finishPoint = new Point((int)PlayerSprite.playerPosition.X, (int)PlayerSprite.playerPosition.Y);
            // generate walkable grid
            AI.Grid grid = new AI.Grid(40, 30, WorldArrays.walkableArray);
            //generate list of nodes to finish node
            List<Point> pathList = AI.PathFinding.FindPath(grid, grid.ConvertPointToGrid(startPoint, 20, 16), grid.ConvertPointToGrid(finishPoint, 20, 16));

            // move to first node in the list
            if (pathList.Count != 0)
            {
                Vector2 convFromGrid = new Vector2(grid.ConvertGridToPoint(new Point(pathList[0].X, pathList[0].Y), 20, 16).X, 
                    grid.ConvertGridToPoint(new Point(pathList[0].X, pathList[0].Y), 20, 16).Y);
                position += speed * Direction(convFromGrid);
            }
            
            base.Update(gameTime, clientBounds, spritePosition);
        }

        public Vector2 Direction(Vector2 nodeToMoveTo)
        {
                // next point is up
                if (position.Y > nodeToMoveTo.Y && position.X == nodeToMoveTo.X)
                {
                    return new Vector2(0, -1);
                }
                // next point is down
                if (position.Y < nodeToMoveTo.Y && position.X == nodeToMoveTo.X)
                {
                    return new Vector2(0, 1);
                }
                // next point is left
                if (position.Y == nodeToMoveTo.Y && position.X > nodeToMoveTo.X)
                {
                    return new Vector2(-1, 0);
                }
                // next point is right
                if (position.Y == nodeToMoveTo.Y && position.X < nodeToMoveTo.X)
                {
                    return new Vector2(1, 0);
                }
                // next point is up-left
                if (position.Y > nodeToMoveTo.Y && position.X > nodeToMoveTo.X)
                {
                    return new Vector2(-1, -1);
                }
                // next point is up-right
                if (position.Y > nodeToMoveTo.Y && position.X < nodeToMoveTo.X)
                {
                    return new Vector2(1, -1);
                }
                // next point is down-left
                if (position.Y < nodeToMoveTo.Y && position.X > nodeToMoveTo.X)
                {
                    return new Vector2(-1, 1);
                }
                // next point is donw-right
                if (position.Y < nodeToMoveTo.Y && position.X < nodeToMoveTo.X)
                {
                    return new Vector2(1, 1);
                }
                return new Vector2(0, 0);            
        }
    }
}