using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;

namespace MultiDungeon
{
    public class BulletManager
    {
        List<Bullet> bullets = new List<Bullet>();
        List<Bullet> toRemove = new List<Bullet>();
        List<Bullet> toAdd = new List<Bullet>();

        public void Update(TileSet tiles, float deltaTime)
        {
            Cleanup();
            AddNewBullets();

            foreach (Bullet b in bullets)
            {
                b.Update(deltaTime);

                foreach (Tile t in tiles.GetTilesNear((int)b.Position.X, (int)b.Position.Y))
                {
                    if (t.Type == TileType.wall)
                    {
                        if (b is Grenade)
                        {

                            if (t.Rect.Intersects(b.Rect))
                            {
                                if (Math.Abs(b.Rect.Center.Y - t.Rect.Center.Y) >
                                    Math.Abs(b.Rect.Center.X - t.Rect.Center.X))
                                {
                                    if (b.Rect.Top < t.Rect.Bottom ||
                                        b.Rect.Bottom > t.Rect.Top)
                                    {
                                        b.FlipY();
                                    }
                                }
                                else
                                {
                                    if (b.Rect.Left < t.Rect.Right ||
                                        b.Rect.Right > t.Rect.Left)
                                    {
                                        b.FlipX();
                                    }
                                }

                            }
                        }
                        else
                        {
                            if (t.Rect.Intersects(b.Rect))
                            {
                                Remove(b);
                            }
                        }
                    }
                }

                if (b is Grenade)
                {
                    Grenade g = (Grenade)b;
                    if (g.Exploded)
                    {
                        toRemove.Add(b);
                        HandleExplosion(g.Position);
                    }
                }
            }

            CollisionTest();
        }

        public void HandleExplosion(Vector2 pos)
        {
            foreach (Player p in World.Players)
            {

            }
        }

        public void CollisionTest()
        {
            foreach (Player p in World.Players)
            {
                foreach (Bullet b in bullets)
                {
                    if (b.PlayerID != p.ID &&
                        p.Alive)
                    {
                        if (b.Rect.Intersects(p.Rect))
                        {
                            p.Hit(b);
                            Remove(b);
                        }
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
