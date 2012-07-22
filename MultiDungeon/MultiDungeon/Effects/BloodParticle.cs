using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Effects
{
    public class BloodParticle : Particle
    {
        float xspeed;
        float yspeed;

        public override bool Alive
        {
            get { return yspeed < 250; }
        }

        public BloodParticle(Vector2 pos)
            : base(pos)
        {
            texture = TextureManager.Map["circle"];
            //rotSpeed = (float)(GameConst.rand.NextDouble() * Math.PI * 2);
            //rotSpeed -= (float)Math.PI;
            //angle = (float)(GameConst.rand.NextDouble() * Math.PI * 2);
            xspeed = -50;
            xspeed += GameConst.rand.Next(100);
            yspeed = -50 - GameConst.rand.Next(175);
            if (GameConst.rand.Next(8) == 0)
            {
                yspeed -= GameConst.rand.Next(200);
                xspeed =  -200 + GameConst.rand.Next(400);
            }
            life = 5.75f;
            int s = 7 + GameConst.rand.Next(6);
            size = new Vector2(s, s);
            color = new Color((byte)GameConst.rand.Next(15) + 40, 0, 0);
        }


        public override void Update(float deltaTime)
        {
            life -= deltaTime / 1000;
            pos.X += xspeed * (deltaTime / 1000);
            pos.Y += yspeed * (deltaTime / 1000);
            yspeed += 500 * (deltaTime / 1000);
            
            rotation = (float)Math.Atan2(xspeed, -yspeed);
            
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (texture.Width / 2) ), 
                (int)(pos.Y - (texture.Height / 2)),
                (int)(size.X), (int)(size.Y + (Math.Abs(yspeed) / 15)));
            rect.X += (int)size.X / 2;
            rect.Y += (int)size.Y / 2;
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            sb.Draw(texture, rect, source, color, rotation,
                new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
