using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon.Items
{
    class TeamChest : Chest
    {
        int width = Tile.TILE_SIZE * 2;
        int height = Tile.TILE_SIZE * 2;
        int id;

        public TeamChest(int teamId, Vector2 pos) : base(teamId, pos, null)
        {
            this.id = teamId;
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
        }

        public override Item Open(Player p)
        {
            World.inMenu = true;
            World.menuManager.teamChest.Open(p);
            World.menuManager.SwitchMenu(World.menuManager.teamChest);
            return null;
        }


        public override void Draw(SpriteBatch sb)
        {
            Color c = id == 0 ? Color.Blue : Color.Red;
            sb.Draw(TextureManager.Map["chest-open"], rect, c);
        }
    }
}
