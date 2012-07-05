using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Weapons
{
    class Sword : Weapon
    {
        int reach = 100;
        Rectangle hitRect = new Rectangle(0, 0, 60, 60);
        float timer;
        float swordTime = 0.4f; // seconds
        List<Player> hasHit = new List<Player>();

        public bool HasHit(Player p)
        {
            return hasHit.Contains(p);
        }

        public void Hit(Player p)
        {
            hasHit.Add(p);
        }

        public bool Attacking
        {
            get { return timer > 0; }
        }

        public void Slice(float angle)
        {
            int centerX = (int)(Math.Cos(angle) * reach);
            int centerY = (int)(Math.Sin(angle) * reach);

            hitRect.X = centerX - hitRect.Width / 2;
            hitRect.Y = centerY - hitRect.Height / 2;

            timer = swordTime;
        }

        public void Update(float dt)
        {
            if (timer > 0)
            {
                timer -= dt / 1000;
            }
            else
            {
                timer = 0;
                hasHit.Clear();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if (Attacking)
            {
                sb.Draw(TextureManager.Map["blank"], hitRect, Color.Orange);
            }
        }
    }
}
