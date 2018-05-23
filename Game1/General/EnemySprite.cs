using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.Logic
{
    class EnemySprite : Sprite
    {
        AI.SmallEnemy smallEnemy;
        // non animated sprite
        public EnemySprite(Texture2D textureImage, Vector2 position, Point frameSize, float speed)
            : base(textureImage, position, frameSize, speed)
        {
            smallEnemy = new AI.SmallEnemy();
        }

        // animated sprite
        public EnemySprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float speed)
            : base(textureImage, position, frameSize, currentFrame, sheetSize, speed)
        {

        }

        public EnemySprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, currentFrame, sheetSize, speed, millisecondsPerFrame)
        {

        }

        public override Vector2 Direction
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Point GetFrameSize
        {
            get
            {
                return frameSize;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            smallEnemy.Update(gameTime,clientBounds,position);
            position = smallEnemy.position;

            CollisionBox.UpdateCollisionBox(
                new Vector2(position.X, position.Y), // player top left vertex (AABB max)
                new Vector2(position.X + frameSize.X, position.Y + frameSize.Y)); // player bottom right vertex (AABB min)
            base.Update(gameTime,clientBounds);
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
            if (CollisionBox.GetAABBmin().Y > collidingSprite.CollisionBox.GetAABBmax().Y && CollisionBox.GetAABBmin().Y < collidingSprite.CollisionBox.GetAABBmin().Y)
            {
                position.Y = collidingSprite.position.Y - GetFrameSize.Y;
                return;
            }
            //  colliding with down side
            if (CollisionBox.GetAABBmax().Y < collidingSprite.CollisionBox.GetAABBmin().Y && CollisionBox.GetAABBmax().Y > collidingSprite.CollisionBox.GetAABBmax().Y)
            {
                position.Y = collidingSprite.position.Y + GetFrameSize.Y;
                return;
            }
            //  colliding with right side
            if (CollisionBox.GetAABBmin().X > collidingSprite.CollisionBox.GetAABBmax().X && CollisionBox.GetAABBmin().X < collidingSprite.CollisionBox.GetAABBmin().X)
            {
                position.X = collidingSprite.position.X - GetFrameSize.X;
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
