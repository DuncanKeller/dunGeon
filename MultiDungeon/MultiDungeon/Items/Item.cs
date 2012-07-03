using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Items
{
    public delegate void RestoreAction(Player p);

    public abstract class Item
    {
        protected Texture2D texture;

        protected double effectTime = 0;

        public double EffectTime
        {
            get { return effectTime; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public abstract RestoreAction Use(Player p);

        public void Draw(SpriteBatch sb, Vector2 v)
        {
            Rectangle r = new Rectangle((int)v.X, (int)v.Y, 60, 60);
            sb.Draw(texture, r, Color.White);
        }
    }
}
