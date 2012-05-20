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
        List<Chest> toRemove = new List<Chest>();
        List<Chest> toAdd = new List<Chest>();

        public List<Chest> Chests
        {
            get { return chests; }
        }

        public ItemManager()
        {
            //test
        }

        public void Update()
        {
            Cleanup();
            AddNewChests();
        }

        private void Cleanup()
        {
            foreach (Chest b in toRemove)
            {
                chests.Remove(b);
            }

            toRemove.Clear();
        }

        private void AddNewChests()
        {
            foreach (Chest b in toAdd)
            {
                chests.Add(b);
            }

            toAdd.Clear();
        }

        public void OpenChest(int id)
        {
            foreach (Chest chest in chests)
            {
                if (chest.ID == id)
                {
                    chest.Open();
                }
            }
        }

        public void Add(Chest c)
        {
            toAdd.Add(c);
        }

        public void Remove(Chest c)
        {
            toRemove.Add(c);
        }

        public void Remove(int id)
        {
            foreach (Chest chest in chests)
            {
                if (chest.ID == id)
                {
                    toRemove.Add(chest);
                }
            }
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
