using System;
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

        int reach = 42;
        Rectangle hitRect = new Rectangle(0, 0, 50, 50);
        float timer;
        float swordTime = 0.2f; // seconds
        float angle = 0;
        float damage;
        List<Player> hasHit = new List<Player>();
        Animator animator;
        Player player;

        public Sword(Player p)
        {
            icon = TextureManager.Map["sword"];
            animator = new Animator(TextureManager.Map["slice"], 1, 4);
            animator.AddAnimation(A_SLICE, 0, 3, 18, false);
            damage = 1.5f;
            player = p;
        }
        
        public bool HasHit(Player p)
        {
            return hasHit.Contains(p);
        }

        public bool CollidingWithEnemy(Player p)
        {
            return p.DrawRect.Intersects(hitRect);
        }

        public void DealDamage(Player p)
        {
            p.Health -= damage;

            if (p.ID == World.gameId)
            {
                if (p.Health <= 0)
                {
                    player.Gold += 50;
                    p.Die();
                }
            }
        }

        public void Hit(Player p)
        {
            hasHit.Add(p);
        }

        public bool Attacking
        {
            get { return timer > 0; }
        }

        public void Slice()
        {
            if (timer == 0)
            {
                timer = swordTime;

                animator.Play(A_SLICE);
            }
        }

        public override void Update(double dt)
        {
            float angle = player.Angle;
            float size = player.DrawRect.Height / 2;
            Vector2 pos = new Vector2(player.Position.X, player.Position.Y - size);
            angle -= 90 * (float)(Math.PI / 180);
            this.angle = angle - (90 * (float)(Math.PI / 180));
            int centerX = (int)pos.X + (int)(Math.Cos(angle) * reach);
            int centerY = (int)pos.Y + (int)(Math.Sin(angle) * reach);

            hitRect.X = centerX;
            hitRect.Y = centerY;
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
                Rectangle drawRect = new Rectangle(hitRect.X + hitRect.Width / 2, hitRect.Y + hitRect.Height / 2,
                    hitRect.Width, hitRect.Height);
                //sb.Draw(TextureManager.Map["blank"], hitRect, Color.Orange);
                animator.Draw(sb, drawRect, Color.White, angle, new Vector2(0.5f, 0.5f));
            }
        }

        public override void DrawIcon(SpriteBatch sb)
        {
            base.DrawIcon(sb);
            
        }
    }
}
