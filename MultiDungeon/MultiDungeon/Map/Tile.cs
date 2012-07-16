using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Map
{
    public enum TileType
    {
        floor,
        wall
    }

    public struct ColorScheme
    {
        public Color wall;
        public Color floor;

        public List<Color> walls;
        public List<Color> floors;
    }

    public class Tile
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

        public void Draw(SpriteBatch sb, ColorScheme colorScheme)
        {
            Color color = Color.White;

            Texture2D texture = null;
            switch (type)
            {
                case TileType.floor:
                    texture = TextureManager.Map["floor"];
                    color = colorScheme.floor;
                    break;
                case TileType.wall:
                    texture = TextureManager.Map["tile"];
                    color = colorScheme.wall;
                    break;
            }
            sb.Draw(texture, Rect, color);
        }
    }
}
