using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Effects
{
    public class HealthParticle : Particle
    {
        public HealthParticle(Vector2 pos)
            : base(pos)
        {
            texture = TextureManager.Map["cross"];
            rotSpeed = 0;
            angle = (float)(-Math.PI / 2);
            speed = 10;
            speed += GameConst.rand.Next(20);
            life = 1f;
            int s = 8 + GameConst.rand.Next(16);
            size = new Vector2(s, s);
            color = new Color(150, 150, 150, 150);
        }


        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
