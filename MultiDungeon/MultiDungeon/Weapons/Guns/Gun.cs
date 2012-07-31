using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    public abstract class Gun : Weapon
    {
        protected BulletManager manager;
        protected Type bulletType;
        protected Player player;

        protected int maxClip;
        protected double reloadTime;
        protected double rateOfFire;
        protected float damage;

        protected int clip;
        protected double reloadTimer;
        protected double fireTimer;

        public bool reloading
        {
            get { return reloadTimer != 0; }
        }

        protected bool primed
        {
            get { return (!reloading && fireTimer == 0 && clip > 0); }
        }

        public Gun(BulletManager bm, Type bt, Player p)
        {
            manager = bm;
            bulletType = bt;
            player = p;
        }

        public override void Update(double deltaTime)
        {
            if (reloadTimer > 0)
            {
                reloadTimer -= deltaTime / 1000;
            }
            else if (reloadTimer < 0)
            {
                reloadTimer = 0;
                clip = maxClip;
            }
            if (fireTimer > 0)
            {
                fireTimer -= deltaTime / 1000;
            }
            else if (fireTimer < 0)
            {
                fireTimer = 0;
            }
        }

        public virtual void Reload()
        {
            if (clip < maxClip)
            {
                clip = 0;
                reloadTimer = reloadTime;
            }
        }

        public void QuickReload()
        {
            if (clip < maxClip)
            {
                clip = 0;
                reloadTimer = reloadTime / 4;
            }
        }

        public void Reset()
        {
            clip = maxClip;
            reloadTimer = 0;
        }

        public abstract void Shoot();

        public virtual void SecondaryFire()
        {

        }

        public virtual void RightHeld()
        {

        }

        public virtual void LeftHeld()
        {

        }

        protected void FireBullet()
        {
            Bullet bullet = (Bullet)Activator.CreateInstance(bulletType);
            Vector2 pos = new Vector2(player.DrawRect.X, player.DrawRect.Y);
            float angleDiff = 0;
            if (this is Flamethrower)
            {
                float spread = (this as Flamethrower).spread;
                angleDiff = (float)GameConst.rand.NextDouble() * spread;
                angleDiff -= spread / 2;
            }
            else if (this is Shotgun)
            {
                float spread = (this as Shotgun).spread;
                angleDiff = (float)GameConst.rand.NextDouble() * spread;
                angleDiff -= spread / 2;
            }

            bullet.Init(pos, player.Angle - (float)(Math.PI / 2) + angleDiff, damage, player.ID);
            manager.Add(bullet);
            clip--;

            if (clip == 0)
            {
                Reload();
            }
            else
            {
                fireTimer = rateOfFire;
            }
            //ServerClient.SendBullet(player.ID);
            Client.Send("b" + "\n" + player.ID.ToString() + "\n" + bulletType.ToString() + "\n" + damage);
        }

        public override void Draw(SpriteBatch sb)
        {

        }

        public override void DrawIcon(SpriteBatch sb)
        {
            base.DrawIcon(sb);

            int maxWidth = 150;
            int width = maxWidth / maxClip;
            sb.Draw(TextureManager.Map["blank"], new Rectangle((GameConst.SCREEN_WIDTH - 210) - maxWidth, 20, maxWidth, 20), new Color(0, 0, 0, 100));

            int reloadWidth = maxWidth - (int)(maxWidth / (reloadTime / reloadTimer));
            if (reloading)
            {
                sb.Draw(TextureManager.Map["blank"], new Rectangle((GameConst.SCREEN_WIDTH - 210) - maxWidth, 20, reloadWidth, 20), Color.Silver);
            }
            for (int i = 0; i < clip; i++)
            {
                sb.Draw(TextureManager.Map["blank"], new Rectangle((GameConst.SCREEN_WIDTH - 210) - ((width + 1) * i) - width, 20, width, 20), Color.Gray);
            }
        }
    }
}
