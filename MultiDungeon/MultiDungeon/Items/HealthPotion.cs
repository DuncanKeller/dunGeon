using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiDungeon.Items
{
    enum HealingLevel
    {
        full = 1,
        strong = 2,
        regular = 3,
        weak = 4
    }

    class HealthPotion : Item
    {
        // 1 - full health
        // 2 - half health
        // 3 - third health
        // 4 - fourth health
        int amount;

        public HealthPotion(HealingLevel a)
            : base()
        {
            amount = (int)a;
            texture = TextureManager.Map["potion"];
            effectTime = 1;

            description = "refill 2/3 health";
        }

        public override RestoreAction Use(Player p)
        {
            p.StatusEffect = StatusEffect.health;
            p.Health += p.MaxHealth / amount;
            if (p.Health > p.MaxHealth)
            {
                p.Health = p.MaxHealth;
            }
            SoundManager.PlaySound("potion");
            return Restore;
        }

        public void Restore(Player p)
        {
            p.StatusEffect = StatusEffect.none;
        }
    }
}
