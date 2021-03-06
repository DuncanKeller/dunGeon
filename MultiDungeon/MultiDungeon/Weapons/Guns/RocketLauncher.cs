﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class RocketLauncher : Gun
    {
        public RocketLauncher(BulletManager bm, Player p)
            : base(bm, typeof(Rocket), p)
        {
            maxClip = 2;
            reloadTime = 4;
            rateOfFire = 2;
            damage = 0;
            clip = maxClip;
            icon = TextureManager.Map["rocket-launcher"];
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
