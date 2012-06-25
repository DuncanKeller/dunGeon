using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class Stoneskin : Item
    {
        // 1 - full health
        // 2 - half health
        // 3 - third health
        // 4 - fourth health
        float toRestore = 0;

        public Stoneskin()
            : base()
        {
            texture = TextureManager.Map["stoneskin"];
        }

        public override RestoreAction Use(Player p)
        {
            toRestore = p.Speed / 3;
            p.Speed /= 3;
            p.Weakness = 0.5;
            p.statusColor = Color.DarkGray;
            return Restore;
        }

        public void Restore(Player p)
        {
            p.Speed += toRestore;
            p.Weakness = 0;
            p.statusColor = Color.Black;
            toRestore = 0;
        }
    }
}
