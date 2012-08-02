using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class SeeingEye : Item
    {
        public SeeingEye()
            : base()
        {
            texture = TextureManager.Map["eye"];
            effectTime = 15;
            description = "see beyond the veil of shadow";
        }

        public override RestoreAction Use(Player p)
        {
            World.Game.shadowColor = new Color(20, 20, 20, 100);
            p.statusColor = Color.LightYellow;
            return Restore;
        }

        public void Restore(Player p)
        {
            World.Game.shadowColor = Color.Black;
            p.statusColor = Color.White;
        }
    }
}
