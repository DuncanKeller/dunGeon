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
        protected Vector2 pos = new Vector2();
        protected Vector2 dest = new Vector2(GameConst.SCREEN_WIDTH - 80, 20);
        protected double effectTime = 0;

        protected static string description = String.Empty;

        int size = 60;

        float timer = 0.84f;

        public string Description
        {
            get { return description; }
        }

        public Vector2 Pos
        {
            get { return pos; }
        }

        public double EffectTime
        {
            get { return effectTime; }
        }

        public Texture2D Texture
        {
            get { return texture; }
        }

        public bool AtDestination
        {
            get { return (Vector2.Distance(pos, dest) < 5); }
        }

        public void Update(float deltaTime, int x, int y, int w, int h)
        {
            if (this is CoinPurse)
            {
                if (dest.X == GameConst.SCREEN_WIDTH - 80)
                {
                    dest = new Vector2(10,40);
                }
            }
            if (timer > 0)
            {
                timer -= deltaTime / 1000;
                pos.X = x - (size / 2);
                pos.Y = y - size - 20 - h;
            }
            else
            {
                pos = Vector2.Lerp(pos, dest, 0.15f);
            }
        }


        public abstract RestoreAction Use(Player p);

        public void Draw(SpriteBatch sb, Vector2 v)
        {
            Rectangle r = new Rectangle((int)v.X, (int)v.Y, size, size);
            sb.Draw(texture, r, Color.White);
        }
    }
}
