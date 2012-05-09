using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class AssaultRifle : Gun
    {
        public AssaultRifle(BulletManager bm, Player p)
            : base(bm, typeof(Bullet), p)
        {
            maxClip = 60;
            reloadTime = 3.0;
            rateOfFire = 0.05;
            damage = 0.5;
            clip = maxClip;
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Shoot()
        {
            if (primed)
            {
                FireBullet();
            }
        }

        public override void RightHeld()
        {
            Shoot();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

    }
}
