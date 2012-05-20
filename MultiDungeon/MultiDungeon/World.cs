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
            set { players[gameId] = value; }
        }

        public static List<Player> Players
        {
            get { return new List<Player>(players.Values); }
        }

        public static Dictionary<int, Player> PlayerHash
        {
            get { return players; }
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
            map.GenerateMap(35, 35);
            Player.Spawn();
            MultiDungeon.HUD.Map.Init(World.Map);
        }


        #region NetworkClient
        public static void RecieveData(string[] datum)
        {
            foreach (string data in datum)
            {
                if (data == String.Empty)
                {
                    continue;
                }
                string[] info = data.Split("\n".ToCharArray());
                try
                {
                    if (info[0] == "id")
                    {
                        int id = Int32.Parse(info[1]);

                        gameId = id;
                        players.Add(gameId, new Mapmaker(100, 100, gameId));
                    }
                    else if (info[0] == "start")
                    {
                        StartGame();
                    }
                    else if (info[0] == "xbox")
                    {
                        
                        int controllerNum = Int32.Parse(info[1]);
                        switch (controllerNum)
                        {
                            case 0:
                                Player.playerIndex = PlayerIndex.One;
                                break;
                            case 1:
                                Player.playerIndex = PlayerIndex.Two;
                                break;
                            case 2:
                                Player.playerIndex = PlayerIndex.Three;
                                break;
                            case 3:
                                Player.playerIndex = PlayerIndex.Four;
                                break;
                        
                        }
                        

                         int playerNum = Int32.Parse(info[1]);
                         switch (playerNum)
                         {
                             case 0:
                                 //Player.playerIndex = PlayerIndex.One;
                                 Player = new Mapmaker(Player.Position.X, Player.Position.Y, Player.ID);
                                 break;
                             case 1:
                                 //Player.playerIndex = PlayerIndex.Two;
                                 Player = new Ninja(Player.Position.X, Player.Position.Y, Player.ID);
                                 break;
                             case 2:
                                 //Player.playerIndex = PlayerIndex.Three;
                                 Player = new Mapmaker(Player.Position.X, Player.Position.Y, Player.ID);
                                 break;
                             case 3:
                                 //Player.playerIndex = PlayerIndex.Four;
                                 Player = new Ninja(Player.Position.X, Player.Position.Y, Player.ID);
                                 break;
                         }
                    }
                    else if (info[0] == "team")
                    {
                        int id = Int32.Parse(info[1]);
                        int team = Int32.Parse(info[2]);
                        PlayerHash[id].Init(team);
                    }
                    else if (info[0] == "reset")
                    {
                        foreach (int key in players.Keys)
                        {
                            players[key].Spawn();
                            players[key].Gold = 0;
                            players[key].Item = null;
                        }
                        itemManager.RemoveChests();
                        map.Populate();
                    }
                    else if (info[0] == "disconnect")
                    {
                        lock (players)
                        {
                            int id = Int32.Parse(info[1]);
                            players.Remove(id);
                        }
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
                            lock (players)
                            {
                                players.Add(id, new Mapmaker(float.Parse(info[2]), float.Parse(info[3]), id));
                            }
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
                            b.Init(p, players[id].Angle - (float)(Math.PI / 2), damage, id);
                            bulletManager.Add(b);
                        }
                    }
                    else if (info[0] == "chest")
                    {
                        int id = Int32.Parse(info[1]);
                        itemManager.OpenChest(id);
                    }
                    else if (info[0] == "rand")
                    {
                        int seed = Int32.Parse(info[1]);
                        GameConst.rand = new Random(seed);
                    }
                    else
                    {
                        throw new Exception("command not found");
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message, MessageType.urgent);
                }

                // print all non-position commands to the console
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
            if (players.ContainsKey(gameId))
            {
                
                Client.Send("p" + "\n" + gameId.ToString() + "\n" + ((int)players[gameId].Position.X).ToString()
                + "\n" + ((int)players[gameId].Position.Y).ToString() + "\n" + 
                (players[gameId].Angle).ToString());
                timer = 0; 
            }
        }

        public static void Update(float deltaTime)
        {
            UpdateNetwork(deltaTime);
            bulletManager.Update(map);
            itemManager.Update();

            lock (players)
            {
                foreach (var v in players)
                {
                    Player p = v.Value;
                    p.Update(deltaTime);
                    p.UpdateCollisions(map.GetTilesNear((int)p.Position.X, (int)p.Position.Y));
                }
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
            //DrawWallTiles(sb);
            itemManager.Draw(sb);

            foreach (var p in players)
            {
                p.Value.Draw(sb);
            }

            bulletManager.Draw(sb);
            
        }

    }
}
