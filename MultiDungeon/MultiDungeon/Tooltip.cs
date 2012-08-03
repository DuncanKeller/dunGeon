using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Tooltip
    {
        public static bool activated = true;

        static int iw = GameConst.SCREEN_WIDTH / 6;
        static int ih = GameConst.SCREEN_HEIGHT / 4;
        static Rectangle itemRect = new Rectangle(GameConst.SCREEN_WIDTH - iw - 20, 150, iw, ih);
        static string oldItemTip;

        static int w = (int)(GameConst.SCREEN_WIDTH * 0.9);
        static int h = GameConst.SCREEN_HEIGHT / 6;
        static Rectangle tooltipRect = new Rectangle((int)(GameConst.SCREEN_WIDTH * 0.05),
            GameConst.SCREEN_HEIGHT - h - (GameConst.SCREEN_HEIGHT / 20), w, h);

        static string currTip = "";
        static Dictionary<Type, List<string>> classTips;
        static List<string> generalTips;
        static List<string> controlTips;

        static float timer = 0;
        static float showTime = 5; // seconds
        static float hideTime = 15; // seconds
        static bool showing = false;

        public static void Init()
        {
            classTips = new Dictionary<Type, List<string>>();
            classTips[typeof(Mapmaker)] = new List<string>();
            classTips[typeof(Mapmaker)].Add("Mapmaker- Explore the dungeon to uncover the minimap");
            classTips[typeof(Ninja)] = new List<string>();
            classTips[typeof(Ninja)].Add("Ninja- Hold still to turn invisible");
            classTips[typeof(Powdermonkey)] = new List<string>();
            classTips[typeof(Powdermonkey)].Add("Powdermonkey- Use explosives to overpower opponents");
            classTips[typeof(Capitalist)] = new List<string>();
            classTips[typeof(Capitalist)].Add("Capitalist- Press Y/Tab to open the shop, where you can buy weapons and items.");
            classTips[typeof(Apothecary)] = new List<string>();
            classTips[typeof(Apothecary)].Add("Apothecary- Press Y/Tab to open the potion thing. Process items to make ingredients. Combine ingredients to make new potions.");
            classTips[typeof(Mystic)] = new List<string>();
            classTips[typeof(Mystic)].Add("Mystic- As the mystic, you can see what items are in chests, and what items other players are holding.");
            classTips[typeof(Mystic)].Add("Mystic- As the mystic, you can find enemies on the minimap.");

            generalTips = new List<string>();
            generalTips.Add("Kill enemies to get gold. Bring enough gold back to the team chest to win!");
            generalTips.Add("Avoid traps and gunfire. You lose gold upon death!");
            generalTips.Add("Chests contain items and gold. You can only hold one item at a time.");
            controlTips = new List<string>();
            controlTips.Add("Press A/E to interact with chests");
            controlTips.Add("Press X/Space Bar to use an item, if you are holding one");
            controlTips.Add("Press B/R to reload");
        }

        private static string FormatText(string d, Rectangle r)
        {
            float len = 0;
            int lastSpace = 0;
            for (int i = 0; i < d.Length; i++)
            {
                len += TextureManager.Fonts["tooltip"].MeasureString(d[i].ToString()).X +
                    TextureManager.Fonts["tooltip"].Spacing;

                char yuk = d[i];
                if (d[i] == ' ')
                {
                    lastSpace = i;
                }

                if (len >= r.Width - 20)
                {
                    len = TextureManager.Fonts["tooltip"].MeasureString(d.Substring(lastSpace + 1, i - (lastSpace))).X;
                    d = d.Insert(lastSpace + 1, "\r\n");
                    i++;
                }
            }

            return d;
        }

        public static void Draw(SpriteBatch sb, float deltaTime)
        {
            if (!activated)
            {
                return;
            }

            if (timer > 0)
            {
                timer -= deltaTime / 1000;
            }
            else
            {
                if (showing == false)
                {
                    int whichList = GameConst.rand.Next(3);
                    switch (whichList)
                    {
                        case 0:
                            currTip = classTips[World.Player.GetType()][GameConst.rand.Next(classTips[World.Player.GetType()].Count - 1)];
                            break;
                        case 1:
                            currTip = generalTips[GameConst.rand.Next(generalTips.Count - 1)];
                            break;
                        case 2:
                            currTip = controlTips[GameConst.rand.Next(controlTips.Count - 1)];
                            break;
                    }
                    currTip = FormatText(currTip, tooltipRect);
                    timer = showTime;
                }
                else
                {
                    timer = hideTime;
                }
                showing = !showing;
            }

            if (World.Player.Item != null)
            {
                if (World.Player.Item.Description != String.Empty)
                {
                    string d = World.Player.Item.Description;
                    if (d != oldItemTip)
                    {
                        d = FormatText(d, itemRect);
                    }
                    sb.Draw(TextureManager.Map["blank"], itemRect, new Color(100, 50, 0, 100));
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(itemRect.X, itemRect.Y - 3, itemRect.Width, 3), new Color(100, 100, 100, 100));
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(itemRect.X, itemRect.Bottom, itemRect.Width, 3), new Color(100, 100, 100, 100));
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(itemRect.X - 3, itemRect.Y - 3, 3, itemRect.Height + 6), new Color(100, 100, 100, 100));
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(itemRect.Right, itemRect.Y - 3, 3, itemRect.Height + 6), new Color(100, 100, 100, 100));

                    sb.DrawString(TextureManager.Fonts["tooltip"], d, new Vector2(itemRect.X + 10, itemRect.Y + 10), Color.Black);

                    oldItemTip = d;
                }
            }

            if (showing)
            {
                sb.Draw(TextureManager.Map["blank"], tooltipRect, new Color(100, 50, 0, 100));
                sb.Draw(TextureManager.Map["blank"], new Rectangle(tooltipRect.X, tooltipRect.Y - 3, tooltipRect.Width, 3), new Color(100, 100, 100, 100));
                sb.Draw(TextureManager.Map["blank"], new Rectangle(tooltipRect.X, tooltipRect.Bottom, tooltipRect.Width, 3), new Color(100, 100, 100, 100));
                sb.Draw(TextureManager.Map["blank"], new Rectangle(tooltipRect.X - 3, tooltipRect.Y - 3, 3, tooltipRect.Height + 6), new Color(100, 100, 100, 100));
                sb.Draw(TextureManager.Map["blank"], new Rectangle(tooltipRect.Right, tooltipRect.Y - 3, 3, tooltipRect.Height + 6), new Color(100, 100, 100, 100));

                sb.DrawString(TextureManager.Fonts["tooltip"], currTip, new Vector2(tooltipRect.X + 10, tooltipRect.Y + 10), Color.Black);
            }
        }
    }
}
