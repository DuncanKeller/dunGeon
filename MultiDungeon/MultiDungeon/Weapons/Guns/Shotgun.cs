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
        int numShots;
        float spread = 0.75f;

        public Shotgun(BulletManager bm, Player p)
            : base(bm, typeof(Shell), p)
        {
            maxClip = 6;
            reloadTime = 1.6;
            rateOfFire = 0.4;
            damage = 0.4;
            clip = maxClip;
            numShots = clip;
            icon = TextureManager.Map["shotgun"];
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Shoot()
        {
            if (primed)
            {
                for (int i = 0; i < numShots; i++)
                {
                    float angle = (float)GameConst.rand.NextDouble() * spread;
                    angle -= spread / 2;
                    FireBullet(angle);
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

    }
}
