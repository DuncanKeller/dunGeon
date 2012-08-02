using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Map;
using MultiDungeon.Effects;

namespace MultiDungeon
{
    public class BulletManager
    {
        List<Bullet> bullets = new List<Bullet>();
        List<Bullet> toRemove = new List<Bullet>();
        List<Bullet> toAdd = new List<Bullet>();

        List<Particle> particles = new List<Particle>();
        List<Particle> toRemoveP = new List<Particle>();
        List<Particle> toAddP = new List<Particle>();

        public void Update(TileSet tiles, float deltaTime)
        {
            Cleanup();
            AddNewBullets();
            AddNewParticles();
         
            foreach (Particle p in particles)
            {
                p.Update(deltaTime);
                if (!p.Alive)
                {
                    toRemoveP.Add(p);
                }
            }

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
                                if (b is Rocket)
                                {
                                    HandleExplosion(b.Position, b);
                                    AddExplosionParticles(b.Position);
                                }
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
                        AddExplosionParticles(g.Position);
                        HandleExplosion(g.Position, g);
                        toRemove.Add(b);
                       
                    }
                }

                if (b is Flame)
                {
                    Flame f = (Flame)b;
                    if(!f.Alive)
                    {
                        toRemove.Add(b);
                    }
                }
            }

            CollisionTest();
        }

        public void WarpEffect(Player player)
        {
            for (int i = 0; i < 40; i++)
            {
                ExplosionParticle p = new ExplosionParticle(player.Position, ParticleType.BlueSmoke);
                toAddP.Add(p);
            }
        }

        public void AddExplosionParticles(Vector2 pos)
        {
            for (int i = 0; i < 30; i++)
            {
                //Particle p = new Particle(pos, ParticleType.Circle);
                //particles.Add(p);
            }
            for (int i = 0; i < 30; i++)
            {
                ExplosionParticle p = new ExplosionParticle(pos, ParticleType.RedSmoke);
                particles.Add(p);
            }
            for (int i = 0; i < 80; i++)
            {
                ExplosionParticle p = new ExplosionParticle(pos, ParticleType.Smoke);
                particles.Add(p);
            }
        }

        public void AddParticle(Particle p)
        {
            toAddP.Add(p);
        }

        public void HandleExplosion(Vector2 pos, Bullet b)
        {
            foreach (Player p in World.Players)
            {
                float dist = Vector2.Distance(p.Position, pos);
                if (dist < 130)
                {
                    p.Hit(b);
                }
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
                            if (!(b is Grenade) && !(b is Rocket))
                            {
                                p.Hit(b);
                            }

                            if (b is Rocket)
                            {
                                HandleExplosion(b.Position, b);
                                AddExplosionParticles(b.Position);
                            }

                            if (!(p.StatusEffect == StatusEffect.invinsible) &&
                                !(b is Flame) && !(b is Grenade))
                            {
                                Remove(b);
                            }
                            if (b is Dart)
                            {
                                p.StatusEffect = StatusEffect.curse;
                                p.ResetStatusTimer();
                            }
                        }
                    }
                }
                if (p is Ninja)
                {
                    Ninja n = (Ninja)p;
                    if (n.Sword.Attacking)
                    {
                        foreach (Player enemy in World.Players)
                        {
                            if (enemy != p)
                            {
                                if (n.Sword.CollidingWithEnemy(enemy))
                                {
                                    if (!n.Sword.HasHit(enemy))
                                    {
                                        n.Sword.DealDamage(enemy);
                                        n.Sword.Hit(enemy);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (p is Mystic)
                {
                    Flamethrower f = ((p as Mystic).CurrentGun as Flamethrower);
                    foreach (Player enemy in World.Players)
                    {
                        if (enemy != p)
                        {
                            if (f.Overlapping(enemy))
                            {
                                if (f.Shooting)
                                {
                                    Flame b = new Flame();
                                    b.Init(new Vector2(0, 0), 0, f.Damage, f.Id);
                                    enemy.Hit(b);
                                }
                            }
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
            foreach (Particle p in toRemoveP)
            {
                particles.Remove(p);
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

        private void AddNewParticles()
        {
            foreach (Particle p in toAddP)
            {
                particles.Add(p);
            }

            toAddP.Clear();
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

            foreach (Particle p in particles)
            {
                p.Draw(sb);
            }
        }
    }
}
