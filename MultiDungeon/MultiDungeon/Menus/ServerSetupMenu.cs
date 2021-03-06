﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using NetworkLibrary;

namespace MultiDungeon.Menus
{
    public class ServerSetupMenu : Menu
    {
        int gametype = 0;
        int players = 1;
        string map = "random";
        GameServer server = null;

        public GameServer Server
        {
            get { return server; }
            set { server = value; }
        }

        public ServerSetupMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddFlavorItem("Game Type", new Vector2(1, 6));
            AddFlavorItem("_________", new Vector2(1, 7));
            AddMenuItem("Standard", new Vector2(1, 8), 0,
                delegate() { gametype = 0; ActivateItem(0, 0); });
            
            AddFlavorItem("Number of Players", new Vector2(6, 6));
            AddFlavorItem("_________", new Vector2(6, 7));
            AddMenuItem("2", new Vector2(6, 8), 1,
                delegate() { players = 2; ActivateItem(0, 1); });
            AddMenuItem("3", new Vector2(6, 9), 1,
                delegate() { players = 3; ActivateItem(1, 1); });
            AddMenuItem("4", new Vector2(6, 10), 1,
                delegate() { players = 4; ActivateItem(2, 1); });
            AddMenuItem("5", new Vector2(6, 11), 1,
                delegate() { players = 5; ActivateItem(3, 1); });
            AddMenuItem("6", new Vector2(6, 12), 1,
                delegate() { players = 6; ActivateItem(4, 1); });

            AddFlavorItem("Map", new Vector2(13, 6));
            AddFlavorItem("_________", new Vector2(13, 7));
            AddMenuItem("Random", new Vector2(13, 8), 2,
                delegate() { map = "random"; ActivateItem(0, 2); });
            AddMenuItem("Raceways", new Vector2(13, 9), 2,
                            delegate() { map = "raceways"; ActivateItem(1, 2); });
            AddMenuItem("Catacombs", new Vector2(13, 10), 2,
                delegate() { map = "catacombs"; ActivateItem(2, 2); });
            AddMenuItem("Piety", new Vector2(13, 11), 2,
                delegate() { map = "piety"; ActivateItem(3, 2); });

            AddMenuItem("START\nSERVER", new Vector2(17, 8), 3,
               delegate() { StartServer();  });
            
        }

        

        public override void BackOut()
        {
            base.BackOut();
            menuManager.SwitchMenu(menuManager.main);
        }

        public void StartServer()
        {
            server = new GameServer();
            //start server
            Thread serverThread = new Thread(server.Start);
            server.InitGame(players, map, gametype);
            serverThread.Start();
            Thread.Sleep(200);

            // connect to it
            if (World.InitNetwork("127.0.0.1"))
            {
                Console.Write("Connected to Server", MessageType.special);
                menuManager.SwitchMenu(menuManager.lobby);
            }
            else
            {
                menuManager.SwitchMenu(menuManager.failure);
            }
        }

        public override void Init()
        {
            yIndex = 0; xIndex = 0;
            menuItems[0][0].Select();
            base.Init();
            //Thread.Sleep(150);
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
