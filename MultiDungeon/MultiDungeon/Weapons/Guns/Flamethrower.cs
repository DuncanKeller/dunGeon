﻿using System;
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

        float range = 140; // px
        float arc = (float)(Math.PI / 4.0); // radians

        public Flamethrower(BulletManager bm, Player p)
            : base(bm, typeof(Flame), p)
        {
            maxClip = 80;
            reloadTime = 3.0;
            rateOfFire = 0.07;
            damage = 0.2f;
            clip = maxClip;
            icon = TextureManager.Map["flames"];
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            if (clip < maxClip
                && !shooting)
            {
                clip += 1;
            }
            shooting = false;
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
                    FireBullet();
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
