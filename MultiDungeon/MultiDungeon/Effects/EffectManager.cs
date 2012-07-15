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
            }
        }
    }
}
