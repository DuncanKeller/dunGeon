using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Effects
{
    enum ParticleType
    {
        Circle,
        Lens,
        Smoke
    }

    class Particle
    {
        ParticleType type;
        Vector2 pos;
        Texture2D texture;
        Vector2 size;
        Color color;
        float angle;
        float speed;
        float maxSpeed = 1000; // pixels per second
        float dampening = 200;

        public Particle(Vector2 pos, ParticleType pt)
        {
            this.pos = pos;
            angle = (float)(GameConst.rand.NextDouble() * Math.PI * 2);
            speed = maxSpeed;
            type = pt;
            switch (type)
            {
                case ParticleType.Circle:
                    size = new Vector2(25 + GameConst.rand.Next(-5, 5), 25 + GameConst.rand.Next(-5, 5));
                    texture = TextureManager.Map["explosion-circle"];
                    break;
                case ParticleType.Smoke:
                    size = new Vector2(50 + GameConst.rand.Next(-15, 15), 20 + GameConst.rand.Next(-15, 15));
                    texture = TextureManager.Map["explosion-lens"];
                    break;
                case ParticleType.Lens:
                    size = new Vector2(20 + GameConst.rand.Next(50), 20);
                    texture = TextureManager.Map["explosion-smoke"];  
                    break;
            }
            int c = GameConst.rand.Next(4);
            switch (c)
            {
                case 0:
                    color = Color.Red;
                    break;
                case 1:
                    color = Color.Yellow;
                    break;
                case 2:
                    color = Color.Orange;
                    break;
                case 3:
                    color = Color.DarkRed;
                    break;
            }
            byte cMod = (byte)GameConst.rand.Next(-10, 10);
            color.R += cMod;
            color.G += cMod;
            color.B += cMod;
        }

        public void Update(float deltatime)
        {
            speed -= dampening * (deltatime / 1000);
            pos.X += (float)Math.Cos(angle) * speed * (deltatime / 1000);
            pos.Y += (float)Math.Sin(angle) * speed * (deltatime / 1000);
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size.X / 2)), (int)(pos.Y - (size.Y / 2)),
                (int)size.X, (int)size.Y);
            sb.Draw(texture, rect, color);
        }
    }
}
