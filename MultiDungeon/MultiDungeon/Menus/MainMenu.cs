using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MultiDungeon.Menus
{
    class MainMenu : Menu
    {    
        public MainMenu(Game1 g) : base(g)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddMenuItem("Start Server", new Vector2(1, 15),
                delegate() {  });
            AddMenuItem("Join Server", new Vector2(1, 16),
                delegate() {  });
            AddMenuItem("Game Settings", new Vector2(1, 17),
                delegate() {  });
            AddMenuItem("Quit", new Vector2(1, 18),
                delegate() { game.Exit(); });

            menuItems[0].Select();
        }

        public void Update()
        {
            base.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
