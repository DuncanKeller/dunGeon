using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Items;

namespace MultiDungeon.Map
{
    public class TileSet
    {
        List<Tile> tiles = new List<Tile>();
        Tile[,] tileMap;

        List<Rectangle> rooms = new List<Rectangle>();
        List<Rectangle> cooridors = new List<Rectangle>();

        int numRooms = 12;

        int width;
        int height;

        List<Rectangle> teamRooms = new List<Rectangle>();
        ColorScheme colorScheme;

        public TileSet()
        {
            
        }

        public Tile this[int x, int y]
        {
            get
            {
                return tileMap[x, y];
            }
        }

        public Rectangle GetTeamRoom(int teamNum)
        {
            return teamRooms[teamNum];
        }

        public List<Tile> Tiles
        {
            get { return tiles; }
        }

        public int Width
        {
            get { return width * Tile.TILE_SIZE; }
        }

        public int Height
        {
            get { return height * Tile.TILE_SIZE; }
        }

        public int WidthIndex
        {
            get { return width; }
        }

        public int HeightIndex
        {
            get { return height; }
        }

        public void Reset()
        {
            teamRooms.Clear();
            rooms.Clear();
            cooridors.Clear();
            tiles.Clear();
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
  
        #region DungeonGeneration
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
                if (room.Right > mwidth - 1 ||
                    room.Bottom > mheight - 1 ||
                    room.X < 1 ||
                    room.Y < 1)
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
                        break;
                    }
                }

                // check out of bounds
                if (sx + width >= mwidth - 1 ||
                    sy + height >= mheight - 1 ||
                    sx < 1 ||
                    sy < 1)
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
                            break;
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
                    { overlapping = true; break; }
                }

                // check for intersection with cooridors
                foreach (Rectangle coor in cooridors)
                {
                    if (coor.Intersects(hall))
                    { overlapping = true; break; }
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

                    if (ry < 1 ||
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

                    if (rx < 1 ||
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
                if (newRoom.Right > mwidth - 1 ||
                    newRoom.X < 1 ||
                    newRoom.Bottom > mheight - 1 ||
                    newRoom.Y < 1)
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
                            break;
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
        #endregion

        public void Populate()
        {
            int numItems = rooms.Count / 2;
            List<int> usedRooms = new List<int>();

            for (int i = 0; i < numItems; i++)
            {
                int roomId = GameConst.rand.Next(numItems);
                while (usedRooms.Contains(roomId)
                    || rooms[roomId] == teamRooms[0]
                     || rooms[roomId] == teamRooms[1])
                {
                    roomId = GameConst.rand.Next(rooms.Count);
                }
                usedRooms.Add(roomId);

                int x = GameConst.rand.Next(rooms[roomId].Width - 1)
                    * Tile.TILE_SIZE + (rooms[roomId].X * Tile.TILE_SIZE);
                int y = GameConst.rand.Next(rooms[roomId].Height - 1)
                     * Tile.TILE_SIZE + (rooms[roomId].Y * Tile.TILE_SIZE);

                Vector2 chestPos = new Vector2(x,y);

                Item item = null;
                switch (GameConst.rand.Next(5))
                {
                    case 0:
                        item = new HealthPotion(HealingLevel.strong);
                        break;
                    case 1:
                        item = new CoinPurse(100);
                        break;
                    case 2:
                        item = new Juice();
                        break;
                    case 3:
                        item = new Stoneskin();
                        break;
                    case 4:
                        item = new SeeingEye();
                        break;
                }

                World.ItemManager.Chests.Add(new Chest(i, chestPos, item));
            }
        }

        public void GenerateMap(int width, int height)
        {
            lock (tiles)
            {
                tiles.Clear();

                bool[,] map = new bool[width, height];

                this.width = width - 1;
                this.height = height - 1;

                for (int x = 1; x < width; x++)
                {
                    for (int y = 1; y < height; y++)
                    {
                        map[x, y] = false;
                    }
                }

                int maxWidth = width / 6;
                int maxHeight = height / 6;
                int maxArea = (width * height) / 5;


                MakeRoom(ref map, width, height, maxWidth, maxHeight);

                while (rooms.Count < numRooms)
                {
                    MakeCooridor(ref map, width, height, maxWidth, maxHeight);
                }

                GenerateTeamRooms();
                Populate();
                CreateTiles(map, width, height);
                InitColorPalets();
            }
        }

        private void GenerateTeamRooms()
        {
            Rectangle room1 = Rectangle.Empty;
            Rectangle room2 = Rectangle.Empty; ;
            float maxDist = 0;

            foreach (Rectangle r1 in rooms)
            {
                foreach (Rectangle r2 in rooms)
                {
                    Vector2 v1 = new Vector2(r1.Center.X, r1.Center.Y);
                    Vector2 v2 = new Vector2(r2.Center.X, r2.Center.Y);
                    float dist = Vector2.Distance(v1, v2);
                    if (dist > maxDist)
                    {
                        maxDist = dist;
                        room1 = r1;
                        room2 = r2;
                    }
                }
            }

            if (room1 == Rectangle.Empty ||
                room2 == Rectangle.Empty)
            {
                throw new Exception("Team rooms not found");
            }
            teamRooms.Add(room1);
            teamRooms.Add(room2);

            int x1 = GameConst.rand.Next(room1.Width - 2)
                    * Tile.TILE_SIZE + (room1.X * Tile.TILE_SIZE);
            int y1 = GameConst.rand.Next(room1.Height - 2)
                    * Tile.TILE_SIZE + (room1.Y * Tile.TILE_SIZE);
            int x2 = GameConst.rand.Next(room2.Width - 2)
                    * Tile.TILE_SIZE + (room2.X * Tile.TILE_SIZE);
            int y2 = GameConst.rand.Next(room2.Height - 2)
                    * Tile.TILE_SIZE + (room2.Y * Tile.TILE_SIZE);

            World.ItemManager.Add(new TeamChest(0, new Vector2(x1, y1)));
            World.ItemManager.Add(new TeamChest(1, new Vector2(x2, y2)));
        }

        private void CreateTiles(bool[,] map, int width, int height)
        {
            tileMap = new Tile[width, height];
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
                    tileMap[x, y] = t;
                }
            }
        }

        public void InitColorPalets()
        {
            colorScheme.floors = new List<Color>();
            colorScheme.walls = new List<Color>();

            colorScheme.floors.Add(Color.DarkSlateBlue);
            colorScheme.walls.Add(Color.DarkSlateBlue);

            colorScheme.floors.Add(Color.DarkSlateGray);
            colorScheme.walls.Add(Color.ForestGreen);

            colorScheme.floors.Add(Color.SandyBrown);
            colorScheme.walls.Add(Color.Orange);

            GetColorScheme();
        }

        public void GetColorScheme()
        {
            int rand = GameConst.rand.Next(colorScheme.walls.Count);
            colorScheme.wall = colorScheme.walls[rand];
            colorScheme.floor = colorScheme.floors[rand];
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Tile t in tiles)
            {
                t.Draw(sb, colorScheme);
            }
        }

        public void DrawGround(SpriteBatch sb)
        {
            foreach (Tile t in tiles)
            {
                if (t.Type == TileType.floor)
                {
                    t.Draw(sb, colorScheme);
                }
            }
        }

        public void DrawWalls(SpriteBatch sb)
        {
            foreach (Tile t in tiles)
            {
                if (t.Type == TileType.wall)
                {
                    t.Draw(sb, colorScheme);
                }
            }
        }

    }
}
