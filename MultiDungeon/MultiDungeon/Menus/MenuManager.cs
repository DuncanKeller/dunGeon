﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace MultiDungeon.Menus
{
    public class MenuManager
    {
        //List<Menu> menus = new List<Menu>();
        Menu currentMenu;

        public MainMenu main;
        public JoinServerMenu join;
        public EnterServerMenu enterServer;
        public FailedToConnectMenu failure;
        public GameLobby lobby;
        public QuickJoin quickJoin;
        public ServerSetupMenu serverSetup;
        public SettingsMenu settings;
        public EndgameMenu endgame;
        public CreditsMenu credits;

        public TeamChestMenu teamChest;
        public CapitalistMenu shop;
        public ApothecaryMenu potion;

        public MenuManager(Game1 game)
        {
            main = new MainMenu(game, this);
            join = new JoinServerMenu(game, this);
            enterServer = new EnterServerMenu(game, this);
            failure = new FailedToConnectMenu(game, this);
            lobby = new GameLobby(game, this);
            quickJoin = new QuickJoin(game, this);
            serverSetup = new ServerSetupMenu(game, this);
            endgame = new EndgameMenu(game, this);
            settings = new SettingsMenu(game, this);
            credits = new CreditsMenu(game, this);

            teamChest = new TeamChestMenu(game, this);
            shop = new CapitalistMenu(game, this);
            potion = new ApothecaryMenu(game, this);

            currentMenu = main;
        }

        public Menu CurrentMenu
        {
            get { return currentMenu; }
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
            if (currentMenu != main)
            {
                if (currentMenu == teamChest)
                {
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(0, 0,
                   GameConst.SCREEN_WIDTH, GameConst.SCREEN_HEIGHT), new Color(210, 180, 140, 210));
                }
                else
                {
                    sb.Draw(TextureManager.Map["menu-default"], new Rectangle(0, 0,
                   GameConst.SCREEN_WIDTH, GameConst.SCREEN_HEIGHT), Color.White);
                }
            }
            currentMenu.Draw(sb);
        }
    }
}
