using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Effects
{
    public class PoisinParticle : Particle
    {
        public PoisinParticle(Vector2 pos) : base(pos)
        {
            texture = TextureManager.Map["bubble"];
            rotSpeed = 0;
            angle = -(float)Math.PI / 2;
            speed = 30;
            speed += GameConst.rand.Next(30);
            life = 1;
            int s = 5 + GameConst.rand.Next(8);
            size = new Vector2(s, s);
            color = new Color(175, 175, 175, 175);
        }


        public override void Update(float deltaTime)
        {
            //if (color.A > 0)
            //{
            //    color.A -= 3;
            //    color.R -= 3;
            //    color.G -= 3;
            //    color.B -= 3;
            //}
            //else
            //{
            //    color.A = 0;
            //}
            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
