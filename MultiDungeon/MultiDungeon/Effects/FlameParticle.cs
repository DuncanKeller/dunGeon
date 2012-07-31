using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Effects
{
    public class FlameParticle : Particle
    {
        float dampening = 300;

        public FlameParticle(Vector2 pos, float angle)
            : base(pos)
        {
            texture = TextureManager.Map["explosion-smoke"];
            rotSpeed = 0;
            this.angle = angle;
            speed = 350 + (float)(GameConst.rand.NextDouble() * 100) - 50; ;
            life = 1f;
            int s = 20 + GameConst.rand.Next(15);
            size = new Vector2(s, s);
            color = new Color(180 + GameConst.rand.Next(40), 0, 0, 150);
            life = 0.8f + (float)(GameConst.rand.NextDouble() * 0.4) - .2f;
        }


        public override void Update(float deltaTime)
        {
            base.Update(deltaTime); 
            if (speed > 0)
            {
                speed -= (deltaTime / 1000) * dampening;
            }
            else
            {
                speed = 0;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (texture.Width / 2)), (int)(pos.Y - (texture.Height / 2)),
               (int)size.X, (int)size.Y);

            if (rect.Right < World.Camera.pos.X - GameConst.SCREEN_WIDTH / 2 ||
               rect.Left > World.Camera.pos.X + GameConst.SCREEN_WIDTH / 2 ||
               rect.Bottom < World.Camera.pos.Y - GameConst.SCREEN_HEIGHT / 2 ||
               rect.Top > World.Camera.pos.Y + GameConst.SCREEN_HEIGHT / 2)
            {
                life = 0;
                speed = 0;
                return;
            }

            rect.X += (int)texture.Width / 2;
            rect.Y += (int)texture.Height / 2;
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            sb.Draw(texture, rect, source, color, rotation,
                new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
