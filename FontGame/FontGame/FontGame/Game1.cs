using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

namespace FontGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player Player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Player = new Player(this);
            BulletHandler = new BulletHandler(this);
            EnemyHandler = new EnemyHandler(this);

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Player.Initialize();
            BulletHandler.Initialize();
            EnemyHandler.Initialize();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            EnemiesKilledFont = Content.Load<SpriteFont>("Fonts/EnemiesKilled");
            Background = Content.Load<Texture2D>("Backgrounds/Background1");
            Player.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // TODO: Add your update logic here
            Player.Update(gameTime);
            BulletHandler.Update(gameTime);
            EnemyHandler.Update(gameTime, Player.Position);

            Rectangle playerRectangle = Player.GetRectangle();
            foreach (KeyValuePair<Enemy, Rectangle> enemy in EnemyHandler.GetRectangles())
            {
                if(playerRectangle.Intersects(enemy.Value))
                {
                    Player.TakeDamage(enemy.Key.Damage);
                    EnemyHandler.RemoveEnemy(enemy.Key);
                    continue;
                }

                foreach (KeyValuePair<Bullet, Rectangle> bullet in BulletHandler.GetRectangles())
                {
                    if (!bullet.Value.Intersects(enemy.Value)) 
                        continue;

                    enemy.Key.TakeDamage(bullet.Key.GetDamage());
                    BulletHandler.RemoveBullet(bullet.Key);

                    if (!enemy.Key.IsDead()) 
                        continue;

                    EnemyHandler.RemoveEnemy(enemy.Key);
                    EnemiesKilled++;
                    break;
                }
            }


            if (Player.IsDead())
            {
                // Do something
            }



            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(Background, new Vector2(0, 0), Color.White);

            Player.Draw(gameTime, spriteBatch);
            BulletHandler.Draw(gameTime, spriteBatch);
            EnemyHandler.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(EnemiesKilledFont, "Drepte: " + EnemiesKilled, new Vector2(0, 420), Color.White);
            spriteBatch.DrawString(EnemiesKilledFont, "Helse: " + Player.GetHealth(), new Vector2(0, 440), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void AddBullet(Vector2 position, Vector2 heading, float rotationAngle)
        {
            BulletHandler.AddBullet(position, heading, rotationAngle);
        }



        private Texture2D Background;

        private BulletHandler BulletHandler;
        private EnemyHandler EnemyHandler;

        private SpriteFont EnemiesKilledFont;
        private int EnemiesKilled;
    }
}
