using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Game1.AI
{
    abstract class Enemies
    {
        public Logic.EnemySprite enemySprite;
        public Vector2 position;
        public int speed = 0;
        int life;

        public Enemies()
        {

        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds, Vector2 spritePosition)
        {
            
        }
    }
}
