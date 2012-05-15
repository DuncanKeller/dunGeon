using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MultiDungeon.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.HUD
{
    enum Location
    {
        unknown,
        floor,
        wall
    }

    static class Map
    {
        static Location[,] map;
        static TileSet tiles;
        static int radius = 100;
        static int WIDTH = 10;
        static int HEIGHT = 10;
        static int SPACING = 1;

        public static void Init(TileSet ts)
        {
            tiles = ts;
            map = new Location[ts.Width, ts.Height];

            for (int x = 0; x < tiles.Width; x++)
            {
                for (int y = 0; y < tiles.Height; y++)
                {
                    map[x, y] = Location.unknown;
                }
            }
        }

        public static void Update(Vector2 playerLoc)
        {
            for (int x = 0; x < tiles.Width; x++)
            {
                for (int y = 0; y < tiles.Height; y++)
                {
                    if (map[x, y] == Location.unknown)
                    {
                        float distance = Vector2.Distance(playerLoc, new Vector2(
                            x * Tile.TILE_SIZE, y * Tile.TILE_SIZE));
                        if (Math.Abs(distance) < radius)
                        {
                            switch (tiles[x, y].Type)
                            {
                                case TileType.wall:
                                    map[x, y] = Location.wall;
                                    break;
                                case TileType.floor:
                                    map[x, y] = Location.floor;
                                    break;
                            }
                        }
                    }
                }
            }
        }

        public static void Draw(SpriteBatch sb, Vector2 pos)
        {
            for (int x = 0; x < tiles.Width; x++)
            {
                for (int y = 0; y < tiles.Height; y++)
                {
                    if (map[x, y] != Location.unknown)
                    {

                        Rectangle rect = new Rectangle((int)pos.X + (x * (WIDTH + SPACING)),
                            (int)pos.Y + (y * (HEIGHT + SPACING)), 
                            WIDTH - (SPACING * 2), HEIGHT - (SPACING * 2));
                        Color c = new Color(50, 50, 50, 50);
                        if (map[x, y] == Location.floor)
                        { c = new Color(255, 0, 0, 255); }

                        sb.Draw(TextureManager.Map["blank"], rect, c);
                    }
                }
            }
        }
    }
}
