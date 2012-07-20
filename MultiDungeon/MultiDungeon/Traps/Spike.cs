using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon.Traps
{
    class Spike : Trap
    {
        bool retracting;
        bool spikesOut = false;

        static Texture2D tRetracted = TextureManager.Map["spikes-retracted"];
        static Texture2D tPrimed = TextureManager.Map["spikes-low"];
        static Texture2D tOut = TextureManager.Map["spikes-high"];
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

        public void TestHit(Player p)
        {
            if (spikesOut)
            {
                if (p.DrawRect.Intersects(rect))
                {
                    p.Die();
                }
            }
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
                    if (timer < inTime / 7)
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

        public override void Update(float deltaTime)
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

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Texture, DrawRect, Color.White);
        }
    }
}