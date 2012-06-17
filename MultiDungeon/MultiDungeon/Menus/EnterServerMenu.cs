using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MultiDungeon.Menus
{
    class EnterServerMenu : Menu
    {
        string ip = "";
        string name = "";

        KeyboardState ks;
        KeyboardState oks;

        public EnterServerMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            AddFlavorItem("please enter a server ip", new Vector2(1, 1));
            AddMenuItem(">", new Vector2(2, 2),
                0, delegate() { menuItems[0][0].Deselect(); menuItems[0][1].Select(); yIndex = 1; });

            AddFlavorItem("please enter a name for the server", new Vector2(1, 4));
            AddMenuItem(">", new Vector2(2, 5),
                0, delegate() { menuItems[0][1].Deselect(); menuItems[0][2].Select(); yIndex = 2; });

            AddMenuItem("DONE", new Vector2(1, 7),
                0, delegate() { menuItems[0][2].Deselect(); menuManager.SwitchMenu(menuManager.join); });

            
        }

        public override void Init()
        {
            ip = "";
            name = "";
            menuItems[0][0].Select();
            yIndex = 0;
            xIndex = 0;
            base.Init();
        }

        public string Name
        {
            get { return name; }
        }

        public string IP
        {
            get { return ip; }
        }

        private string field
        {
            set
            {
                if (menuItems[0][0].Selected)
                {
                    ip = value;
                }
                else if(menuItems[0][1].Selected)
                {
                    name = value;
                }
            }
            get
            {
                if (menuItems[0][0].Selected)
                {
                    return ip;
                }
                else if (menuItems[0][1].Selected)
                {
                    return name;
                }
                else { return "something went wrong. Check the field property!"; }
            }
        }

        public override void Update()
        {
            ks = Keyboard.GetState();
            if (ks.GetPressedKeys().Count() > 0)
            {
                Keys k = ks.GetPressedKeys()[0];

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
                        else if (k == Keys.Space)
                        {
                            field += ' ';
                        }
                    }
                    if (k == Keys.Enter)
                    {
                        if (menuItems[0][0].Selected)
                        { menuItems[0][0].Deselect(); menuItems[0][1].Select(); yIndex++; }
                        else if (menuItems[0][1].Selected)
                        { menuItems[0][1].Deselect(); menuItems[0][2].Select(); yIndex++; }
                        else if (menuItems[0][2].Selected)
                        { }
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

            sb.DrawString(TextureManager.Fonts["console"], name,
                new Vector2((GameConst.SCREEN_WIDTH / 20) * 3,
               (GameConst.SCREEN_HEIGHT / 20) * 5), Color.White);
            base.Draw(sb);
        }
    }
}
