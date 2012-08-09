using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MultiDungeon.HUD
{
    static class Hud
    {
        static Rectangle healthRect;
        static int sounds = 3;
        public static void Init()
        {
            //Map.Init(World.Map);
        }

        public static void Update(float deltaTime)
        {
            foreach (Player p in World.Players)
            {
                // test for mapmaker here
                if (p is Mapmaker)
                {
                    Map.Update(p);
                }
            }
            if (World.Player.Item != null)
            {
                //Vector2 v = Camera.ToLocalLocation(World.Player.Position, World.Camera.Transform);
                Vector2 v = new Vector2(GameConst.SCREEN_WIDTH / 2, GameConst.SCREEN_HEIGHT / 2);
                World.Player.Item.Update(deltaTime,
                    (int)v.X , (int)v.Y, World.Player.DrawRect.Width, World.Player.DrawRect.Height);
            }
            int healthWidth = (int)(((World.Player.MaxHealth - (World.Player.MaxHealth - World.Player.Health)) / World.Player.MaxHealth) * 200);
            healthRect = new Rectangle(45, 20, healthWidth, 20);
        }

        public static void Draw(SpriteBatch sb, float deltaTime)
        {
            // draw map
            Map.Draw(sb, new Vector2(20, 80));
            Map.DrawPlayers(sb, new Vector2(20, 80));
            // health
            sb.Draw(TextureManager.Map["health-icon"], new Rectangle(10, 20, 25, 20), Color.White);
            sb.Draw(TextureManager.Map["health-bar"], healthRect, Color.Red);
            sb.DrawString(TextureManager.Fonts["console"], Math.Max(Math.Round(World.Player.Health, 1), 0.1).ToString() + "/" + World.Player.MaxHealth,
                new Vector2(healthRect.Right + 5, healthRect.Top - (TextureManager.Fonts["console"].MeasureString("0").Y / 5)), Color.Red);
            // money
            sb.Draw(TextureManager.Map["gold-icon"], new Rectangle(10, 40, 25, 20), Color.White);
            Rectangle goldRect = new Rectangle(45, 40, World.Player.Gold / 2, 20);
            sb.Draw(TextureManager.Map["gold-bar"], goldRect, Color.Gold);
            sb.DrawString(TextureManager.Fonts["console"], World.Player.Gold.ToString(),
               new Vector2(goldRect.Right + 5, goldRect.Top - (TextureManager.Fonts["console"].MeasureString("0").Y / 5)), Color.Gold);
            // guns & ammo
            World.Player.CurrentGun.DrawIcon(sb);
            World.Player.CurrentGun.DrawArsenal(sb);
            // item
            if (World.Player.Item != null)
            {
                World.Player.Item.Draw(sb, World.Player.Item.Pos );
            }

            Tooltip.Draw(sb, deltaTime);
            DrawCountdown(sb);
        }

        public static void DrawCountdown(SpriteBatch sb)
        {
            if (World.Countdown > -1)
            {
                Color c = Color.White;
                Texture2D texture = null;

                if (World.Countdown > 2)
                {
                    texture = TextureManager.Map["countdown-3"];
                    if (sounds == 3)
                    {
                        SoundManager.PlaySound("ding");
                        sounds--;
                    }
                }
                else if (World.Countdown > 1)
                {
                    texture = TextureManager.Map["countdown-2"];
                    if (sounds == 2)
                    {
                        SoundManager.PlaySound("ding");
                        sounds--;
                    }
                }
                else if (World.Countdown > 0)
                {
                    texture = TextureManager.Map["countdown-1"];
                    if (sounds == 1)
                    {
                        SoundManager.PlaySound("ding");
                        sounds--;
                    }
                }
                else
                {
                    texture = TextureManager.Map["countdown-go"];
                    c = Color.Green;
                    if (sounds == 0)
                    {
                        SoundEffectInstance s = SoundManager.SpecialSounds["ding"].CreateInstance();
                        s.Pitch = 0.5f; 
                        s.Play();
                        sounds--;
                    }
                }

                float remainder = Math.Abs(World.Countdown - ((float)Math.Truncate(World.Countdown)));
                int width = (int)(texture.Width * remainder);
                int height = (int)(texture.Height * remainder);
                Rectangle rect = new Rectangle((GameConst.SCREEN_WIDTH / 2) - (width / 2),
                    (GameConst.SCREEN_HEIGHT / 4) - (height / 2), width, height);
                sb.Draw(texture, rect, c);
            }
            else
            {
                sounds = 3;
            }
        }
    }
}