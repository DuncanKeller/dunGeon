using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetworkLibrary;

namespace DungeonServer
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer server = new GameServer();
            server.Start();

            while (true)
            {
                string command = Console.ReadLine();

                if (command.Equals("new seed", StringComparison.CurrentCultureIgnoreCase))
                {
                    server.GenerateSeed();
                    Console.WriteLine(server.Seed);
                }
                else if (command.Equals("start game", StringComparison.CurrentCultureIgnoreCase))
                {
                    server.StartGame();
                }
            }
        }
    }
}
