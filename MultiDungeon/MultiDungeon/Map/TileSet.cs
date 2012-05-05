using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Map
{
    class TileSet
    {
        List<Tile> tiles = new List<Tile>();

        public TileSet()
        {
            GenerateMap(16, 10);
        }

        public List<Tile> Tiles
        {
            get { return tiles; }
        }

        public List<Tile> GetTilesNear(int x, int y)
        {
            List<Tile> toReturn = new List<Tile>();
            foreach (Tile t in tiles)
            {
                float dist = float.MaxValue;
                Vector2 checkPos = new Vector2(x, y);
                Vector2 tilePos = t.Position;
                Vector2.Distance(ref checkPos, ref tilePos, out dist);
                if (Math.Abs(dist) < 100)
                {
                    toReturn.Add(t);
                }
            }
            return toReturn;
        }

        public void GenerateMap(int width, int height)
        {
            bool[,] map = new bool[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    map[x, y] = false;
                }
            }

            for (int x = 0; x < width; x++)
            {
                map[x, 0] = true;
                map[x, height - 1] = true;
            }

            for (int y = 0; y < height; y++)
            {
                map[0, y] = true;
                map[width - 1, y] = true;
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile t;
                    if (map[x, y])
                    {
                        t = new Tile(TileType.wall, x, y);
                    }
                    else
                    {
                        t = new Tile(TileType.floor, x, y);
                    }
                    tiles.Add(t);
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(sb);
            }
        }
    }
}
