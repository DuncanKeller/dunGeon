using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon.Items
{
    public class Chest
    {
        protected Rectangle rect;
        Item contents;

        int width = Tile.TILE_SIZE;
        int height = Tile.TILE_SIZE;
        int id;

        public Rectangle Rect
        {
            get { return rect; }
        }

        public int ID
        {
            get { return id; }
        }

        public Chest(int id, Vector2 pos, Item i)
        {
            this.id = id;
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            contents = i;
        }

        public virtual Item Open(Player p)
        {
            if (contents != null)
            {
                Item returnItem = contents;
                contents = null;
                return returnItem;
            }
            return p.Item;
        }

        public virtual void Open()
        {
            if (contents != null)
            {
                contents = null;
            }
        }

        public virtual void Draw(SpriteBatch sb)
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
