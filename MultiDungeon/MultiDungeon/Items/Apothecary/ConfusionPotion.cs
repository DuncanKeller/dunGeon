using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class ConfusionPotion : Item
    {
        public ConfusionPotion()
            : base()
        {
            texture = TextureManager.Map["stoneskin"];
        }

        public override RestoreAction Use(Player p)
        {
            effectTime = 45; // seconds
            p.StatusEffect = StatusEffect.confuse;
            return Restore;
        }

        public void Restore(Player p)
        {
            p.StatusEffect = StatusEffect.none;
        }
    }
}
