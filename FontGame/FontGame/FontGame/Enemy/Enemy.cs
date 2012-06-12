

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
            Texture = Game.Content.Load<Texture2D>("Enemies/EnemyCrap");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (xMove == 0 || yMove == 0)
            {
                xMove = (Target.X - Position.X) / 1000;
                yMove = (Target.Y - Position.Y) / 1000;

                float max = Math.Max(Math.Abs(xMove), Math.Abs(yMove));

                if (max == Math.Abs(xMove))
                {
                    if (xMove >= 0)
                    {
                        yMove = yMove * Math.Abs(MaxSpeed / xMove);
                        xMove = MaxSpeed;
                    }
                    else
                    {
                        yMove = yMove * Math.Abs(MaxSpeed / xMove);
                        xMove = MaxSpeed * -1;
                    }
                }
                else
                {
                    if (yMove >= 0)
                    {
                        xMove = xMove * Math.Abs(MaxSpeed / yMove);
                        yMove = MaxSpeed;
                    }
                    else
                    {
                        xMove = xMove * Math.Abs(MaxSpeed / yMove);
                        yMove = MaxSpeed * -1;
                    }
                }
            }

            position.X += gameTime.ElapsedGameTime.Milliseconds * xMove;
            position.Y += gameTime.ElapsedGameTime.Milliseconds * yMove;

            base.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, Color.White);

            base.Draw(gameTime);
        }


        private float xMove = 0;
        private float yMove = 0;
        private const float _max_speed = 0.08f;
        protected virtual float MaxSpeed
        {
            get { return _max_speed; }
        }

        public Texture2D Texture;
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

        public int Damage = 10;
    }
}