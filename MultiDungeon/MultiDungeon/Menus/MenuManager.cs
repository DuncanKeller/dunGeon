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

        public MainMenu main;

        public MenuManager(Game1 game)
        {
            main = new MainMenu(game, this);

            menus.Add(main);
            currentMenu = main;
        }

        public void Update()
        {
            currentMenu.Update();
        }

        public void SwitchMenu(Menu m)
        {
            currentMenu = m;
        }

        public void Draw(SpriteBatch sb)
        {
            currentMenu.Draw(sb);
        }
    }
}
