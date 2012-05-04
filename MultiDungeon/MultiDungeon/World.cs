using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class World
    {
        public static Random rand = new Random();
        public static int gameId;
        static Dictionary<int, Player> players = new Dictionary<int, Player>();

        public static void Init()
        {
            gameId = rand.Next(999);
            ServerClient.Connect();
            players.Add(gameId, new Player(gameId, (float)rand.NextDouble() * 700f,
                (float)rand.NextDouble() * 400f));
            players[gameId].c = new Color(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        public static void RecieveData(string data)
        {
            string[] info = data.Split("\n".ToCharArray());
            if (players.ContainsKey(Int32.Parse(info[0])))
            {
                if (Int32.Parse(info[0]) != gameId)
                {
                    players[Int32.Parse(info[0])].SetPos(float.Parse(info[1]), float.Parse(info[2]));
                }
            }
            else
            {
                players.Add(Int32.Parse(info[0]), new Player(Int32.Parse(info[0]),
                    float.Parse(info[1]), float.Parse(info[2])));
            }
        }

        public static void Update(float deltaTime)
        {
            foreach (var v in players)
            {
                Player p = v.Value;
                p.Update(deltaTime);
                ServerClient.Send(p.ID + "\n" + p.Position.X + "\n" + p.Position.Y);
            } 
          
        }

        public static void Draw(SpriteBatch sb)
        {
            foreach (var p in players)
            {
                p.Value.Draw(sb);
            }
        }
    }
}
