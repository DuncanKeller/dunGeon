using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;
using System.Timers;
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
        static Camera cam;

        public static BulletManager BulletManager
        {
            get { return bulletManager; }
        }

        public static void Init(GraphicsDeviceManager g)
        {
            map = new TileSet();
            InitNetwork();
            cam = new Camera(g);
        }

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
                            b.Init(players[id].Position, players[id].Angle - (float)(Math.PI / 2));
                            bulletManager.Add(b);
                        }
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

            cam.pos = players[gameId].Position;
          
        }

        public static void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.Default, RasterizerState.CullNone, null, cam.getTransformation());

            map.Draw(sb);

            foreach (var p in players)
            {
                p.Value.Draw(sb);
            }

            bulletManager.Draw(sb);

            sb.End();
        }
    }
}
