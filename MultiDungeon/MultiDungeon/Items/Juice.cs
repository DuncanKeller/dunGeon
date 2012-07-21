using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class Juice : Item
    {
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
            p.StatusEffect = StatusEffect.speed;
            p.statusColor = Color.Purple;
            return Restore;
        }

        public void Restore(Player p)
        {
            p.Speed -= toRestore;
            p.Weakness = 0;
            p.statusColor = Color.White;
            p.StatusEffect = StatusEffect.none;
            toRestore = 0;
        }
    }
}
