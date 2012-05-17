﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    abstract class Gun : Weapon
    {
        BulletManager manager;
        Type bulletType;
        Player player;

        protected int maxClip;
        protected double reloadTime;
        protected double rateOfFire;
        protected double damage;

        protected double clip;
        protected double reloadTimer;
        protected double fireTimer;

        protected bool reloading
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
            bullet.Init(pos, player.Angle - (float)(Math.PI / 2), damage);
            manager.Add(bullet);
            clip--;

            if (clip == 0)
            {
                reloadTimer = reloadTime;
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
    }
}
