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
            reloadSpeed = 3;
            rateOfFire = 0.3;
            damage = 5;
        }

        public override void  Update(double deltaTime)
        {
 	         base.Update(deltaTime);
        }

        public override void Shoot(GamePadState input, GamePadState oldInput)
        {
          
        }

        public override void  Draw(SpriteBatch sb)
        {
 	         base.Draw(sb);
        }
        
    }
}
