using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FontGame
{
    public class GunBullet : Bullet
    {
        public GunBullet(Game game) : base(game)
        {
            
        }


        public override void Initialize()
        {
            Texture = Game.Content.Load<Texture2D>("Bullets/GunBullet");

            Origin.X = Texture.Width / 2f;
            Origin.Y = Texture.Height / 2f;

            base.Initialize();

        }
    }
}
