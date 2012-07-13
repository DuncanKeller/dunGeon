using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class GrenadeLauncher : Gun
    {
        public GrenadeLauncher(BulletManager bm, Player p)
            : base(bm, typeof(Grenade), p)
        {
            maxClip = 4;
            reloadTime = 3.2;
            rateOfFire = 1.2;
            damage = 0;
            clip = maxClip;
            icon = TextureManager.Map["grenade-launcher"];
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
