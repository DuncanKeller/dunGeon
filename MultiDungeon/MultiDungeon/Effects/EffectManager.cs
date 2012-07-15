using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MultiDungeon.Effects
{
    public static class EffectManager
    {
        static float timer;
        static float maxTimer;

        public static void Update(float deltaTime, Type t, Player p)
        {
            if(t == typeof(PoisinParticle))
            {
                maxTimer = 0.5f;
            }
            else if (t == typeof(StarParticle))
            {
                maxTimer = 0.65f;
            }
            else if (t == typeof(HealthParticle))
            {
                maxTimer = 0.8f;
            }

            if (timer > 0)
            {
                timer -= deltaTime / 100;
            }
            else
            {
                timer = maxTimer;

                int x = p.DrawRect.Left + GameConst.rand.Next(p.DrawRect.Width);
                int y = p.DrawRect.Top + GameConst.rand.Next(p.DrawRect.Height);

                if (t == typeof(PoisinParticle))
                {
                    World.BulletManager.AddParticle(new PoisinParticle(new Vector2(x, y)));
                }
                if (t == typeof(HealthParticle))
                {
                    World.BulletManager.AddParticle(new HealthParticle(new Vector2(x, y)));
                }
                else if (t == typeof(StarParticle))
                {
                    World.BulletManager.AddParticle(new StarParticle(new Vector2(x, y)));
                    int x2 = p.DrawRect.Left + GameConst.rand.Next(p.DrawRect.Width);
                    int y2 = p.DrawRect.Top + GameConst.rand.Next(p.DrawRect.Height);
                    World.BulletManager.AddParticle(new SparkleParticle(new Vector2(x2, y2)));
                }
            }
        }
    }
}
