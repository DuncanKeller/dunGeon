using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon.Traps
{
    class Spike
    {
        bool retracting;
        bool spikesOut = false;

        Texture2D tRetracted;
        Texture2D tPrimed;
        Texture2D tOut;
        Rectangle rect;

        float timer;
        float outTime = 3; // seconds
        float inTime = 3;

        public Spike(int x, int y, bool retracting)
        {
            rect = new Rectangle(x, y, Tile.TILE_SIZE, Tile.TILE_SIZE);
            /*
            * add textures
            */
            this.retracting = retracting;
            spikesOut = !retracting;
            timer = inTime;
        }

        public Texture2D Texture
        {
            get
            {
                if (spikesOut)
                {
                    return tOut;
                }
                else
                {
                    if (timer < inTime / 10)
                    {
                        return tPrimed;
                    }
                    else
                    {
                        return tRetracted;
                    }
                }
            }
        }

        public bool SpikesOut
        {
            get { return spikesOut; }
        }

        public Rectangle Rect
        {
            get { return rect; }
        }

        public Rectangle DrawRect
        {
            get
            {
                return new Rectangle(rect.X, rect.Y - Tile.TILE_SIZE / 2,
                    Tile.TILE_SIZE, Tile.TILE_SIZE + (Tile.TILE_SIZE / 2));
            }
        }

        public void Update(float deltaTime)
        {
            if (retracting)
            {
                if (spikesOut)
                {
                    if (timer > 0)
                    {
                        timer -= deltaTime / 1000;
                    }
                    else
                    {
                        spikesOut = !spikesOut;
                        timer = inTime;
                    }
                }
                else
                {
                    if (timer > 0)
                    {
                        timer -= deltaTime / 1000;
                    }
                    else
                    {
                        spikesOut = !spikesOut;
                        timer = outTime;
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, DrawRect, Color.White);
        }
    }
}