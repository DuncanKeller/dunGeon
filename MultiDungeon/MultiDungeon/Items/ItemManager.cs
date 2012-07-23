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
        List<Backdrop> backdrops = new List<Backdrop>();

        public List<Chest> Chests
        {
            get { return chests; }
        }

        public void Reset()
        {
            foreach (Chest c in chests)
            {
                toRemove.Add(c);
            }
        }

        public void AddBackdrop(int x, int y)
        {
            backdrops.Add(new Backdrop(x, y));
        }

        public void Update(float deltaTime)
        {
            Cleanup();
            AddNewChests();

            foreach (Chest chest in chests)
            {
                chest.Update(deltaTime);
            }
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

        public void Add(TeamChest c)
        {
            toAdd.Add(c);
        }

        public void Remove(Chest c)
        {
            toRemove.Add(c);
        }

        public void RemoveChests()
        {
            foreach (Chest chest in chests)
            {
                toRemove.Add(chest);
            }
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

        public void DrawBackdrop(SpriteBatch sb)
        {
            foreach (Backdrop backdrop in backdrops)
            {
                backdrop.Draw(sb);
            }
        }
    }
}
