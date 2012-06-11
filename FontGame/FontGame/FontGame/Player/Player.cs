using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace FontGame
{
    class Player : DrawableGameComponent
    {
        public Player(Game game) : base(game)
        {

            // TODO: Construct any child components here

            Position = new Vector2();
            Target = new Vector2();
            Origin = new Vector2();
            
            Health = 100;
        }




        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }


        public new void LoadContent()
        {
            Texture = Game.Content.Load<Texture2D>("Timmy/Timmy");

            Viewport viewport = GraphicsDevice.Viewport;

            Position.X = viewport.Width / 2;
            Position.Y = viewport.Height / 2;

            Origin.X = Texture.Width / 2;
            Origin.Y = Texture.Height / 2;

            base.LoadContent();
        }




        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {

            //if (Position.X < target.X - 5)
            //{
            //    Position.X += gameTime.ElapsedGameTime.Milliseconds * 0.3f;
            //}
            //else if (Position.X > target.X + 5)
            //{
            //    Position.X -= gameTime.ElapsedGameTime.Milliseconds * 0.3f;
            //}
            //else
            //{
            //    Position.X = target.X;
            //}

            //if (Position.Y < target.Y - 5)
            //{
            //    Position.Y += gameTime.ElapsedGameTime.Milliseconds * 0.3f;
            //}
            //else if (Position.Y > target.Y + 5)
            //{
            //    Position.Y -= gameTime.ElapsedGameTime.Milliseconds * 0.3f;
            //}
            //else
            //{
            //    Position.Y = target.Y;
            //}



            if (Position.Y >= 480 - Texture.Width)
            {
                Position.Y = 480 - 64;
            }

            if (Position.X >= 800 - 64)
            {
                Position.X = 800 - 64;
            }


            // The time since Update was called last.
            //float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Process touch events
            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.State != TouchLocationState.Pressed)
                    continue;

                float radians = (float) Math.Atan2(tl.Position.Y - Position.Y, tl.Position.X - Position.X);
                radians = radians - MathHelper.Pi/2;
                Debug.WriteLine(radians);
                RotationAngle = radians;

                (Game as Game1).AddBullet(Position, tl.Position, RotationAngle);
            }


            base.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, RotationAngle, Origin, 1f, SpriteEffects.None, 0f);



            base.Draw(gameTime);
        }



        #region Properties

        public Vector2 Position;
        private Vector2 Target;
        private Vector2 Origin;
        private float RotationAngle;

        private Texture2D Texture;

        private int Health { get; set; }

        #endregion
    }
}
