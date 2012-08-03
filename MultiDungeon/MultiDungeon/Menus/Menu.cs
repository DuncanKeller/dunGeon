using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MultiDungeon.Menus
{
    public class Menu
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
        protected GamePadState oldGamepad;
        protected MouseState oldMouse;
        bool mouseActive = true;
        float mouseVeloc = 0;
        float maxVeloc = 1.5f;

        public Menu(Game1 g, MenuManager mm)
        {
            menuManager = mm;
            game = g;
        }

        public virtual void BackOut()
        {
            menuItems[xIndex][yIndex].Selected = false;
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

        public void AddFlavorItem(Texture2D texture, Vector2 pos, int w, int h)
        {
            nonSelectableItems.Add(new MenuItem(texture, pos, w, h, delegate() { }));
        }

        public void ActivateItem(int item, int x)
        {
            int length = menuItems[x].Count;
            for (int i = 0; i < length; i++)
            {
                menuItems[x][i].Activated = false;
            }
            menuItems[x][item].Activated = true;
        }

        public virtual void Update()
        {

            // get input
            GamePadState gs = GamePad.GetState(PlayerIndex.One);
            MouseState mouse = Mouse.GetState();
            // pause after action is taken
            if (timer == 0)
            {
                bool action = false;
                // move up and down the menu
                if (gs.ThumbSticks.Left.Y > THRESHHOLD)
                {
                    yIndex = yIndex == 0 ? menuItems[xIndex].Count - 1 : yIndex - 1;
                    action = true;
                    mouseActive = false;
                }
                else if (gs.ThumbSticks.Left.Y < -THRESHHOLD)
                {
                    yIndex = yIndex == menuItems[xIndex].Count - 1 ? 0 : yIndex + 1;
                    action = true;
                    mouseActive = false;
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
                            mouseActive = false;
                        }
                        else if (gs.ThumbSticks.Left.X < -THRESHHOLD)
                        {
                            xIndex = xIndex == 0 ? menuItems.Count - 1 : xIndex - 1;
                            action = true;
                            mouseActive = false;
                        }
                    }
                }
                if (action)
                {
                    if (yIndex >= menuItems[xIndex].Count)
                    {
                        yIndex = menuItems[xIndex].Count - 1;
                    }
                    // select new item, deselect old, reset timer         
                    menuItems[prevIndex.X][prevIndex.Y].Deselect();
                    menuItems[xIndex][yIndex].Select();
                    timer = 10;
                }
            }
            else
            {
                timer--;
            }
            if (mouseActive)
            {
                mouseVeloc += (mouse.X - oldMouse.X) / 20;

                foreach (List<MenuItem> items in menuItems)
                {
                    foreach (MenuItem item in items)
                    {
                        Vector2 pos = new Vector2(mouse.X, mouse.Y);
                        if (item.Hovering(pos))
                        {
                            item.Select();

                            foreach (List<MenuItem> subItemList in menuItems)
                            {
                                foreach (MenuItem subItem in subItemList)
                                {
                                    if (subItem != item)
                                    {
                                        subItem.Deselect();
                                    }
                                }
                            }

                            if (mouse.LeftButton == ButtonState.Pressed &&
                                oldMouse.LeftButton == ButtonState.Released)
                            {
                                item.Invoke();
                            }
                        }
                        else
                        {
                            if (!item.Text.Contains('>'))
                            {
                                item.Deselect();
                            }
                        }
                    }
                }

                if (mouseVeloc > maxVeloc)
                {
                    mouseVeloc = maxVeloc;
                }
                else if (mouseVeloc < -maxVeloc)
                {
                    mouseVeloc = -maxVeloc;
                }

                if (mouseVeloc > 0)
                {
                    mouseVeloc -= mouseVeloc / 10;
                }
                else
                {
                    mouseVeloc += mouseVeloc / -10;
                }
            }
            else
            {
                // mouse inactive
                if (Math.Abs(mouseVeloc) > maxVeloc / 3)
                {
                    mouseActive = true;
                }
            }
            if (gs.Buttons.A == ButtonState.Pressed &&
                oldGamepad.Buttons.A == ButtonState.Released)
            {
                if (menuItems.Count > 0)
                {
                    if (menuItems[xIndex] != null)
                    {
                        if (menuItems[xIndex][yIndex] != null)
                        {
                            menuItems[xIndex][yIndex].Invoke();
                            mouseActive = false;
                        }
                    }
                }
            }
            // gamepad
            if (gs.Buttons.B == ButtonState.Pressed &&
                oldGamepad.Buttons.B == ButtonState.Released)
            {
                BackOut();
            }

            //mouse
             if (mouse.RightButton == ButtonState.Pressed &&
                 oldMouse.RightButton == ButtonState.Released)      
            {
                BackOut();
            }

            prevIndex.Y = yIndex;
            prevIndex.X = xIndex;
            oldGamepad = gs;
            oldMouse = mouse;
        }

        public virtual void Init()
        {
            foreach (List<MenuItem> list in menuItems)
            {
                foreach (MenuItem item in list)
                {
                    item.Init();
                }
            }
        }

        public virtual void Draw(SpriteBatch sb)
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
            if (mouseActive)
            {
                sb.Draw(TextureManager.Map["cursor"],
                    new Rectangle(oldMouse.X - 2, oldMouse.Y - 2, 30, 40),
                    new Rectangle(0, 0, TextureManager.Map["cursor"].Width, TextureManager.Map["cursor"].Height),
                    Color.White, mouseVeloc, new Vector2(0, 0), SpriteEffects.None, 0);
            }
        }
    }
}
