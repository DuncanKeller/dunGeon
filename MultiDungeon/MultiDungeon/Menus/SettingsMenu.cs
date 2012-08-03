using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Graphics;

namespace MultiDungeon.Menus
{
    public class SettingsMenu : Menu
    {
        int newW;
        int newH;

        public SettingsMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddFlavorItem("Game Settings", new Vector2(1, 1));

            AddFlavorItem("Tooltips", new Vector2(2, 6));
            AddFlavorItem("_______", new Vector2(2, 7));
            AddMenuItem("On", new Vector2(2, 8), 0,
                delegate() { Activate(0, 0); Tooltip.activated = true; });
            AddMenuItem("Off", new Vector2(2, 9), 0,
                delegate() { Activate(0, 1); Tooltip.activated = false; });

            AddFlavorItem("Window\r\nSize", new Vector2(6, 6));
            AddFlavorItem("______", new Vector2(6, 7));
            AddMenuItem("Large", new Vector2(6, 8), 1,
                delegate() { Activate(1, 0); newW = 800; newH = 800; });
            AddMenuItem("Small", new Vector2(6, 9), 1,
                delegate() { Activate(1, 1); newW = 600; newH = 600; });

            AddFlavorItem("Effects", new Vector2(9, 6));
            AddFlavorItem("______", new Vector2(9, 7));
            AddMenuItem("High", new Vector2(9, 8), 2,
                delegate() { Activate(2, 0); GameConst.effectSetting = 2; });
            AddMenuItem("Med", new Vector2(9, 9), 2,
                delegate() { Activate(2, 1); GameConst.effectSetting = 1; });
            AddMenuItem("Low", new Vector2(9, 10), 2,
              delegate() { Activate(2, 2); GameConst.effectSetting = 0; });


            AddMenuItem("Return to menu", new Vector2(1, 16), 0,
                delegate() { BackOut(); });
        }

        private void InitItems()
        {
            foreach (List<MenuItem> items in menuItems)
            {
                foreach (MenuItem i in items)
                {
                    i.Activated = false;
                }
            }

            switch (GameConst.effectSetting)
            {
                case 0:
                    menuItems[2][2].Activated = true;
                    break;
                case 1:
                    menuItems[2][1].Activated = true;
                    break;
                case 2:
                    menuItems[2][0].Activated = true;
                    break;
            }

            if (GameConst.SCREEN_WIDTH == 800)
            {
                menuItems[1][0].Activated = true;
            }
            else
            {
                menuItems[1][1].Activated = true;
            }

            if (Tooltip.activated)
            {
                menuItems[0][0].Activated = true;
            }
            else
            {
                menuItems[0][1].Activated = true;
            }
        }

        private void Activate(int x, int y)
        {
            foreach (MenuItem i in menuItems[x])
            {
                i.Activated = false;
            }
            menuItems[x][y].Activated = true;
        }

        public override void Init()
        {
            newW = GameConst.SCREEN_WIDTH;
            newH = GameConst.SCREEN_HEIGHT;
            InitItems();
            yIndex = 0; xIndex = 0;
            menuItems[0][0].Select();
            base.Init();
        }

        public override void BackOut()
        {
            if (newW != GameConst.SCREEN_WIDTH ||
                newH != GameConst.SCREEN_HEIGHT)
            {
                GameConst.SCREEN_WIDTH = newW;
                GameConst.SCREEN_HEIGHT = newH;
                game.Graphics.PreferredBackBufferWidth = newW;
                game.Graphics.PreferredBackBufferHeight = newH;
                game.Graphics.ApplyChanges();
                Graphics.Shadowmap.Init(game, game.Graphics.GraphicsDevice, game.Content);
            }

            menuManager.SwitchMenu(menuManager.main);
            base.BackOut();
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
