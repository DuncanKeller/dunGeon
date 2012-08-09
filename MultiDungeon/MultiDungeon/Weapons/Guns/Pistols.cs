using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
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
            damage = 0.75f;
            clip = maxClip;
            secondGun = g;
            icon = TextureManager.Map["pistols"];
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
                float v = (float)(GameConst.rand.NextDouble() * 0.2f) - 0.1f;
                SoundManager.SpecialSounds["gunshot"].Play(0.75f, v, 0);
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
