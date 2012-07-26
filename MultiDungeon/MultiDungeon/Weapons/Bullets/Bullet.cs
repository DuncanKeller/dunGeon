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
        protected Vector2 pos;
        protected float angle;
        protected float damage;
        protected float speed = 750;
        protected int pid;
        public Vector2 flip = new Vector2(1, 1);

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
            get { return new Rectangle((int)pos.X - 3, (int)pos.Y - 3, 5, 7); }
        }

        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        public Bullet()
        {
            
        }

        public void FlipX()
        {
            flip.X *= -1;
        }

        public void FlipY()
        {
            flip.Y *= -1;
        }

        public virtual void Init(Vector2 pos, float angle, float damage, int pid)
        {
            this.pos = pos;
            this.angle = angle;
            this.pid = pid;
            this.damage = damage;
        }

        public virtual void Update(float deltaTime)
        {
            pos.X += (float)Math.Cos(angle) * speed * (deltaTime / 1000) * flip.X;
            pos.Y += (float)Math.Sin(angle) * speed * (deltaTime / 1000) * flip.Y;

            ////homing
            //if (World.PlayerHash[pid].StatusEffect == StatusEffect.homing)
            //{
            //    foreach (Player p in World.Players)
            //    {
            //        if (p.ID != pid)
            //        {
            //            float distance = float.MaxValue;
            //            Vector2 playerPos = p.Position;
            //            Vector2.Distance(ref playerPos, ref pos, out distance);

            //            if (distance < 300)
            //            {
            //                float newAngle = (float)(Math.Atan2(playerPos.Y - pos.Y, playerPos.X = pos.X) );
            //                //newAngle /= 3;

            //                angle = newAngle;
            //            }
            //        }
            //    }
            //}

        }

        public virtual void Draw(SpriteBatch sb)
        {
            Texture2D texture = TextureManager.Map["bullet"];
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            sb.Draw(texture, Rect, source, Color.Silver, angle + (float)(Math.PI / 2),
                new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
