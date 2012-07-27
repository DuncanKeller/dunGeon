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
        wall,
        sidewall
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

        public Rectangle DrawRect
        {
            get { return new Rectangle((int)pos.X, (int)pos.Y , TILE_SIZE, TILE_SIZE); }
        }

        public Rectangle Rect
        {
            get { return new Rectangle((int)pos.X, (int)(pos.Y + (TILE_SIZE * (2.0f / 3.0f))), TILE_SIZE, TILE_SIZE); }
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

        public void Draw(SpriteBatch sb, ColorScheme colorScheme, Vector2 source, short ahead = 0)
        {
            if (DrawRect.Right > source.X - GameConst.SCREEN_WIDTH / 2 &&
                    DrawRect.Left < source.X + GameConst.SCREEN_WIDTH / 2 &&
                    DrawRect.Bottom > -100 - source.Y - GameConst.SCREEN_HEIGHT / 2 &&
                    DrawRect.Top < source.Y + GameConst.SCREEN_HEIGHT / 2)
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
                    case TileType.sidewall:
                        texture = TextureManager.Map["wall"];
                        color = colorScheme.wall;
                        break;
                }
                if (ahead == 0)
                {
                    sb.Draw(texture, DrawRect, color);
                }
                if (type == TileType.wall)
                {
                    if (source.Y > DrawRect.Top && ahead == 1)
                    {
                        sb.Draw(texture, DrawRect, color);
                    }
                    else if (source.Y <= DrawRect.Top && ahead == 2)
                    {
                        sb.Draw(texture, DrawRect, color);
                    }
                }

            }
        }

    }
}
