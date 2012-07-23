using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon.Items
{
    class Backdrop
    {
        Texture2D texture;
        Rectangle rect;

        public Backdrop(int x, int y)
        {
            int w = 0;
            int h = 0;
           
            switch (GameConst.rand.Next(3))
            {
                case 0:
                    texture = TextureManager.Map["skull"];
                    w = 30;
                    h = 25;
                    break;
                case 1:
                    texture = TextureManager.Map["bones"];
                    w = 25;
                    h = 20;
                    break;
                case 2:
                    texture = TextureManager.Map["rocks"];
                    w = 25;
                    h = 20;
                    break;
            }

            rect = new Rectangle(
               x + GameConst.rand.Next(Tile.TILE_SIZE - w),
               y + GameConst.rand.Next(Tile.TILE_SIZE - h), w, h);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(texture, rect, new Color(240,240,240));
        }
    }
}
