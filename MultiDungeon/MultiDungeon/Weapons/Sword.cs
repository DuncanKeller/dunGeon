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
        int reach = 50;
        Rectangle hitRect = new Rectangle(0, 0, 40, 40);
        float timer;
        float swordTime = 0.15f; // seconds
        List<Player> hasHit = new List<Player>();

        public Sword()
        {
            icon = TextureManager.Map["sword"];
        }

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

        public void Slice(float angle, Vector2 pos)
        {
            angle -= 90 * (float)(Math.PI / 180);
            if (timer == 0)
            {
                int centerX = (int)pos.X + (int)(Math.Cos(angle) * reach);
                int centerY = (int)pos.Y + (int)(Math.Sin(angle) * reach);

                hitRect.X = centerX;
                hitRect.Y = centerY;

                timer = swordTime;
            }
        }

        public override void Update(double dt)
        {
            if (timer > 0)
            {
                timer -= (float)(dt / 1000);
            }
            else
            {
                timer = 0;
                hasHit.Clear();
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Attacking)
            {
                sb.Draw(TextureManager.Map["blank"], hitRect, Color.Orange);
            }
        }

        public override void DrawIcon(SpriteBatch sb)
        {
            base.DrawIcon(sb);
            
        }
    }
}
