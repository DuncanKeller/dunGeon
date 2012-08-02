using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    public class Grenade : Bullet
    {
        protected float explosionTimer = 2;
        float dampening = 770;

        public bool Exploded
        {
            get { return explosionTimer == 0; }
        }

        public new Rectangle Rect
        {
            get { return new Rectangle((int)pos.X - 6, (int)pos.Y - 6, 12, 12); }
        }

        public Grenade()
        {
            damage = 5;
            speed = 700;
        }

        public override void Update(float deltaTime)
        {
            if (speed > 0)
            {
                speed -= (deltaTime / 1000) * dampening;
            }
            else
            {
                speed = 0;
            }

            if (explosionTimer > 0)
            {
                explosionTimer -= deltaTime / 1000;
            }
            else
            {
                explosionTimer = 0;
            }

            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            Texture2D texture = TextureManager.Map["grenade"];
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            sb.Draw(texture, Rect, source, Color.White, angle + (float)(Math.PI / 2),
                new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
