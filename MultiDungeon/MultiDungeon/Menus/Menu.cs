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
        protected List<List<MenuItem>> menuItems = new List<List<MenuItem>>();
        protected List<MenuItem> nonSelectableItems = new List<MenuItem>();
        protected int yIndex = 0;
        protected int xIndex = 0;
        protected Point prevIndex = new Point(0, 0);
        protected int timer = 0;
        protected Game1 game;
        protected MenuManager menuManager;
        protected const double THRESHHOLD = 0.2;

        public Menu(Game1 g, MenuManager mm)
        {
            menuManager = mm;
            game = g;
        }

        /// <summary>
        /// Add a new MenuItem to the current menu
        /// </summary>
        /// <param name="text">What does the menu item say?</param>
        /// <param name="pos">Where is it (NOTE, not px value, its a ratio / 20)</param>
        /// <param name="index">Specifiy the horizontal menu</param>
        /// <param name="action">The function it runs</param>
        public void AddMenuItem(string text, Vector2 pos, int index, MenuAction action)
        {
            while (menuItems.Count - 1 < index)
            {
                menuItems.Add(new List<MenuItem>());
            } 
            menuItems[index].Add(new MenuItem(text, pos, action));
        }

        /// <summary>
        /// Add an item that is not selectable by the user
        /// </summary>
        /// <param name="text">What it says</param>
        /// <param name="pos">Where it is (NOTE, not px value, its a ratio / 20)</param>
        public void AddFlavorItem(string text, Vector2 pos)
        {
            nonSelectableItems.Add(new MenuItem(text, pos, delegate() { } ));
        }

        public void Update()
        {
            // get input
            GamePadState gs = GamePad.GetState(PlayerIndex.One);
            // pause after action is taken
            if (timer == 0)
            {
                bool action = false;
                // move up and down the menu
                if (gs.ThumbSticks.Left.Y > THRESHHOLD)
                {
                    yIndex = yIndex == 0 ? menuItems[xIndex].Count - 1 : yIndex - 1;
                    action = true;
                }
                else if (gs.ThumbSticks.Left.Y < -THRESHHOLD)
                {
                    yIndex = yIndex == menuItems[xIndex].Count - 1 ? 0 : yIndex + 1;
                    action = true;
                }
                else
                {
                    if (menuItems.Count > 1)
                    {
                        // move between left and right menus
                        if (gs.ThumbSticks.Left.X > THRESHHOLD)
                        {
                            xIndex = xIndex == menuItems.Count - 1 ? 0 : xIndex + 1;
                            action = true;
                        }
                        else if (gs.ThumbSticks.Left.X < -THRESHHOLD)
                        {
                            xIndex = xIndex == 0 ? menuItems.Count - 1 : xIndex - 1;
                            action = true;
                        }
                    }
                }
                if (action)
                {
                    // select new item, deselect old, reset timer
                    menuItems[xIndex][yIndex].Select();
                    menuItems[prevIndex.X][prevIndex.Y].Deselect();
                    timer = 10;
                }
            }
            else
            {
                timer--;
            }
            if (gs.Buttons.A == ButtonState.Pressed)
            {
                menuItems[xIndex][yIndex].Invoke();
            }

            prevIndex.Y = yIndex;
            prevIndex.X = xIndex;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (List<MenuItem> list in menuItems)
            {
                foreach (MenuItem item in list)
                {
                    item.Draw(sb);
                }
            }
            foreach (MenuItem item in nonSelectableItems)
            {
                item.Draw(sb);
            }
        }
    }
}
