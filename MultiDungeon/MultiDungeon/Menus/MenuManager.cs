using System;
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
        public EndgameMenu endgame;

        public TeamChestMenu teamChest;

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

            teamChest = new TeamChestMenu(game, this);

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
            if (currentMenu != main)
            {
                if (currentMenu == teamChest)
                {
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(0, 0,
                   GameConst.SCREEN_WIDTH, GameConst.SCREEN_HEIGHT), new Color(210,180,140,100));
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
