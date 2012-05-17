﻿using System;
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
        }

        public override void Use(Player p)
        {
            p.Health += p.MaxHealth / amount;
            if (p.Health > p.MaxHealth)
            {
                p.Health = p.MaxHealth;
            }
        }
    }
}
