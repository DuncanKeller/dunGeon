using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Menus
{
    class MenuManager
    {
        List<Menu> menus = new List<Menu>();
        Menu currentMenu;

        MainMenu main;

        public MenuManager(Game1 game)
        {
            main = new MainMenu(game);

            menus.Add(main);
            currentMenu = main;
        }

        public void Update()
        {
            currentMenu.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            currentMenu.Draw(sb);
        }
    }
}
