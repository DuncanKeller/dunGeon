using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.HUD
{
    static class Hud
    {
        static Rectangle healthRect;
        public static void Init()
        {
            Map.Init(World.Map);
        }

        public static void Update()
        {
            Map.Update(World.Player.Position);

            int healthWidth = (int)(((World.Player.MaxHealth - World.Player.Health) / World.Player.MaxHealth) * 200);
            healthRect = new Rectangle(20, 20, healthWidth, 40);
        }

        public static void Draw(SpriteBatch sb)
        {
            // draw map
            Map.Draw(sb, new Vector2(20, 80));
            // health
            sb.Draw(TextureManager.Map["blank"], healthRect, Color.Red);
        }
    }
}
