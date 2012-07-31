using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Flamethrower : Gun
    {
        bool shooting = false;
        public float spread = 0.75f;
        float maxTimer = 0.3f;
        float timer = 0;

        float range = 140; // px
        float arc = (float)(Math.PI / 4.0); // radians

        public int Id
        {
            get {return player.ID;}
        }

        public float Damage
        {
            get { return damage; }
        }

        public bool Shooting
        {
            get
            {
                return shooting;
            }
        }

        public Flamethrower(BulletManager bm, Player p)
            : base(bm, typeof(Flame), p)
        {
            maxClip = 80;
            reloadTime = 3.0;
            rateOfFire = 0.07;
            damage = 0.35f;
            clip = maxClip;
            icon = TextureManager.Map["flames"];
        }

        public bool Overlapping(Player p)
        {

            if (Vector2.Distance(p.Position, player.Position) < range)
            {
                float minArc = player.Angle - (float)(Math.PI / 2) - arc;
                float maxArc = player.Angle - (float)(Math.PI / 2) + arc;
                float direction = (float)Math.Atan2(player.Position.Y - p.Position.Y, 
                    player.Position.X - p.Position.X);
                if (direction > minArc && direction < maxArc)
                {
                    return true;
                }
            }
            return false;
        }
        
        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            if (clip < maxClip
                && !shooting)
            {
                clip += 1;
            }

            if (shooting)
            {
                if (timer > 0)
                {
                    timer -= (float)deltaTime / 1000;
                }
                else
                {
                    if (World.gameId == player.ID)
                    {
                        Client.Send("flames\n" + player.ID + "!");
                    }
                    timer = maxTimer;
                }
            }

            shooting = false;
        }

        public void Flames()
        {
            if (primed)
            {
                for (int i = 0; i < 5; i++)
                {
                    //FireBullet();
                    
                }
            }
            shooting = true;
        }

        public override void Reload()
        {
            
        }

        public override void Shoot()
        {
            if (primed)
            {
                for (int i = 0; i < 5; i++)
                {
                    //FireBullet();
                }
            }
        }

        public override void RightHeld()
        {
            Shoot();
            shooting = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
