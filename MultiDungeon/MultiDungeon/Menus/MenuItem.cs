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
        Rectangle hit;
        Texture2D texture;
        string text;
        Vector2 pos;
        MenuAction action;
        bool selected = false;
        int w;
        int h;
        Color color = Color.White;

        bool activated = false;

        public bool hidden = false;

        public string Text
        {
            get { return text; }
        }

        public bool Activated
        {
            get { return activated; }
            set { activated = value; }
        }

        public void Init()
        {
            if (texture != null)
            {
                hit = new Rectangle((int)((GameConst.SCREEN_WIDTH / 20) * pos.X),
                      (int)((GameConst.SCREEN_HEIGHT / 20) * pos.Y),
                      w, h);
            }
            else
            {
                hit = new Rectangle((int)((GameConst.SCREEN_WIDTH / 20) * pos.X),
                    (int)((GameConst.SCREEN_HEIGHT / 20) * pos.Y),
                    (int)TextureManager.Fonts["console"].MeasureString(text).X,
                    (int)TextureManager.Fonts["console"].MeasureString(text).Y);
            }
        }

        public MenuItem(string text, Vector2 pos, MenuAction action)
        {
            this.text = text;
            this.pos = pos;
            this.action = action;
            texture = null;
            Init();
        }

        public MenuItem(Texture2D texture, Vector2 pos, int w, int h, MenuAction action)
        {
            this.text = "";
            this.pos = pos;
            this.action = action;
            this.texture = texture;
            this.w = w;
            this.h = h;
            Init();
        }

        public bool Hovering(Vector2 pos)
        {
            Rectangle testRect = new Rectangle((int)pos.X, (int)pos.Y, 1, 1);
            if (testRect.Intersects(hit))
            {
                return true;
            }
            return false;
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
            set { selected = value; }
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
            if (!hidden)
            {
                Color c = selected ? Color.White : new Color(100, 100, 100);
                if (activated)
                { c = Color.Yellow; }
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
}
