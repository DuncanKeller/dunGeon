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
            get { return new Rectangle((int)pos.X - 5, (int)pos.Y - 13, 10, 26); }
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
            Texture2D texture = TextureManager.Map["rocket"];
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            sb.Draw(texture, Rect, source, Color.Gray, angle + (float)(Math.PI / 2),
                new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
