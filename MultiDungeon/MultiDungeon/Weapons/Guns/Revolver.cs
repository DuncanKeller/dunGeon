﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Revolver : Gun
    {
        public Revolver(BulletManager bm, Player p) : base(bm, typeof(Bullet), p)
        {
            maxClip = 6;
            reloadTime = 2.2;
            rateOfFire = 0.1;
            damage = 5;
            clip = maxClip;
        }

        public override void  Update(double deltaTime)
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

        public override void  Draw(SpriteBatch sb)
        {
 	         base.Draw(sb);
        }
        
    }
}
