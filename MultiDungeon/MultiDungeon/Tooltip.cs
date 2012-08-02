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
        static int w = GameConst.SCREEN_WIDTH / 6;
        static int h = GameConst.SCREEN_HEIGHT / 4;
        static Rectangle rect = new Rectangle(GameConst.SCREEN_WIDTH - w - 20, 150, w, h);

        public static void Draw(SpriteBatch sb)
        {
            if (World.Player.Item != null)
            {
                if (World.Player.Item.Description != String.Empty)
                {
                    string d = World.Player.Item.Description;

                    int len = 0;
                    int lastSpace = 0;
                    for (int i = 0; i < d.Length; i++)
                    {
                        len += (int)(TextureManager.Fonts["tooltip"].MeasureString(d[i].ToString()).X);

                        if (d[i] == ' ')
                        {
                            lastSpace = i;
                        }

                        if (len >= w - 20)
                        {
                            len = 0;
                            d = d.Insert(lastSpace + 1, "\r\n");
                            i += 4;
                        }
                    }

                    sb.Draw(TextureManager.Map["blank"], rect, new Color(100, 50, 0, 100));
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(rect.X, rect.Y - 3, rect.Width, 3), new Color(100, 100, 100, 100));
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(rect.X, rect.Bottom, rect.Width, 3), new Color(100, 100, 100, 100));
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(rect.X - 3, rect.Y - 3, 3, rect.Height + 6), new Color(100, 100, 100, 100));
                    sb.Draw(TextureManager.Map["blank"], new Rectangle(rect.Right, rect.Y - 3, 3, rect.Height + 6), new Color(100, 100, 100, 100));

                    sb.DrawString(TextureManager.Fonts["tooltip"], d, new Vector2(rect.X + 10, rect.Y + 10), Color.Black);
                }
            }
        }
    }
}
