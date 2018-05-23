using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class DynamicSprite : Sprite
    {
        Vector2 jumpForce = new Vector2(0, -500f);
        Vector2 startPos;
        Vector2 endPos;
        public Vector2 Velocity = new Vector2();
        bool goToEnd = true;

        // non animated sprite
        public DynamicSprite(Texture2D textureImage, Vector2 position, Point frameSize, float speed, Vector2 endPos)
            :base(textureImage,position,frameSize,speed)
        {
            this.startPos = position;
            this.endPos = endPos;
        }

        // animated sprite
        public DynamicSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float speed)
            :base(textureImage,position,frameSize,currentFrame,sheetSize,speed)
        {

        }

        public DynamicSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float speed, int millisecondsPerFrame)
            :base(textureImage,position,frameSize,currentFrame,sheetSize,speed,millisecondsPerFrame)
        {

        }

        public override Vector2 Direction
        {
            get
            {
                Vector2 direction = Vector2.Zero;

                if (goToEnd)
                {
                    direction = endPos - startPos;
                    direction.Normalize();
                    if(position.X > endPos.X - 5 && position.X < endPos.X + 5 && position.Y < endPos.Y + 5 && position.Y > endPos.Y - 5)
                        goToEnd = false;
                }
                else
                {
                    direction = startPos - endPos;
                    direction.Normalize();
                    if((position.X > startPos.X - 5 && position.X < startPos.X + 5 && position.Y < startPos.Y + 5 && position.Y > startPos.Y - 5))
                        goToEnd = true;
                }

                return direction;
            }
        }

        public override Point GetFrameSize
        {
            get
            {
                return frameSize;
            }
        }


        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            //move logic
            position += speed * Direction;                                                       // update position


            // Update collision box to current position
            CollisionBox.UpdateCollisionBox(
                new Vector2(position.X, position.Y), // top left vertex (AABB max)
                new Vector2(position.X + frameSize.X, position.Y + frameSize.Y)); // bottom right vertex (AABB min)

            base.Update(gameTime, clientBounds);
        }       
    }
}
