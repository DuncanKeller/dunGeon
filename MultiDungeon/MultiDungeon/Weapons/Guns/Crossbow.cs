using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Crossbow : Gun
    {
        public Crossbow(BulletManager bm, Player p)
            : base(bm, typeof(Arrow), p)
        {
            maxClip = 1;
            reloadTime = 3.0;
            rateOfFire = 0.0;
            damage = 15;
            clip = maxClip;
            icon = TextureManager.Map["crossbow"];
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
