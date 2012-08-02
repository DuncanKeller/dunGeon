using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class SpareMag : Item
    {
        public SpareMag()
            : base()
        {
            texture = TextureManager.Map["mag"];
            effectTime = 0.1;
            description = "a substitute for reloading, and 4 times faster";
        }

        public override RestoreAction Use(Player p)
        {
            if (p.CurrentGun is Gun)
            {
                Gun g = (Gun)p.CurrentGun;
                g.QuickReload();
            }
            return null;
        }
    }
}
