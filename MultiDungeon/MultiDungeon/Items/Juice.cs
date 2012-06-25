using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class Juice : Item
    {
        // 1 - full health
        // 2 - half health
        // 3 - third health
        // 4 - fourth health
        float toRestore = 0;

        public Juice()
            : base()
        {
            texture = TextureManager.Map["juice"];
        }

        public override RestoreAction Use(Player p)
        {
            toRestore = p.Speed;
            p.Speed += p.Speed;
            p.Weakness = 1.5;
            p.statusColor = Color.GreenYellow;
            return Restore;
        }

        public void Restore(Player p)
        {
            p.Speed -= toRestore;
            p.Weakness = 0;
            p.statusColor = Color.White;
            toRestore = 0;
        }
    }
}
