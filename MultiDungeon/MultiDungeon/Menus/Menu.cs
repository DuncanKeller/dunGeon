using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MultiDungeon.Menus
{
    class Menu
    {
        protected List<MenuItem> menuItems = new List<MenuItem>();
        protected int index = 0;
        protected int prevIndex = 0;
        protected int timer = 0;
        protected Game1 game;
        protected const double THRESHHOLD = 0.2;

        public Menu(Game1 g)
        {
            game = g;
        }

        public void AddMenuItem(string text, Vector2 pos, MenuAction action)
        {
            menuItems.Add(new MenuItem(text, pos, action));
        }

        public void Update()
        {
            GamePadState gs = GamePad.GetState(PlayerIndex.One);
            if (timer == 0)
            {
                if (gs.ThumbSticks.Left.Y > THRESHHOLD)
                {
                    index = index == 0 ? menuItems.Count - 1 : index - 1;
                    menuItems[index].Select();
                    menuItems[prevIndex].Deselect();
                    timer = 10;
                }
                else if (gs.ThumbSticks.Left.Y < -THRESHHOLD)
                {
                    index = index == menuItems.Count - 1 ? 0 : index + 1;
                    menuItems[index].Select();
                    menuItems[prevIndex].Deselect();
                    timer = 10;
                }
            }
            else
            {
                timer--;
            }
            if (gs.Buttons.A == ButtonState.Pressed)
            {
                menuItems[index].Invoke();
            }

            prevIndex = index;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (MenuItem item in menuItems)
            {
                item.Draw(sb);
            }
        }
    }
}
