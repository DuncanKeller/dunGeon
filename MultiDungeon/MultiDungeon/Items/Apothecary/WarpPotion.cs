using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Items
{
    class WarpPotion : Item
    {
        public WarpPotion()
            : base()
        {
            texture = TextureManager.Map["stoneskin"];
        }

        public override RestoreAction Use(Player p)
        {
            Vector2 v = new Vector2(World.Map.GetTeamRoom(p.Team).X + (World.Map.GetTeamRoom(p.Team).Width / 2),
                World.Map.GetTeamRoom(p.Team).Y + (World.Map.GetTeamRoom(p.Team).Height / 2));
            p.Position = v;
            return null;
        }

        //public void Restore(Player p)
        //{

        //}
    }
}
