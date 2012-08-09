using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace MultiDungeon
{
    class AssaultRifle : Gun
    {
        bool shooting = false;
        float soundTimer = 0;
        float playSoundTime = 0.12f; // seconds
        public AssaultRifle(BulletManager bm, Player p)
            : base(bm, typeof(Bullet), p)
        {
            maxClip = 60;
            reloadTime = 3.0;
            rateOfFire = 0.05;
            damage = 0.5f;
            clip = maxClip;
            icon = TextureManager.Map["assault-rifle"];
        }

        public override void Update(double deltaTime)
        {
            if (shooting)
            {
                if (clip > 0 && !reloading)
                {
                    soundTimer += (float)deltaTime / 1000;

                    if (soundTimer >= playSoundTime)
                    {
                        soundTimer = 0;
                        SoundManager.PlaySound("assault-rifle");
                        //SoundEffectInstance s = SoundManager.SpecialSounds["assault-rifle"].CreateInstance();
                        //s.Play();
                    }
                }
            }
            else
            {
                soundTimer = playSoundTime;
            }

            base.Update(deltaTime);
            shooting = false;
        }

        public override void Shoot()
        {
            if (primed)
            {
                FireBullet();
            }
        }

        public override void RightHeld()
        {
            Shoot();

            shooting = true;
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }

    }
}
