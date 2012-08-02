using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class Stoneskin : Item
    {
        float toRestore = 0;

        public Stoneskin()
            : base()
        {
            texture = TextureManager.Map["stoneskin"];
            description = "increased defense, slower momement";
        }

        public override RestoreAction Use(Player p)
        {
            toRestore = p.Speed - (p.Speed / 3);
            p.Speed -= toRestore;
            p.Weakness = 2;
            p.statusColor = new Color(50, 50, 50, 255);
            return Restore;
        }

        public void Restore(Player p)
        {
            p.Speed += toRestore;
            p.Weakness = 0;
            p.statusColor = Color.White;
            toRestore = 0;
        }
    }
}
