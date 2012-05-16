using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon.Items
{
    class Chest
    {
        Rectangle rect;
        Item contents;

        int width = Tile.TILE_SIZE;
        int height = Tile.TILE_SIZE;

        public Rectangle Rect
        {
            get { return rect; }
        }

        public Chest(Vector2 pos, Item i)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            contents = i;
        }

        public Item Open(Player p)
        {
            if (contents != null)
            {
                Item returnItem = contents;
                contents = null;
                return returnItem;
            }
            return p.Item;
        }

        public void Draw(SpriteBatch sb)
        {
            if (contents == null)
            {
                sb.Draw(TextureManager.Map["chest-open"], rect, Color.White);
            }
            else
            {
                sb.Draw(TextureManager.Map["chest-closed"], rect, Color.White);
            }
        }
    }
}
