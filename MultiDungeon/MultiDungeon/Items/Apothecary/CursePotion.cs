using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class CursePotion : Item
    {
        public CursePotion()
            : base()
        {
            texture = TextureManager.Map["potion-curse"];
        }

        public override RestoreAction Use(Player p)
        {
            effectTime = 30; // seconds
            p.StatusEffect = StatusEffect.curse;
            p.statusColor = Color.LightGreen;
            return Restore;
        }

        public void Restore(Player p)
        {
            p.StatusEffect = StatusEffect.none;
            p.statusColor = Color.White;
        }
    }
}
