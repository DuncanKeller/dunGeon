using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MultiDungeon.Map;

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
            Vector2 v = new Vector2((World.Map.GetTeamRoom(p.Team).X * Tile.TILE_SIZE) + (World.Map.GetTeamRoom(p.Team).Width / 2),
                (World.Map.GetTeamRoom(p.Team).Y * Tile.TILE_SIZE) + (World.Map.GetTeamRoom(p.Team).Height / 2));
            p.Position = v;
            World.BulletManager.WarpEffect(p);
            return null;
        }

        //public void Restore(Player p)
        //{

        //}
    }
}
