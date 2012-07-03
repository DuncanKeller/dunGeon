using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Menus
{
    public delegate void MenuAction();

    public class MenuItem
    {
        Texture2D texture;
        string text;
        Vector2 pos;
        MenuAction action;
        bool selected = false;
        int w;
        int h;
        Color color = Color.White;

        public MenuItem(string text, Vector2 pos, MenuAction action)
        {
            this.text = text;
            this.pos = pos;
            this.action = action;
            texture = null;
        }

        public MenuItem(Texture2D texture, Vector2 pos, int w, int h, MenuAction action)
        {
            this.text = "";
            this.pos = pos;
            this.action = action;
            this.texture = texture;
            this.w = w;
            this.h = h;
        }

        public void ChangeText(string t)
        {
            text = t;
        }

        public void ChangeColor(Color c)
        {
            color = c;
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
            int width = (GameConst.SCREEN_WIDTH / 20) * w;
            int height = (GameConst.SCREEN_WIDTH / 20) * h;

            if (texture == null)
            {
                sb.DrawString(TextureManager.Fonts["console"], append + text, position, c);
            }
            else
            {
                sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, width, height), color);
            }
        }
        
    }
}
