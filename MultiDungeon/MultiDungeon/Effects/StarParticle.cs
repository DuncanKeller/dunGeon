using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Effects
{
    public class StarParticle : Particle
    {
        public StarParticle(Vector2 pos)
            : base(pos)
        {
            texture = TextureManager.Map["star"];
            rotSpeed = (float)(GameConst.rand.NextDouble() * Math.PI * 2);
            rotSpeed -= (float)Math.PI;
            angle = (float)(GameConst.rand.NextDouble() * Math.PI * 2);
            speed = 30;
            speed += GameConst.rand.Next(30);
            life = 0.5f;
            int s = 5 + GameConst.rand.Next(8);
            size = new Vector2(s, s);
            color = new Color(150, 150, 0, 150);
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
