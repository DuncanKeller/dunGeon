using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Effects
{
    public class SkullParticle : Particle
    {
        float xspeed;
        float yspeed;

        public override bool Alive
        {
            get { return yspeed < 250; }
        }

        public SkullParticle(Vector2 pos)
            : base(pos)
        {
            texture = TextureManager.Map["skull"];
            rotSpeed = -(float)(GameConst.rand.NextDouble() * Math.PI * 8);
            rotSpeed -= (float)Math.PI * 2;
            if (GameConst.rand.Next(1) == 0)
            {
                rotSpeed = (float)(GameConst.rand.NextDouble() * Math.PI * 8);
                rotSpeed += (float)Math.PI * 2;
            }
            //angle = (float)(GameConst.rand.NextDouble() * Math.PI * 2);
            xspeed = -75;
            xspeed += GameConst.rand.Next(150);
            yspeed = -50 - GameConst.rand.Next(175);
            life = 5.75f;
            size = new Vector2(25, 20);
            color = Color.White;
        }


        public override void Update(float deltaTime)
        {
            life -= deltaTime / 1000;
            pos.X += xspeed * (deltaTime / 1000);
            pos.Y += yspeed * (deltaTime / 1000);
            yspeed += 300 * (deltaTime / 1000);

            rotation += rotSpeed * (deltaTime / 1000);

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
