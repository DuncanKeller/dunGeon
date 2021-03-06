﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    public abstract class Weapon
    {
        protected Texture2D icon;

        public Texture2D Icon
        {
            get { return icon; }
        }

        public abstract void Update(double dt);

        public abstract void Draw(SpriteBatch sb);

        public virtual void DrawIcon(SpriteBatch sb)
        {
            sb.Draw(icon, new Rectangle(GameConst.SCREEN_WIDTH - 200, 20, 120, 120), Color.White);
        }

        public void DrawArsenal(SpriteBatch sb)
        {
            int count = 0;
            List<Weapon> weapons = new List<Weapon>();
            int index = 0;
            foreach (Weapon weapon in World.Player.Guns)
            {
                if (weapon == World.Player.CurrentGun)
                {
                    break;
                }
                index++;
            }

            for (int i = index + 1; i < index + World.Player.Guns.Count; i++)
            {
                int currGun = i;
                if (currGun >= World.Player.Guns.Count)
                {
                    currGun -= World.Player.Guns.Count;
                }
                weapons.Add(World.Player.Guns[currGun]);
            }

            foreach (Weapon weapon in weapons)
            {
                Color c = Color.White;

                if (weapon == World.Player.CurrentGun)
                { c = Color.Yellow; }

                sb.Draw(weapon.Icon, new Rectangle((GameConst.SCREEN_WIDTH - 210 - 60) - (count * 60), 60, 60, 60), c);
                count++;
            }
        }
    }
}
