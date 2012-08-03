using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Graphics;

namespace MultiDungeon.Menus
{
    public class CreditsMenu : Menu
    {
        public CreditsMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddFlavorItem("Game Settings", new Vector2(1, 1));

            AddFlavorItem("Featuring the Musical Stylings of", new Vector2(2, 3));
            AddFlavorItem("Somebody", new Vector2(4, 4));

            AddFlavorItem("The Few, the Brave, the Playtesters", new Vector2(2, 7));
            AddFlavorItem("Christian Beekman", new Vector2(4, 8));
            AddFlavorItem("Nadeem Persico-Shammas", new Vector2(4, 9));
            AddFlavorItem("Jackson Keller", new Vector2(4, 10));
            AddFlavorItem("Samuel Borchart", new Vector2(4, 11));
            AddFlavorItem("Jacob Miller", new Vector2(4, 12));
   
            AddMenuItem("Return to menu", new Vector2(1, 16), 0,
                delegate() { BackOut(); });
        }

     
        public override void Init()
        {
            yIndex = 0; xIndex = 0;
            menuItems[0][0].Select();
            base.Init();
        }

        public override void BackOut()
        {
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
