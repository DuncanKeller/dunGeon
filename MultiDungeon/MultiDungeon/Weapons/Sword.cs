﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Duncanimation;

namespace MultiDungeon.Weapons
{
    class Sword : Weapon
    {
        const string A_SLICE = "slice";

        int reach = 50;
        Rectangle hitRect = new Rectangle(0, 0, 40, 40);
        float timer;
        float swordTime = 0.15f; // seconds
        float angle = 0;
        List<Player> hasHit = new List<Player>();
        Animator animator;

        public Sword()
        {
            icon = TextureManager.Map["sword"];
            animator = new Animator(TextureManager.Map["slice"], 1, 4);
            animator.AddAnimation(A_SLICE, 0, 3, 20, false);
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
            this.angle = angle - (90 * (float)(Math.PI / 180));
            if (timer == 0)
            {
                int centerX = (int)pos.X + (int)(Math.Cos(angle) * reach);
                int centerY = (int)pos.Y + (int)(Math.Sin(angle) * reach);

                hitRect.X = centerX;
                hitRect.Y = centerY;

                timer = swordTime;

                animator.Play(A_SLICE);
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
            animator.Update((float)dt);
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Attacking)
            {
                //sb.Draw(TextureManager.Map["blank"], hitRect, Color.Orange);
                animator.Draw(sb, hitRect, Color.White, angle);
            }
        }

        public override void DrawIcon(SpriteBatch sb)
        {
            base.DrawIcon(sb);
            
        }
    }
}
