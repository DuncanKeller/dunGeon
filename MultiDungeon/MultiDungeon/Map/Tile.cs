using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Map
{
    enum TileType
    {
        floor,
        wall
    }

    class Tile
    {
        public static int TILE_SIZE = 64;
        TileType type;
        Vector2 pos;

        public Rectangle Rect
        {
            get { return new Rectangle((int)pos.X, (int)pos.Y, TILE_SIZE, TILE_SIZE); }
        }

        public TileType Type
        {
            get { return type; }
        }

        public Vector2 Position
        {
            get { return pos; }
        }

        public Tile(TileType t, int x, int y)
        {
            this.pos = new Vector2(x * TILE_SIZE, y * TILE_SIZE);
            type = t;
        }

        public void Draw(SpriteBatch sb)
        {
            Color color = Color.DarkSlateBlue;
            Texture2D texture = null;
            switch (type)
            {
                case TileType.floor:
                    texture = TextureManager.Map["blank"];
                    break;
                case TileType.wall:
                    texture = TextureManager.Map["tile"];
                    break;
            }
            sb.Draw(texture, Rect, color);
        }
    }
}
