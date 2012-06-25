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
        BulletManager manager;
        Type bulletType;
        Player player;
        protected Texture2D icon;

        protected int maxClip;
        protected double reloadTime;
        protected double rateOfFire;
        protected double damage;

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

        public Texture2D Icon
        {
            get { return icon; }
        }

        public Gun(BulletManager bm, Type bt, Player p)
        {
            manager = bm;
            bulletType = bt;
            player = p;
        }

        public virtual void Update(double deltaTime)
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

        public void Reload()
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

        protected void FireBullet(float angleDiff = 0)
        {
            Bullet bullet = (Bullet)Activator.CreateInstance(bulletType);
            Vector2 pos = new Vector2(player.DrawRect.X, player.DrawRect.Y);
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
            Client.Send("b" + "\n" + player.ID.ToString() + "\n" + damage.ToString());
        }

        public virtual void Draw(SpriteBatch sb)
        {

        }

        public void DrawIcon(SpriteBatch sb)
        {
            sb.Draw(Icon, new Rectangle(GameConst.SCREEN_WIDTH - 200, 20, 120, 120), Color.White);

            int maxWidth = 150;
            int width = maxWidth / maxClip;
            sb.Draw(TextureManager.Map["blank"], new Rectangle((GameConst.SCREEN_WIDTH - 210) - maxWidth, 20, maxWidth, 20), new Color(0,0,0,100));
         
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

        public void DrawArsenal(SpriteBatch sb)
        {
            int count = 0;
            foreach (Gun gun in World.Player.Guns)
            {
                Color c = Color.White;

                if (gun == World.Player.CurrentGun)
                { c = Color.Yellow; }

                sb.Draw(gun.Icon, new Rectangle((GameConst.SCREEN_WIDTH - 210 - 60) - (count * 60), 60, 60, 60), c);
                count++;
            }
        }
    }
}
