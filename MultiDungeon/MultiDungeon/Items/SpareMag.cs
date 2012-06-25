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
        }

        public override RestoreAction Use(Player p)
        {
            p.CurrentGun.QuickReload();
            return null;
        }
    }
}
