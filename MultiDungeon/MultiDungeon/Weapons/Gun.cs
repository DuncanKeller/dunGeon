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
    abstract class Gun : Weapon
    {
        BulletManager manager;
        Type bulletType;
        Player player;

        protected int maxClip;
        protected double reloadSpeed;
        protected double rateOfFire;
        protected int damage;

        protected double clip;
        protected double reloadTimer;
        protected double fireTimer;


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

        public abstract void Shoot(GamePadState input, GamePadState oldInput);

        private void FireBullet()
        {
            //var cons = bulletType.GetConstructors(BindingFlags.Public);
            Bullet bullet = (Bullet)Activator.CreateInstance(bulletType);
            manager.Add(bullet);
        }

        public virtual void Draw(SpriteBatch sb)
        {

        }
    }
}
