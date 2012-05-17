using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;
using System.Timers;
using MultiDungeon.Items;

namespace MultiDungeon
{
    class World
    {
        public static Random rand = new Random();
        public static int gameId;
        static Dictionary<int, Player> players = new Dictionary<int, Player>();
        static TileSet map;
        static int timer = 0;
        static BulletManager bulletManager = new BulletManager();
        static ItemManager itemManager = new ItemManager();
        static Camera cam;

        public static BulletManager BulletManager
        {
            get { return bulletManager; }
        }

        public static ItemManager ItemManager
        {
            get { return itemManager; }
        }

        public static Camera Camera
        {
            get { return cam; }
        }

        public static Player Player
        {
            get { return players[gameId]; }
        }

        public static List<Player> Players
        {
            get { return new List<Player>(players.Values); }
        }

        public static TileSet Map
        {
            get { return map; }
        }

        public static void Init(GraphicsDeviceManager g)
        {
            InitNetwork();
            map = new TileSet();
            cam = new Camera(g);
        }


        public static void StartGame()
        {
            map.GenerateMap(30, 30);
        }


        #region NetworkClient
        public static void RecieveData(string[] datum)
        {
            foreach (string data in datum)
            {
                string[] info = data.Split("\n".ToCharArray());
                try
                {
                    if (info[0] == "id")
                    {
                        int id = Int32.Parse(info[1]);

                        gameId = id;
                        players.Add(gameId, new Player((float)rand.NextDouble() * 700f,
                        (float)rand.NextDouble() * 400f, gameId));
                        players[gameId].c = new Color(rand.Next(255), rand.Next(255), rand.Next(255));
                    }
                    else if (info[0] == "disconnect")
                    {
                        int id = Int32.Parse(info[1]);
                        players.Remove(id);
                    }
                    else if (info[0] == "p")
                    {
                        int id = Int32.Parse(info[1]);
                        if (players.ContainsKey(id))
                        {
                            if (id != gameId)
                            {
                                players[id].SetPos(int.Parse(info[2]), int.Parse(info[3]));
                                players[id].Angle = float.Parse(info[4]);
                            }
                        }
                        else
                        {
                            players.Add(id, new Player(float.Parse(info[2]), float.Parse(info[3]), id));
                        }
                    }
                    else if (info[0] == "b")
                    {
                        int id = Int32.Parse(info[1]);
                        if (gameId != id)
                        {
                            Bullet b = new Bullet();
                            Vector2 p = new Vector2(players[id].DrawRect.X, players[id].DrawRect.Y);
                            double damage = Double.Parse(info[2]);
                            b.Init(p, players[id].Angle - (float)(Math.PI / 2), damage);
                            bulletManager.Add(b);
                        }
                    }
                    else if (info[0] == "rand")
                    {
                        int seed = Int32.Parse(info[1]);
                        GameConst.rand = new Random(seed);
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message, MessageType.urgent);
                }

                if (info[0] != "p")
                {
                    string message = "";

                    for (int i = 0; i < info.Length; i++)
                    {
                        message += info[i];
                        if (i < info.Length - 1)
                        {
                            message += ", ";
                        }
                    }
                    Console.Write(message);
                }
            }
        }
        #endregion

        public static void InitNetwork()
        {
            Client.Connect();
        }

        public static void UpdateNetwork(float deltaTime)
        {
            timer++;
            //if (timer > 2)
            {
                
                Client.Send("p" + "\n" + gameId.ToString() + "\n" + ((int)players[gameId].Position.X).ToString()
                + "\n" + ((int)players[gameId].Position.Y).ToString() + "\n" + 
                (players[gameId].Angle).ToString());
                timer = 0;
                 
            }

            //ServerClient.SendPosition(gameId, (int)players[gameId].Position.X, (int)players[gameId].Position.Y, (double)players[gameId].Angle);
            
        }

        public static void Update(float deltaTime)
        {
            UpdateNetwork(deltaTime);
            bulletManager.Update(map);

            foreach (var v in players)
            {
                Player p = v.Value;
                p.Update(deltaTime);
                p.UpdateCollisions(map.GetTilesNear((int)p.Position.X, (int)p.Position.Y));
            }

            // camera
            UpdateCamera();

        }

        public static void UpdateCamera()
        {
            cam.pos = players[gameId].Position;
            /*
            if (players[gameId].Position.X > GameConst.SCREEN_WIDTH / 2 &&
                players[gameId].Position.X < map.Width - (GameConst.SCREEN_WIDTH / 2))
            {
                cam.pos.X = players[gameId].Position.X;
            }
            else if (players[gameId].Position.X <= GameConst.SCREEN_WIDTH / 2)
            {
                cam.pos.X = GameConst.SCREEN_WIDTH / 2;
            }
            else if (players[gameId].Position.X >= map.Width - (GameConst.SCREEN_WIDTH / 2))
            {
                cam.pos.X = map.Width - (GameConst.SCREEN_WIDTH / 2);
            }

            if (players[gameId].Position.Y > GameConst.SCREEN_HEIGHT / 2 &&
                players[gameId].Position.Y < map.Height - (GameConst.SCREEN_HEIGHT / 2))
            {
                cam.pos.Y = players[gameId].Position.Y;
            }
            else if (players[gameId].Position.Y <= GameConst.SCREEN_HEIGHT / 2)
            {
                cam.pos.Y = GameConst.SCREEN_HEIGHT / 2;
            }
            else if (players[gameId].Position.Y >= map.Height - (GameConst.SCREEN_HEIGHT / 2))
            {
                cam.pos.Y = map.Height - (GameConst.SCREEN_HEIGHT / 2);
            }
             */
        }

        public static void DrawGroundTiles(SpriteBatch sb)
        {
            map.DrawGround(sb);
        }

        public static void DrawWallTiles(SpriteBatch sb)
        {
            map.DrawWalls(sb);
        }

        public static void DrawScene(SpriteBatch sb)
        {
            DrawWallTiles(sb);

            itemManager.Draw(sb);

            foreach (var p in players)
            {
                p.Value.Draw(sb);
            }

            bulletManager.Draw(sb);
            
        }

       
    }
}
