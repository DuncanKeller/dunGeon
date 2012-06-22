using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Items
{
    public abstract class Item
    {
        protected Texture2D texture;

        public abstract void Use(Player p);

        public void Draw(SpriteBatch sb, Vector2 v)
        {
            Rectangle r = new Rectangle((int)v.X, (int)v.Y, 60, 60);
            sb.Draw(texture, r, Color.White);
        }
    }
}
