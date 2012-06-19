﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace MultiDungeon.Menus
{
    class MenuManager
    {
        //List<Menu> menus = new List<Menu>();
        Menu currentMenu;

        public MainMenu main;
        public JoinServerMenu join;
        public EnterServerMenu enterServer;
        public FailedToConnectMenu failure;
        public GameLobby lobby;

        public MenuManager(Game1 game)
        {
            main = new MainMenu(game, this);
            join = new JoinServerMenu(game, this);
            enterServer = new EnterServerMenu(game, this);
            failure = new FailedToConnectMenu(game, this);
            lobby = new GameLobby(game, this);

            currentMenu = main;
        }

        public void Update()
        {
            currentMenu.Update();
        }

        public void SwitchMenu(Menu m)
        {
            currentMenu = m;
            currentMenu.Init();
            Thread.Sleep(200);
        }

        public void Draw(SpriteBatch sb)
        {
            currentMenu.Draw(sb);
        }
    }
}
