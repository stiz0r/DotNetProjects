using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontGame
{
    public class BulletHandler : DrawableGameComponent
    {
        public BulletHandler(Game game) : base(game)
        {



        }


        public void AddBullet(Vector2 position, Vector2 heading, float rotationAngle)
        {

            GunBullet bullet = new GunBullet(Game);
            bullet.Initialize();
            bullet.Position = new Vector2(position.X, position.Y);
            bullet.Heading = heading;
            bullet.RotationAngle = rotationAngle;

            Bullets.Add(bullet);
        }

        public void RemoveBullet(Bullet bullet)
        {
            Bullets.Remove(bullet);
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
        public void Update(GameTime gameTime)
        {
            UpdateBullets(gameTime);
            CleanUpBullets();

            base.Update(gameTime);
        }

        private void UpdateBullets(GameTime gameTime)
        {
            foreach (Bullet bullet in Bullets)
            {
                bullet.Update(gameTime);

                //if (playerRect.Intersects(enemyRect))
                //{
                //    gameOver = true;
                //}
            }
        }

        private void CleanUpBullets()
        {
            List<Bullet> removeBullets = new List<Bullet>();
            foreach (Bullet bullet in Bullets)
            {
                if (Math.Abs(bullet.Position.X) > 2000 && Math.Abs(bullet.Position.Y) > 2000)
                    removeBullets.Add(bullet);
            }

            // Clean up of bullets out of reach
            removeBullets.ForEach(bullet => Bullets.Remove(bullet));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            foreach (Bullet bullet in Bullets)
            {
                bullet.Draw(gameTime, spriteBatch);
            }

            base.Draw(gameTime);
        }


        public List<KeyValuePair<Bullet, Rectangle>> GetRectangles()
        {
            List<KeyValuePair<Bullet, Rectangle>> rectangles = new List<KeyValuePair<Bullet, Rectangle>>();
            foreach (Bullet bullet in Bullets)
            {
                rectangles.Add(new KeyValuePair<Bullet, Rectangle>(bullet, new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, bullet.Texture.Width, bullet.Texture.Height)));
            }
            return rectangles;
        }

        private List<Bullet> Bullets = new List<Bullet>();
    }
}
