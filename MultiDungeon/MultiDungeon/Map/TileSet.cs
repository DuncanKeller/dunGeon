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

        List<Rectangle> rooms = new List<Rectangle>();
        List<Rectangle> cooridors = new List<Rectangle>();

        int numRooms = 12;

        public TileSet()
        {
            GenerateMap(50, 50);
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
        /*
        private Rectangle MakeRoomFromPath(ref bool[,] map, Rectangle path, int mwidth, int mheight, int rwidth, int rheight)
        {

        }
        */
        private Rectangle MakeRoom(ref bool[,] map, int mwidth, int mheight, int rwidth, int rheight)
        {
            Rectangle room = Rectangle.Empty;
            bool completed = false;
            while (!completed)
            {
                bool overlapping = false;

                // generate random room
                room = new Rectangle(
                    1 + GameConst.rand.Next(mwidth - 2),
                    1 + GameConst.rand.Next(mheight - 2),
                    3 + GameConst.rand.Next(rwidth - 3),
                    3 + GameConst.rand.Next(rheight - 3));

                // test for room lying outside bounds
                if (room.Right > mwidth ||
                    room.Bottom > mheight)
                {
                    continue;
                }

                // test for intersection with rooms
                for (int x = room.X; x < room.Right; x++)
                {
                    for (int y = room.Y; y < room.Bottom; y++)
                    {
                        if (map[x, y])
                        {
                            // overlapping another room
                            overlapping = true;
                        }
                    }
                }

                if (overlapping)
                {
                    continue;
                }

                completed = true;
            }

            // mark room on map
            for (int x = room.X; x < room.Right; x++)
            {
                for (int y = room.Y; y < room.Bottom; y++)
                {
                    map[x, y] = true;
                }
            }
            rooms.Add(room);
            return room;
        }

        private Rectangle GetEntrance(bool[,] map, int mwidth, int mheight)
        {
            bool completed = false;
            Rectangle entrance = Rectangle.Empty;
            while (!completed)
            {
                int id = GameConst.rand.Next(rooms.Count);
                bool overlapping = false;
                Rectangle room = rooms[id];

                int side = GameConst.rand.Next(3);
                int sx = 0;
                int sy = 0;
                int width = 0;
                int height = 0;
                Rectangle checkRect = Rectangle.Empty;

                switch (side)
                {
                    case 0:
                        sx = room.X + GameConst.rand.Next(room.Width);
                        sy = room.Y - 3;
                        width = room.Width;
                        height = 1;
                        checkRect = new Rectangle(room.X, room.Y - 1, room.Width, 1);
                        break;
                    case 1:
                        sx = room.X + GameConst.rand.Next(room.Width);
                        sy = room.Bottom;
                        width = room.Width;
                        height = 1;
                        checkRect = new Rectangle(room.X, room.Bottom, room.Width, 1);
                        break;
                    case 2:
                        sx = room.X - 3;
                        sy = room.Y + GameConst.rand.Next(room.Height);
                        width = 1;
                        height = room.Height;
                        checkRect = new Rectangle(room.Y, room.X - 1, 1, room.Height);
                        break;
                    case 3:
                        sx = room.Right;
                        sy = room.Y + GameConst.rand.Next(room.Height);
                        width = 1;
                        height = room.Height;
                        checkRect = new Rectangle(room.Y, room.Right, 1, room.Height);
                        break;
                }

                // not already a path on that side
                foreach (Rectangle r in cooridors)
                {
                    if (r.Intersects(checkRect))
                    {
                        overlapping = true;
                    }
                }

                // check out of bounds
                if (sx + width >= mwidth ||
                    sy + height >= mheight ||
                    sx < 0 ||
                    sy < 0)
                {
                    continue;
                }

                // check for path already there
                for (int x = sx; x < sx + width; x++)
                {
                    for (int y = sy; y < sy + height; y++)
                    {
                        if (map[x, y])
                        {
                            overlapping = true;
                        }
                    }
                }

                if (overlapping)
                {
                    continue;
                }

                // set x and y for entrance
                
                int w = height > 1 ? 3 : 1;
                int h = width > 1 ? 3 : 1;

                entrance = new Rectangle(sx, sy, w, h);
                completed = true;
            }
            return entrance;
        }

        public Rectangle MakeCooridor(ref bool[,] map, int mwidth, int mheight, int rwidth, int rheight)
        {
            bool completed = false;
            Rectangle hall = Rectangle.Empty;

            while (!completed)
            {
                hall = GetEntrance(map, mwidth, mheight);
                bool overlapping = false;

                // check for intersection with rooms
                foreach (Rectangle room in rooms)
                {
                    if (room.Intersects(hall))
                    { overlapping = true; }
                }

                // check for intersection with cooridors
                foreach (Rectangle coor in cooridors)
                {
                    if (coor.Intersects(hall))
                    { overlapping = true; }
                }

                // check for out of bounds
                if (hall.Right > mwidth ||
                    hall.Bottom > mheight)
                {
                    overlapping = true;
                }

                if (overlapping)
                { continue; }

                // properties of new room
                int ry = 0;
                int rx = 0;
                int w = 3 + GameConst.rand.Next(rwidth - 3);
                int h = 3 + GameConst.rand.Next(rheight - 3);

                // decide position
                if (hall.Width > 1)
                {
                    ry = GameConst.rand.Next(hall.Y, hall.Bottom);

                    if (ry < 0 ||
                        hall.X - 1< 0)
                    {
                        continue;
                    }

                    if (map[hall.X - 1, ry])
                    {
                        rx = hall.Right ;
                    }
                    else
                    {
                        rx = hall.X - w;
                    }
                }
                else
                {
                    rx = GameConst.rand.Next(hall.X, hall.Right);

                    if (rx < 0 ||
                        hall.Y - 1 < 0)
                    {
                        continue;
                    }

                    if (map[rx, hall.Y - 1])
                    {
                        ry = hall.Bottom ;
                    }
                    else
                    {
                        ry = hall.Y - h;
                    }
                }

                Rectangle newRoom = new Rectangle(rx, ry, w, h);

                // check for room outtabounds
                if (newRoom.Right > mwidth ||
                    newRoom.X < 0 ||
                    newRoom.Bottom > mheight ||
                    newRoom.Y < 0)
                {
                    continue;
                }

                //check for room overlap
                for (int x = newRoom.X; x < newRoom.Right; x++)
                {
                    for (int y = newRoom.Y; y < newRoom.Bottom; y++)
                    {
                        if (map[x, y])
                        {
                            overlapping = true;
                        }
                    }
                }

                if (overlapping)
                { continue; }

                for (int x = newRoom.X; x < newRoom.Right; x++)
                {
                    for (int y = newRoom.Y; y < newRoom.Bottom; y++)
                    {
                        map[x, y] = true;
                    }
                }

                rooms.Add(newRoom);

                completed = true;
            }


            for (int x = hall.X; x < hall.Right; x++)
            {
                for (int y = hall.Y; y < hall.Bottom; y++)
                {
                    map[x, y] = true;
                }
            }

            cooridors.Add(hall);

            return hall;
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

            int maxWidth = width / 5;
            int maxHeight = height / 5;
            int maxArea = (width * height) / 5;


            MakeRoom(ref map, width, height, maxWidth, maxHeight);

            while (rooms.Count < numRooms)
            {
                MakeCooridor(ref map, width, height, maxWidth, maxHeight);
                
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile t;
                    if (map[x, y])
                    {
                        t = new Tile(TileType.floor, x, y);
                    }
                    else
                    {
                        t = new Tile(TileType.wall, x, y);
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
