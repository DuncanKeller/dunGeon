using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    public class Flame : Bullet
    {
        protected float lifeTime;
        float dampening = 200;

        public bool Alive
        {
            get { return (lifeTime > 0 && speed > 0); }
        }

        public new Rectangle Rect
        {
            get { return new Rectangle((int)pos.X - 15, (int)pos.Y - 15, 30, 30); }
        }

        public Flame()
        {
            lifeTime = 1.7f + (float)(GameConst.rand.NextDouble() * 0.4) - .2f;
            damage = .015;
            speed = 250 + (float)(GameConst.rand.NextDouble() * 100) - 50; ;
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

            if (lifeTime > 0)
            {
                lifeTime -= deltaTime / 1000;
            }
            else
            {
                lifeTime = 0;
            }

            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.Map["flame"], Rect, new Color(255,20,20,100));
        }
    }
}
