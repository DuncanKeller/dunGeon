using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Menus
{
    public class FailedToConnectMenu : Menu
    {
        public FailedToConnectMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddFlavorItem("Uh Oh! Looks like I was unable to \nconnect to the server! :(\nSorry, man!", 
                new Vector2(1, 2));
            AddMenuItem("OK", new Vector2(1, 15), 0,
                delegate() { menuManager.SwitchMenu(menuManager.main); } );
        }

        public override void BackOut()
        {
            base.BackOut();
            menuManager.SwitchMenu(menuManager.main);
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
            base.Draw(sb);
        }
    }
}
