using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class InvinsibilityPotion : Item
    {
        public InvinsibilityPotion()
            : base()
        {
            texture = TextureManager.Map["stoneskin"];
        }

        public override RestoreAction Use(Player p)
        {
            p.StatusEffect = StatusEffect.invinsible;
            p.statusColor = Color.Gold;
            return Restore;
        }

        public void Restore(Player p)
        {
            p.StatusEffect = StatusEffect.none;
            p.statusColor = Color.White;
        }
    }
}
