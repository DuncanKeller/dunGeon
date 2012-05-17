using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Items
{
    class ItemManager
    {
        List<Chest> chests = new List<Chest>();

        public List<Chest> Chests
        {
            get { return chests; }
        }

        public ItemManager()
        {
            //test
            chests.Add(new Chest(new Vector2(300, 300), new HealthPotion(HealingLevel.regular)));
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Chest chest in chests)
            {
                chest.Draw(sb);
            }
        }
    }
}
