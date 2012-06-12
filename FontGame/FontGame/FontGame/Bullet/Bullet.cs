using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace FontGame
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public abstract class Bullet : DrawableGameComponent
    {
        public Bullet(Game game) : base(game)
        {
            
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            const float maxSpeed = 0.2f;
            // TODO: Add your update code here
            if (xMove == 0 || yMove == 0)
            {
                xMove = (Heading.X - Position.X)/1000;
                yMove = (Heading.Y - Position.Y)/1000;

                float max = Math.Max(Math.Abs(xMove), Math.Abs(yMove));

                if (max == Math.Abs(xMove))
                {
                    if(xMove >= 0)
                    {
                        yMove = yMove * Math.Abs(maxSpeed / xMove);
                        xMove = maxSpeed;                        
                    }
                    else
                    {
                        yMove = yMove * Math.Abs(maxSpeed / xMove);
                        xMove = maxSpeed*-1;
                    }
                }
                else
                {
                    if(yMove >= 0)
                    {
                        xMove = xMove * Math.Abs(maxSpeed / yMove);
                        yMove = maxSpeed;                        
                    }
                    else
                    {
                        xMove = xMove * Math.Abs(maxSpeed / yMove);
                        yMove = maxSpeed*-1;
                    }
                }
            }

            Position.X += gameTime.ElapsedGameTime.Milliseconds * xMove;
            Position.Y += gameTime.ElapsedGameTime.Milliseconds * yMove;

            base.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Texture, Position, Color.White);
            spriteBatch.Draw(Texture, Position, null, Color.White, RotationAngle, Origin, 1f, SpriteEffects.None, 0f);


            base.Draw(gameTime);
        }




        private float xMove = 0;
        private float yMove = 0;

        public float RotationAngle;

        public Vector2 Position;
        public Vector2 Heading;
        protected Vector2 Origin;


        public Texture2D Texture;
    }
}
