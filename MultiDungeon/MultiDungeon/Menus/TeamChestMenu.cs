using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MultiDungeon.Menus
{
    public class TeamChestMenu : Menu
    {
        Player player = null;

        public TeamChestMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddFlavorItem("YOU", new Vector2(3, 5));
            AddFlavorItem("000", new Vector2(3, 6));

            AddMenuItem(">", new Vector2(10, 6), 0,
                delegate() { AddGold();  });

            AddFlavorItem("TEAM", new Vector2(15, 5));
            AddFlavorItem("000", new Vector2(15, 6));

            AddFlavorItem("Press A to add 50 GOLD to Team chest", new Vector2(1, 15));
            AddFlavorItem("Press B to return", new Vector2(1, 18));


            menuItems[0][0].Select();
        }

        public void AddGold()
        {
            if (player != null)
            {
                int send = player.Gold >= 50 ? 50 : player.Gold;
                player.Gold -= send;
                SoundManager.PlaySound("coins");
                Client.Send("teamgold\n" + player.Team + "\n" + send );
            }
            UpdateGoldAmnt();
        }

        public override void BackOut()
        {
            Close();
            World.inMenu = false;
        }

        public void Open(Player p)
        {
            player = p;
        }

        public void Close()
        {
            player = null;
        }

        public void UpdateGoldAmnt()
        {
            nonSelectableItems[1].ChangeText(player.Gold.ToString());
            nonSelectableItems[3].ChangeText(World.endgame.teamGold[player.Team].ToString());
        }

        public override void Init()
        {
            UpdateGoldAmnt();
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
