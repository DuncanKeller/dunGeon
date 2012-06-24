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
            //Map.Init(World.Map);
        }

        public static void Update()
        {
            foreach (Player p in World.Players)
            {
                // test for mapmaker here
                if (p is Mapmaker)
                {
                    Map.Update(p);
                }
            }
            int healthWidth = (int)(((World.Player.MaxHealth - (World.Player.MaxHealth - World.Player.Health)) / World.Player.MaxHealth) * 200);
            healthRect = new Rectangle(20, 20, healthWidth, 20);
        }

        public static void Draw(SpriteBatch sb)
        {
            // draw map
            Map.Draw(sb, new Vector2(20, 80));
            // health
            sb.Draw(TextureManager.Map["blank"], healthRect, Color.Red);
            sb.DrawString(TextureManager.Fonts["console"], Math.Round(World.Player.Health, 1).ToString() + "/" + World.Player.MaxHealth,
                new Vector2(healthRect.Right + 5, healthRect.Top - (TextureManager.Fonts["console"].MeasureString("0").Y / 5)), Color.Red);
            // money
            Rectangle goldRect = new Rectangle(20, 40, World.Player.Gold / 2, 20);
            sb.Draw(TextureManager.Map["blank"], goldRect, Color.Gold);
            sb.DrawString(TextureManager.Fonts["console"], World.Player.Gold.ToString(),
               new Vector2(goldRect.Right + 5, goldRect.Top - (TextureManager.Fonts["console"].MeasureString("0").Y / 5)), Color.Gold);
            // guns & ammo
            World.Player.CurrentGun.DrawIcon(sb);
            World.Player.CurrentGun.DrawArsenal(sb);
            // item
            if (World.Player.Item != null)
            {
                World.Player.Item.Draw(sb, new Vector2(GameConst.SCREEN_WIDTH - 80, 20));
            }
        }
    }
}