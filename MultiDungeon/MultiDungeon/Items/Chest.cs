using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon.Items
{
    public class Chest
    {
        protected Rectangle rect;
        Item contents;

        int width = Tile.TILE_SIZE;
        int height = Tile.TILE_SIZE;
        int id;
        
        int maxTimer = 20;
        float timer;

        public Rectangle Rect
        {
            get { return rect; }
        }

        public int ID
        {
            get { return id; }
        }

        public Chest(int id, Vector2 pos, Item i)
        {
            this.id = id;
            rect = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            contents = i;
            timer = maxTimer;
        }

        public void Update(float deltaTime)
        {
            // stick an item in there after a while (respawn)
            if (!(this is TeamChest))
            {
                if (contents == null)
                {
                    if (timer > 0)
                    {
                        timer -= deltaTime / 1000;
                    }
                    else
                    {
                        timer = maxTimer;
                        contents = RandomItem();
                    }
                }
            }
        }

        public static Item RandomItem()
        {
            Item item = null;
            switch (GameConst.rand.Next(6))
            {
                case 0:
                    item = new HealthPotion(HealingLevel.strong);
                    break;
                case 1:
                    item = new CoinPurse(100);
                    break;
                case 2:
                    item = new Juice();
                    break;
                case 3:
                    item = new Stoneskin();
                    break;
                case 4:
                    item = new SeeingEye();
                    break;
                case 5:
                    item = new SpareMag();
                    break;
            }
            return item;
        }

        public virtual Item Open(Player p)
        {
            if (contents != null)
            {
                Item returnItem = contents;
                contents = null;
                return returnItem;
            }
            return p.Item;
        }

        public virtual void Open()
        {
            if (contents != null)
            {
                contents = null;
            }
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (contents == null)
            {
                sb.Draw(TextureManager.Map["chest-open"], rect, Color.White);
            }
            else
            {
                sb.Draw(TextureManager.Map["chest-closed"], rect, Color.White);
            }
        }
    }
}
