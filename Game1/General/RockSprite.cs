using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Logic
{
    class RockSprite : Sprite
    {
        Vector2 force = Vector2.Zero;
        Vector2 fallForce = Vector2.Zero;
        Vector2 acceleration = Vector2.Zero;
        Vector2 velocity = Vector2.Zero;
        Physics.Box oldBoxPos = new Physics.Box();
        bool haveDirection = false;

        public RockSprite(Texture2D textureImage, Vector2 position, Point frameSize, float speed, Point sheetSize) 
            : base(textureImage, position, frameSize, speed, sheetSize)
        {
            velocity = speed * Direction;
        }

        public override Vector2 Direction
        {
            get
            {
                // We want to calculate direction just once
                if (!haveDirection)
                {
                    MouseState mouseState = Mouse.GetState();
                    Vector2 mousePosition = new Vector2(mouseState.Position.X, mouseState.Position.Y);
                    Vector2 directionToMouse = new Vector2(mousePosition.X - position.X,mousePosition.Y - position.Y);
                    directionToMouse.Normalize();
                    haveDirection = true;

                    return directionToMouse;
                }
                else
                {
                    return new Vector2(1, 1); // dont change any value
                }
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
            //  TODO: add friction if collide with terrain

            // update old collisionBox position
            oldBoxPos.UpdateCollisionBox(
                new Vector2(position.X, position.Y), // player top left vertex (AABB max)
                new Vector2(position.X + frameSize.X, position.Y + frameSize.Y)); // player bottom right vertex (AABB min)

            // update position and velocity
            position += new Vector2(velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds, // x = Vx * time
                velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds +  // y = Vy*time + (gravity * time^2) / 2
                ((Physics.World.Gravity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds * (float)gameTime.ElapsedGameTime.TotalSeconds)/ 2));
            velocity = new Vector2(velocity.X, velocity.Y + (Physics.World.Gravity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds));

            // update collisionBox 
            CollisionBox.UpdateCollisionBox(
                new Vector2(position.X, position.Y), // player top left vertex (AABB max)
                new Vector2(position.X + frameSize.X, position.Y + frameSize.Y)); // player bottom right vertex (AABB min)

            base.Update(gameTime, clientBounds);
        }

        // AABB test
        public bool IsOverlaping(Sprite spriteToTest)
        {
            // Monogame uses different coordinates system so AABB test will be slightly different than usual
            if (CollisionBox.GetAABBmax().X > spriteToTest.CollisionBox.GetAABBmin().X || CollisionBox.GetAABBmin().X < spriteToTest.CollisionBox.GetAABBmax().X)
                return false;

            if (CollisionBox.GetAABBmax().Y > spriteToTest.CollisionBox.GetAABBmin().Y || CollisionBox.GetAABBmin().Y < spriteToTest.CollisionBox.GetAABBmax().Y)
                return false;
            return true;
        }

        public void ResolveCollision(Sprite collidingSprite)
        {
            // colliding with top side
            //if (oldBoxPos.GetAABBmin().Y < collidingSprite.CollisionBox.GetAABBmax().Y && CollisionBox.GetAABBmin().Y >= collidingSprite.CollisionBox.GetAABBmax().Y)
            //{
            //    position.Y = collidingSprite.position.Y - GetFrameSize.Y;
            //    return;
            //}
            // colliding with top side
            if (CollisionBox.GetAABBmin().Y > collidingSprite.CollisionBox.GetAABBmax().Y && CollisionBox.GetAABBmax().Y < collidingSprite.CollisionBox.GetAABBmax().Y)
            {
                position.Y = collidingSprite.position.Y - GetFrameSize.Y;
                return;
            }
            //  colliding with down side
            if (CollisionBox.GetAABBmax().Y < collidingSprite.CollisionBox.GetAABBmin().Y && CollisionBox.GetAABBmin().Y > collidingSprite.CollisionBox.GetAABBmax().Y)
            {
                position.Y = collidingSprite.position.Y + GetFrameSize.Y;
                return;
            }
            //  colliding with right side
            if (CollisionBox.GetAABBmin().X > collidingSprite.CollisionBox.GetAABBmax().X && CollisionBox.GetAABBmin().X < collidingSprite.CollisionBox.GetAABBmin().X)
            {
                velocity.X = 0;
                return;
            }
            //  colliding with left side
            if (CollisionBox.GetAABBmax().X < collidingSprite.CollisionBox.GetAABBmin().X && CollisionBox.GetAABBmax().X > collidingSprite.CollisionBox.GetAABBmax().X)
            {
                position.X = collidingSprite.position.X + collidingSprite.GetFrameSize.X;
                return;
            }
        }
    }
}
