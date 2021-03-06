﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class VampirePotion : Item
    {
        //float toRestore = 0;

        public VampirePotion()
            : base()
        {
            texture = TextureManager.Map["potion-vampire"];
        }

        public override RestoreAction Use(Player p)
        {
            //toRestore = p.Speed - (p.Speed / 3);
            //p.Speed -= toRestore;
            effectTime = 30; // seconds
            p.StatusEffect = StatusEffect.vampire;
            p.statusColor = Color.DarkRed;
            SoundManager.PlaySound("potion");
            return Restore;
        }

        public void Restore(Player p)
        {
            //p.Speed += toRestore;
            p.StatusEffect = StatusEffect.none;
            p.statusColor = Color.White;
            //toRestore = 0;
        }
    }
}
