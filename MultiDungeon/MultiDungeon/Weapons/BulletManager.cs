using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon
{
    class BulletManager
    {
        List<Bullet> bullets = new List<Bullet>();
        List<Bullet> toRemove = new List<Bullet>();
        List<Bullet> toAdd = new List<Bullet>();

        public void Update(TileSet tiles)
        {
            Cleanup();
            AddNewBullets();

            foreach (Bullet b in bullets)
            {
                b.Update();

                foreach (Tile t in tiles.GetTilesNear((int)b.Position.X, (int)b.Position.Y))
                {
                    if (t.Type == TileType.wall)
                    {
                        if (t.Rect.Intersects(b.Rect))
                        {
                            Remove(b);
                        }
                    }
                }
            }

            CollisionTest();
        }

        public void CollisionTest()
        {
            foreach (Player p in World.Players)
            {
                foreach (Bullet b in bullets)
                {
                    if (b.Rect.Intersects(p.DrawRect))
                    {
                        p.Hit(b);
                        Remove(b);
                    }
                }
            }
        }

        private void Cleanup()
        {
            foreach (Bullet b in toRemove)
            {
                bullets.Remove(b);
            }

            toRemove.Clear();
        }

        private void AddNewBullets()
        {
            foreach (Bullet b in toAdd)
            {
                bullets.Add(b);
            }

            toAdd.Clear();
        }

        public void Add(Bullet b)
        {
            toAdd.Add(b);
        }

        public void Remove(Bullet b)
        {
            toRemove.Add(b);
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (Bullet b in bullets)
            {
                b.Draw(sb);
            }
        }
    }
}
