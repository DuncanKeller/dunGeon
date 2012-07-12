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
        Smoke,
        RedSmoke
    }

    public enum ParticleLevel
    {
        Low = 10,
        Med = 30,
        High = 60
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
        float dampening = 70f;
        float dadt = 0;
        float rotation;
        float rotSpeed;

        public bool Alive
        {
            get { return speed > 1; }
        }

        public Particle(Vector2 pos, ParticleType pt)
        {
            this.pos = pos;
            angle = (float)(GameConst.rand.NextDouble() * Math.PI * 2);
            rotation = (float)(GameConst.rand.NextDouble() * Math.PI * 2);
            rotSpeed = (float)((GameConst.rand.NextDouble() * Math.PI * 2 ) - Math.PI);
            speed = maxSpeed - GameConst.rand.Next((int)maxSpeed) ;
            type = pt;
            int s;
            switch (type)
            {
                case ParticleType.Circle:
                    s = 25 + GameConst.rand.Next(-5, 5);
                    size = new Vector2(s, s);
                    texture = TextureManager.Map["explosion-circle"];
                    break;
                case ParticleType.Smoke:
                    s = 50 + GameConst.rand.Next(-30, 30);
                    size = new Vector2(s, s);
                    texture = TextureManager.Map["explosion-smoke"];
                    break;
                case ParticleType.RedSmoke: 
                    s = 50 + GameConst.rand.Next(-30, 30);
                    size = new Vector2(s, s);
                    texture = TextureManager.Map["explosion-smoke"];
                    speed = speed - GameConst.rand.Next((int)speed);
                    break;
                case ParticleType.Lens:
                    size = new Vector2(50 + GameConst.rand.Next(50), 20);
                    texture = TextureManager.Map["explosion-lens"];
                    speed -= maxSpeed / 3;
                    break;
            }
            int c = GameConst.rand.Next(4);
            if (type == ParticleType.Circle || type == ParticleType.Lens)
            {
                switch (c)
                {
                    case 0:
                        color = new Color(100, 75, 0);
                        break;
                    case 1:
                        color = new Color(100, 20, 0);
                        break;
                    case 2:
                        color = new Color(140, 0, 0);
                        break;
                    case 3:
                        color = new Color(75, 0, 0);
                        break;
                }
            }
            else if (type == ParticleType.RedSmoke)
            {
                int b = GameConst.rand.Next(100);
                color = new Color((byte)b, 0, 0, 50);
            }
            else
            {
                color = new Color(0, 0, 0, 50);
            }
            //byte cMod = (byte)GameConst.rand.Next(-10, 10);
            //color.R += cMod;
            //color.G += cMod;
            //color.B += cMod;
        }

        public void Update(float deltatime)
        {
            dadt += 1000 * (deltatime / 1000);
            //dampening -= dadt;
            speed /= dampening * (deltatime / 1000);
            pos.X += (float)Math.Cos(angle) * speed * (deltatime / 1000);
            pos.Y += (float)Math.Sin(angle) * speed * (deltatime / 1000);
            rotation += rotSpeed * (deltatime / 1000);

            if (type == ParticleType.Lens)
            {
                size.X /= 1.1f;
            }
            else if (type == ParticleType.Circle)
            {
                size.X /= 1.04f;
                size.Y /= 1.04f;
            }
            else
            {
                size.X += 0.90f;
                size.Y += 0.90f;
                if (type == ParticleType.RedSmoke)
                {
                    if (color.R > 0)
                    {
                        color.R -= (byte)5;
                    }
                    else
                    {
                        color.R = 0;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle rect = new Rectangle((int)(pos.X - (size.X / 2)), (int)(pos.Y - (size.Y / 2)),
                (int)size.X, (int)size.Y);
            Rectangle source = new Rectangle(0,0,texture.Width, texture.Height);
            sb.Draw(texture, rect, source, color, rotation, 
                new Vector2(texture.Width / 2, texture.Height / 2), SpriteEffects.None, 0);
        }
    }
}
