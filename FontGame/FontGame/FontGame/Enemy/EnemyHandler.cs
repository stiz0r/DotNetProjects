using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontGame
{
    public class EnemyHandler : DrawableGameComponent
    {
        List<Enemy> enemies = new List<Enemy>();
        



        public EnemyHandler(Game game) : base(game)
        {
            // TODO: Construct any child components here
        }

        public int NumberOfEnemies()
        {
            return enemies.Count;
        }

        public void AddEnemy()
        {
            Enemy enemy = new Enemy(Game);
            enemy.Initialize();
            enemy.LoadContent();
            enemies.Add(enemy);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            enemies.Remove(enemy);
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

        public void Clear()
        {
            enemies.Clear();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime, Vector2 target)
        {
            if(enemies.Count < 4)
                AddEnemy();

            // TODO: Add your update code here
            //Rectangle playerRect = new Rectangle((int)target.X, (int)target.Y, 64, 64);

            foreach (Enemy e in enemies)
            {
                e.Target = target;
                e.Update(gameTime);
                //Rectangle enemyRect = new Rectangle((int)e.Position.X + 7, (int)e.Position.Y + 7, 32 - 7, 32 - 7);

                //if (playerRect.Intersects(enemyRect))
                //{
                //    gameOver = true;
                //}
            }

            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Enemy e in enemies)
            {
                e.Draw(gameTime, spriteBatch);
            }

            base.Draw(gameTime);
        }



        public List<KeyValuePair<Enemy, Rectangle>> GetRectangles()
        {
            List<KeyValuePair<Enemy, Rectangle>> rectangles = new List<KeyValuePair<Enemy, Rectangle>>();
            foreach (Enemy enemy in enemies)
            {
                rectangles.Add(new KeyValuePair<Enemy, Rectangle>(enemy, new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, enemy.Texture.Width, enemy.Texture.Height)));
            }
            return rectangles;
        }
    }

}
