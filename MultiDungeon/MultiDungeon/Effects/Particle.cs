using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Effects
{
    abstract class Particle
    {
        protected Vector2 pos;
        protected Texture2D texture;
        protected Vector2 size;
        protected Color color;
        protected float angle;
        protected float speed;
        protected float rotation;
        protected float rotSpeed;

        public Particle(Vector2 pos)
        {
            this.pos = pos;
        }

        public void Update(float deltaTime)
        {
            pos.X += (float)Math.Cos(angle) * speed * (deltaTime / 1000);
            pos.Y += (float)Math.Sin(angle) * speed * (deltaTime / 1000);
            rotation += rotSpeed * (deltaTime / 1000);
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size.X / 2)), (int)(pos.Y - (size.Y / 2)),
                (int)size.X, (int)size.Y);
            rect.X += texture.Width;
            rect.Y += texture.Height;
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            sb.Draw(texture, rect, source, color, rotation,
                new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
