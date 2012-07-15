using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class MidasPotion : Item
    {
        public MidasPotion()
            : base()
        {
            texture = TextureManager.Map["potion-midas"];
        }

        public override RestoreAction Use(Player p)
        {
            effectTime = 45; // seconds
            p.StatusEffect = StatusEffect.midas;
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
