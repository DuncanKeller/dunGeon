using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Pistols : Gun
    {
        Pistols secondGun;

        public Pistols(BulletManager bm, Player p, Pistols g = null)
            : base(bm, typeof(Bullet), p)
        {
            maxClip = 12;
            reloadTime = 1.35;
            rateOfFire = 0.05;
            damage = 1.5;
            clip = maxClip;
            secondGun = g;
        }

        public override void  Update(double deltaTime)
        {
 	         base.Update(deltaTime);
             if (secondGun != null)
             {
                 secondGun.Update(deltaTime);
             }
        }

        public override void Shoot()
        {
            if (primed)
            {
                FireBullet();
            }
        }

        public override void SecondaryFire()
        {
            if (secondGun != null)
            {
                if (secondGun.primed)
                {
                    secondGun.FireBullet();
                }
            }
        }

        public override void  Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            if (secondGun != null)
            {
                secondGun.Draw(sb);
            }
        }
    }
}
