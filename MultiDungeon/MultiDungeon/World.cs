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

        public static void Init()
        {
            map = new TileSet();
            InitNetwork();
        }

        public static void RecieveData(string data)
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
                else
                {
                    int id = Int32.Parse(info[1]);
                    if (players.ContainsKey(id))
                    {
                        if (id != gameId)
                        {
                            players[id].SetPos(float.Parse(info[2]), float.Parse(info[3]));
                        }
                    }
                    else
                    {
                        players.Add(id, new Player(float.Parse(info[2]), float.Parse(info[3]), id));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void InitNetwork()
        {
            ServerClient.Connect();
        }

        public static void UpdateNetwork(float deltaTime)
        {
            timer++;
            if (timer > 5)
            {
                ServerClient.Send("position" + "\n" + gameId.ToString() + "\n" + ((int)players[gameId].Position.X).ToString()
                + "\n" + ((int)players[gameId].Position.Y).ToString());
                timer = 0;
            }
            
            //ServerClient.SendPosition(gameId, players[gameId].Position.X, players[gameId].Position.Y);
            
        }

        public static void Update(float deltaTime)
        {
            UpdateNetwork(deltaTime);

            foreach (var v in players)
            {
                Player p = v.Value;
                p.Update(deltaTime);
                p.UpdateCollisions(map.GetTilesNear((int)p.Position.X, (int)p.Position.Y));
            } 
          
        }

        public static void Draw(SpriteBatch sb)
        {
            map.Draw(sb);

            foreach (var p in players)
            {
                p.Value.Draw(sb);
            }
        }
    }
}
