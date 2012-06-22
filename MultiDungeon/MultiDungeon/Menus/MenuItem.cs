using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Menus
{
    delegate void MenuAction();

    class MenuItem
    {
        string text;
        Vector2 pos;
        MenuAction action;
        bool selected = false;

        public MenuItem(string text, Vector2 pos, MenuAction action)
        {
            this.text = text;
            this.pos = pos;
            this.action = action;
        }

        public void ChangeText(string t)
        {
            text = t;
        }

        public bool Selected
        {
            get { return selected; }
        }

        public void Invoke()
        {
            action();
        }

        public void Select()
        {
            selected = true;
        }

        public void Deselect()
        {
            selected = false;
        }

        public void Draw(SpriteBatch sb)
        {
            Color c = selected ? Color.White : new Color(100, 100, 100);
            string append = selected ? " " : "";
            Vector2 position = new Vector2((GameConst.SCREEN_WIDTH / 20) * pos.X,
                (GameConst.SCREEN_HEIGHT / 20) * pos.Y);
            sb.DrawString(TextureManager.Fonts["console"], append + text, position, c);
            
        }
        
    }
}
