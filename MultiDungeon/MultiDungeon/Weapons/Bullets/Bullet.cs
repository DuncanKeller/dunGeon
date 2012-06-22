using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    public class Bullet
    {
        Vector2 pos;
        float angle;
        double damage;
        protected float speed = 15;
        int pid;

        public Vector2 Position
        {
            get { return pos; }
        }

        public double Damage
        {
            get { return damage; }
        }

        public int PlayerID
        {
            get { return pid; }
        }

        public Rectangle Rect
        {
            get { return new Rectangle((int)pos.X - 3, (int)pos.Y - 3, 6, 6); }
        }

        public float Angle
        {
            get { return angle; }
        }

        public Bullet()
        {
            
        }

        public virtual void Init(Vector2 pos, float angle, double damage, int pid)
        {
            this.pos = pos;
            this.angle = angle;
            this.pid = pid;
            this.damage = damage;
        }

        public virtual void Update()
        {
            pos.X += (float)Math.Cos(angle) * speed;
            pos.Y += (float)Math.Sin(angle) * speed;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.Map["blank"], Rect, Color.Black);
        }
    }
}
