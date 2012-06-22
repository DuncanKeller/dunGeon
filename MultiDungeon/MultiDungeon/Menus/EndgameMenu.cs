using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Menus
{
    public class EndgameMenu : Menu
    {
        int winningTeam;

        public EndgameMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddFlavorItem("win/lose", new Vector2(1, 5));

            AddMenuItem("Return to Lobby", new Vector2(1, 15), 0,
                delegate() { menuManager.SwitchMenu(menuManager.lobby); });
            AddMenuItem("Exit to Menu", new Vector2(1, 16), 0,
                delegate() { menuManager.SwitchMenu(menuManager.main); World.Disconnect(); });
        }

        public override void Init()
        {
            string victory = winningTeam == World.Player.Team ? "VICTORY!\n YOU ARE A DISTINGUISHED CHAMPION OF THE DUNGEON,\n AND THE ENVY OF KINGS!"
                : "FAILURE\n YOU HAVE LET DOWN THE GODS OF FORTUNE,\n NOW BATHE IN THEIR WRATH!";
            nonSelectableItems[0].ChangeText(victory);
            yIndex = 0; xIndex = 0;
            menuItems[0][0].Select();
            base.Init();
        }

        public void SetTeam(int t)
        {
            winningTeam = t;
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
