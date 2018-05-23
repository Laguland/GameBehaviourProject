using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Game1.Physics;

namespace Game1
{
    class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        PlayerSprite player;
        StaticSprite windUp;
        Logic.EnemySprite enemySprite;
        List<Sprite> dynamicSpriteList;
        List<Sprite> staticSpriteList;
        List<Sprite> rockList;
        MouseState mouseState;


        public SpriteManager(Game game) : base(game)
        {

        }

        public override void Initialize()
        {
            mouseState = new MouseState();
            AI.WorldArrays.GenerateMapArray();
            base.Initialize();
        }

        // Load all sprites here
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            staticSpriteList = new List<Sprite>();
            dynamicSpriteList = new List<Sprite>();
            rockList = new List<Sprite>();

            //player sprite
            player = new PlayerSprite(Game.Content.Load<Texture2D>("graphic/stand_r"), Vector2.Zero, new Point(15, 32),
                new Point(0, 0), new Point(0, 0), 6);

            // enemies sprite
            enemySprite = new Logic.EnemySprite(Game.Content.Load<Texture2D>("graphic/Enemy-right1"), Vector2.Zero, new Point(20, 16), 0);

            // dynamic enviroment

            DynamicSprite dSprite = new DynamicSprite(Game.Content.Load<Texture2D>("graphic/floor"), new Vector2(100, 80), new Point(20, 16), 1, new Vector2(200,160));
            dynamicSpriteList.Add(dSprite);
            DynamicSprite dSprite2 = new DynamicSprite(Game.Content.Load<Texture2D>("graphic/floor"), new Vector2(500, 280), new Point(20, 16), 1, new Vector2(500, 400));
            dynamicSpriteList.Add(dSprite2);

            // Wind

            windUp = new StaticSprite(Game.Content.Load<Texture2D>("graphic/Pipe"), new Vector2(600, 0), new Point(200, 480));

            // static enviroment
            for(int x = 0; x< 40; x++)
            {
                for(int y = 0; y< 30; y++)
                {
                    if(AI.WorldArrays.mapArray[x,y] == 1.0f)
                    {
                        StaticSprite normalGroundTile = new StaticSprite(Game.Content.Load<Texture2D>("graphic/floor"), new Vector2(x*20, y*16), new Point(20, 16));
                        staticSpriteList.Add(normalGroundTile);
                    }
                }
            }

            base.LoadContent();
        }

        // Main game loop
        public override void Update(GameTime gameTime)
        {
            // throw rock on left mouse button release
            mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                // create rock to throw
                Logic.RockSprite rock = new Logic.RockSprite(Game.Content.Load<Texture2D>("graphic/dirtfull"), player.position + new Vector2(0,20), new Point(5, 5), 100f, new Point(1, 0));
                rockList.Add(rock);
            }

            // update player and enemy
            player.Update(gameTime, Game.Window.ClientBounds);
            enemySprite.Update(gameTime, Game.Window.ClientBounds);

            //update moving platforms
            foreach (Sprite s in dynamicSpriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);
            }

            // update rocks
            foreach (Logic.RockSprite s in rockList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);
            }

            // check player and rock collisions with static sprites
            foreach (Sprite s in staticSpriteList)
            {
                if (player.IsOverlaping(s))
                {
                    player.ResolveCollision(s);
                }

                foreach (Logic.RockSprite d in rockList)
                {
                    if (d.IsOverlaping(s))
                        d.ResolveCollision(s);
                }
            }

            // check player and rock collisions with dynamic sprites
            foreach (Sprite s in dynamicSpriteList)
            {
                if (player.IsOverlaping(s))
                {
                    player.ResolveCollision(s);
                }

                foreach (Logic.RockSprite d in rockList)
                {
                    if (d.IsOverlaping(s))
                        d.ResolveCollision(s);
                }
            }

            // wind logic
            if(player.IsOverlaping(windUp))
            {
                player.position += new Vector2(0, -10);
            }

            base.Update(gameTime);
        }

        // Draw all sprites
        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            windUp.Draw(gameTime, spriteBatch);

            // Draw player
            player.Draw(gameTime, spriteBatch);

            // Draw Enemy
            enemySprite.Draw(gameTime, spriteBatch);

            // Draw static sprites
            foreach (Sprite s in staticSpriteList)
            {
                s.Draw(gameTime, spriteBatch);
            }

            // draw dynamic sprites
            foreach (Sprite s in dynamicSpriteList)
            {
                s.Draw(gameTime, spriteBatch);
            }

            // draw rocks
            foreach(Logic.RockSprite s in rockList)
            {
                s.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
