using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MultiDungeon.Menus
{
    public class QuickJoin : Menu
    {
        string ip = "";

        KeyboardState ks;
        KeyboardState oks;

        public QuickJoin(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            AddFlavorItem("please enter a server ip", new Vector2(1, 1));
            AddMenuItem(">", new Vector2(2, 2),
                0, delegate() { menuItems[0][0].Deselect(); menuItems[0][1].Select(); yIndex = 1; });

            AddMenuItem("CONNECT", new Vector2(1, 4),
                0, delegate() { menuItems[0][1].Deselect(); menuManager.join.ConnectToServer(ip); });
        }

        public override void BackOut()
        {
            base.BackOut();
            menuManager.SwitchMenu(menuManager.main);
        }

        public override void Init()
        {
            ip = "";
            menuItems[0][0].Select();
            yIndex = 0;
            xIndex = 0;
            base.Init();
        }

        public string IP
        {
            get { return ip; }
        }

        private string field
        {
            set
            {
                 ip = value;
            }
            get
            {
                return ip;
            }
        }

        public override void Update()
        {
            ks = Keyboard.GetState();
            if (ks.GetPressedKeys().Count() > 0)
            {
                foreach (Keys k in ks.GetPressedKeys())
                {
                    if (ks.IsKeyDown(k) &&
                        oks.IsKeyUp(k))
                    {
                        if (ip.Length < 30)
                        {
                            if (k >= Keys.A && k <= Keys.Z ||
                                k >= Keys.D0 && k <= Keys.D9)
                            {
                                field += (char)(k.GetHashCode());
                            }
                            else if (k == Keys.Back && ip.Length > 0)
                            {
                                field = field.Substring(0, field.Length - 1);
                            }
                            else if (k == Keys.OemPeriod)
                            {
                                field += '.';
                            }
                        }
                        if (k == Keys.Enter)
                        {
                            if (menuItems[0][0].Selected)
                            { menuItems[0][0].Deselect(); menuItems[0][1].Select(); yIndex++; }
                            else if (menuItems[0][1].Selected)
                            { menuItems[0][1].Invoke(); }
                        }
                    }
                }
            }
            oks = ks;
            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            /*
            sb.DrawString(TextureManager.Fonts["console"], ip,
                new Vector2((GameConst.SCREEN_WIDTH / 2) - (TextureManager.Fonts["console"].MeasureString(ip).X / 2),
                    GameConst.SCREEN_HEIGHT / 6), Color.White);
             * */
            sb.DrawString(TextureManager.Fonts["console"], ip,
                new Vector2((GameConst.SCREEN_WIDTH / 20) * 3,
               (GameConst.SCREEN_HEIGHT / 20) * 2), Color.White);
            base.Draw(sb);
        }
    }
}
