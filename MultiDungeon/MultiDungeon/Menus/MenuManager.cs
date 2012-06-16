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
        MainMenu main;

        public MenuManager(Game1 game)
        {
            main = new MainMenu(game);
        }

        public void Update()
        {
            main.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            main.Draw(sb);
        }
    }
}
