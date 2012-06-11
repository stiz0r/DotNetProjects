

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontGame
{
    public class Enemy : DrawableGameComponent
    {


        public Enemy(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
            Random rnd = new Random();

            int posX = rnd.Next(-1000, 1000);
            int posY = rnd.Next(-1000, 1000);

            if (posX >= 0 && posX <= 800)
            {
                posX = -32;
            }

            if (posY >= 0 && posY <= 480)
            {
                posY = -32;
            }

            position = new Vector2(posX, posY);
            target = new Vector2();

            LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        public void LoadContent()
        {
            texture = Game.Content.Load<Texture2D>("Enemies/EnemyCrap");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            if (position.X < target.X - 5)
            {
                position.X += gameTime.ElapsedGameTime.Milliseconds * 0.08f;
            }
            else if (position.X > target.X + 5)
            {
                position.X -= gameTime.ElapsedGameTime.Milliseconds * 0.08f;
            }
            else
            {
                position.X = target.X;
            }

            if (position.Y < target.Y - 5)
            {
                position.Y += gameTime.ElapsedGameTime.Milliseconds * 0.08f;
            }
            else if (position.Y > target.Y + 5)
            {
                position.Y -= gameTime.ElapsedGameTime.Milliseconds * 0.08f;
            }
            else
            {
                position.Y = target.Y;
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);

            base.Draw(gameTime);
        }


        Texture2D texture;
        Vector2 position;
        Vector2 target;

        public Vector2 Target
        {
            get { return target; }
            set { target = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
    }
}