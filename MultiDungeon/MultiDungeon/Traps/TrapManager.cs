using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Traps
{
    class TrapManager
    {
        List<Trap> traps = new List<Trap>();
        List<Trap> toRemove = new List<Trap>();
        List<Trap> toAdd = new List<Trap>();

        public List<Trap> Traps
        {
            get { return traps; }
        }

        public void Reset()
        {
            foreach (Trap t in traps)
            {
                toRemove.Add(t);
            }
        }

        public void Update(float deltaTime)
        {
            Cleanup();
            AddNewChests();

            foreach (Trap trap in traps)
            {
                trap.Update(deltaTime);
                foreach (Player p in World.Players)
                {
                    if (trap is Spike)
                    {
                        (trap as Spike).TestHit(p);
                    }
                }
            }
        }

        private void Cleanup()
        {
            foreach (Trap t in toRemove)
            {
                traps.Remove(t);
            }

            toRemove.Clear();
        }

        private void AddNewChests()
        {
            foreach (Trap t in toAdd)
            {
                traps.Add(t);
            }

            toAdd.Clear();
        }

        public void Add(Trap t)
        {
            toAdd.Add(t);
        }

        public void Remove(Trap t)
        {
            toRemove.Add(t);
        }

        public void RemoveTraps()
        {
            foreach (Trap trap in traps)
            {
                toRemove.Add(trap);
            }
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Trap trap in traps)
            {
                trap.Draw(sb);
            }
        }
    }
}
