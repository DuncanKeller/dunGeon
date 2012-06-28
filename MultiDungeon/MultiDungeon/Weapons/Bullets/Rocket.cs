using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    public class Rocket : Bullet
    {
        float dx = 800;
        float acc = 1000;
        float maxSpeed = 10000;

        public new Rectangle Rect
        {
            get { return new Rectangle((int)pos.X - 6, (int)pos.Y - 6, 12, 12); }
        }

        public Rocket()
        {
            damage = 5;
            speed = 0;
        }

        public override void Update(float deltaTime)
        {
            if (speed < maxSpeed)
            {
                speed += (deltaTime / 1000) * acc;
                acc += (deltaTime / 1000) * dx;
            }
            else 
            {
                speed = maxSpeed;
            }
            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.Map["blank"], Rect, Color.Black);
        }
    }
}
