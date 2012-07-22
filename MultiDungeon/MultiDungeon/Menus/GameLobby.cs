using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace MultiDungeon.Menus
{
    public class GameLobby : Menu
    {
        string classType = "mapmaker";
        int team = 0;
        bool ready = false;

        public GameLobby(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddFlavorItem("Class Type", new Vector2(1, 6));
            AddFlavorItem("_________", new Vector2(1, 7));
            AddMenuItem("Mapmaker", new Vector2(1, 8), 0,
                delegate() { classType = "mapmaker"; SendClass(); ActivateItem(0, 0); });
            AddMenuItem("Ninja", new Vector2(1, 9), 0,
                delegate() { classType = "ninja"; SendClass(); ActivateItem(1, 0); });
            AddMenuItem("Powdermonkey", new Vector2(1, 10), 0,
                delegate() { classType = "powdermonkey"; SendClass(); ActivateItem(2, 0); });
            AddMenuItem("Capitalist", new Vector2(1, 11), 0,
                delegate() { classType = "capitalist"; SendClass(); ActivateItem(3, 0); });
            AddMenuItem("Apothecary", new Vector2(1, 12), 0,
                delegate() { classType = "apothecary"; SendClass(); ActivateItem(4, 0); });
            AddMenuItem("Mystic", new Vector2(1, 13), 0,
               delegate() { classType = "mystic"; SendClass(); ActivateItem(5, 0); });

            AddFlavorItem("Team", new Vector2(7, 6));
            AddFlavorItem("_________", new Vector2(7, 7));
            AddMenuItem("Blue", new Vector2(7, 8), 1,
                delegate() { team = 0; SendTeam(); ActivateItem(0, 1); });
            AddMenuItem("Red", new Vector2(7, 9), 1,
                delegate() { team = 1; SendTeam(); ActivateItem(1, 1); });

            AddMenuItem("OK", new Vector2(15, 8), 2,
               delegate() { SendReady();  });

            
            
        }

        public void SendClass()
        {
            Client.Send("class" + "\n" + World.gameId + "\n" + classType );
        }

        public void SendTeam()
        {
            Client.Send("team" + "\n" + World.gameId + "\n" + team );
        }

        public void SendReady()
        {
            ready = true;
            Client.Send("ready");
        }

        public override void Init()
        {
            yIndex = 0; xIndex = 0;
            menuItems[0][0].Select();
            ready = false;
            base.Init();
            Thread.Sleep(150);
            Client.Send("connect" + "\n" + World.gameId );
            SendClass();
        }

        public override void Update()
        {
            if (!ready)
            {
                base.Update();
            }
        }

        static Vector2 GetPos(float x, float y)
        {
            return new Vector2((GameConst.SCREEN_WIDTH / 20f) * x, (GameConst.SCREEN_HEIGHT / 20f) * y);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            int count = 0;
            lock (World.PlayerInfo)
            {
                try
                {
                    foreach (var player in World.PlayerInfo)
                    {
                        Color c = player.Value.team == 0 ? Color.Blue : Color.Red;
                        sb.DrawString(TextureManager.Fonts["console"], player.Key.ToString(),
                            GetPos(1, count + 1), c);
                        count++;
                    }
                }
                catch (Exception e) { }
            }
        }
    }
}
