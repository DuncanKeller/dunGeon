using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon
{
    class World
    {
        public static Random rand = new Random();
        public static int gameId;
        static Dictionary<int, Player> players = new Dictionary<int, Player>();
        static TileSet map;

        public static void Init()
        {
            

            map = new TileSet();

            InitNetwork();
        }

        public static void RecieveData(string data)
        {
            string[] info = data.Split("\n".ToCharArray());
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
                if (players.ContainsKey(Int32.Parse(info[0])))
                {
                    if (Int32.Parse(info[0]) != gameId)
                    {
                        players[Int32.Parse(info[0])].SetPos(float.Parse(info[1]), float.Parse(info[2]));
                    }
                }
                else
                {
                    players.Add(Int32.Parse(info[0]), new Player(float.Parse(info[1]), float.Parse(info[2]), Int32.Parse(info[0])));
                }
            }
        }

        public static void InitNetwork()
        {
            ServerClient.Connect();
        }

        public static void UpdateNetwork()
        {
            
        }

        public static void Update(float deltaTime)
        {
            UpdateNetwork();

            foreach (var v in players)
            {
                Player p = v.Value;
                p.Update(deltaTime);
                if (ServerClient.connected)
                {
                    ServerClient.Send(p.ID + "\n" + p.Position.X + "\n" + p.Position.Y);
                }
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
