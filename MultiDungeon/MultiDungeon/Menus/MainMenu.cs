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
        public MainMenu(Game1 g, MenuManager mm) : base(g, mm)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddMenuItem("Start Server", new Vector2(1, 15), 0,
                delegate() { menuManager.SwitchMenu(menuManager.serverSetup); });
            AddMenuItem("Join Server", new Vector2(1, 16), 0,
                delegate() { menuManager.SwitchMenu(menuManager.join); });
            AddMenuItem("Game Settings", new Vector2(1, 17), 0,
                delegate() { });
            AddMenuItem("Quit", new Vector2(1, 18), 0,
                delegate() { game.Exit(); });

            menuItems[0][0].Select();
        }


        public override void Init()
        {
            yIndex = 0; xIndex = 0;
            menuItems[0][0].Select();
            base.Init();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(TextureManager.Map["menu-main"], new Rectangle(0, 0,
                GameConst.SCREEN_WIDTH, GameConst.SCREEN_HEIGHT), Color.White);
            base.Draw(sb);

            sb.DrawString(TextureManager.Fonts["console"], "dunGeon",
                new Vector2(GameConst.SCREEN_WIDTH - (TextureManager.Fonts["console"].MeasureString("dunGeon").X)
                    - (GameConst.SCREEN_WIDTH / 20), GameConst.SCREEN_HEIGHT / 30), new Color(70, 70, 70));
        }
    }
}
