using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class InvincibilityPotion : Item
    {
        public InvincibilityPotion()
            : base()
        {
            texture = TextureManager.Map["potion-invincibility"];
        }

        public override RestoreAction Use(Player p)
        {
            effectTime = 8; // seconds
            p.StatusEffect = StatusEffect.invinsible;
            p.statusColor = Color.Gold;
            SoundManager.PlaySound("potion");
            return Restore;
        }

        public void Restore(Player p)
        {
            p.StatusEffect = StatusEffect.none;
            p.statusColor = Color.White;
        }
    }
}
