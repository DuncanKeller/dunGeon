using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Shotgun : Gun
    {
        public Shotgun(BulletManager bm, Player p)
            : base(bm, typeof(Shell), p)
        {
            maxClip = 6;
            reloadTime = 3;
            rateOfFire = 0.4;
            damage = 10;
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

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

    }
}
