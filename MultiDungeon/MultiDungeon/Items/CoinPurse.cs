﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiDungeon.Items
{
    class CoinPurse : Item
    {
        int amount;

        public CoinPurse(int a)
            : base()
        {
            amount = a;
        }

        public override void Use(Player p)
        {
            p.Gold += amount;
        }
    }
}