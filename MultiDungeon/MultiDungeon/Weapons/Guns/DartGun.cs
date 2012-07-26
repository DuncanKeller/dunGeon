using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class DartGun : Gun
    {
        public DartGun(BulletManager bm, Player p)
            : base(bm, typeof(Dart), p)
        {
            maxClip = 3;
            reloadTime = 1;
            rateOfFire = 0.3;
            damage = 0.2f;
            clip = maxClip;
            icon = TextureManager.Map["dartgun"];
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
