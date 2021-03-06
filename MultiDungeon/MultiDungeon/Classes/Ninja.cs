﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using MultiDungeon.Weapons;

namespace MultiDungeon
{
    class Ninja : Player
    {
        int visibility;
        int maxInvis = 60;
        float invisTimer;
        int maxTimer = 1000;
        public Ninja(float x, float y, int id)
            : base(x, y, id)
        {
            upgrade.maxSpeed = 4;
            guns.Add(new Weapons.Sword(this));
            guns.Add(new Crossbow(World.BulletManager, this));
            characterTest = TextureManager.Map["ninja-blue"];
        }

        public Sword Sword
        {
            get
            {
                foreach (Weapon w in guns)
                {
                    if (w is Sword)
                    {
                        return w as Sword;
                    }
                }
                return null;
            }
        }

        public override void Update(float deltaTime)
        {
            // update specific class info
            if (invisTimer < 1000)
            {
                invisTimer += deltaTime;
            }
            else 
            {
                if (visibility > maxInvis)
                {
                    visibility -= 2;
                }
                else
                {
                    visibility = maxInvis;
                }
            }

            if (Math.Abs(GamePad.GetState(playerIndex).ThumbSticks.Left.X) > 0.25 ||
                Math.Abs(GamePad.GetState(playerIndex).ThumbSticks.Left.Y) > 0.25 ||
                GamePad.GetState(playerIndex).Triggers.Left > 0.1 ||
                GamePad.GetState(playerIndex).Triggers.Right > 0.1 ||
                Mouse.GetState().LeftButton == ButtonState.Pressed ||
                Mouse.GetState().RightButton == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.W) ||
                Keyboard.GetState().IsKeyDown(Keys.A) ||
                Keyboard.GetState().IsKeyDown(Keys.S) ||
                Keyboard.GetState().IsKeyDown(Keys.D))
            {
                invisTimer = 0;
                visibility = 255;
            }
            color.A = (byte)visibility;
            color.R = (byte)visibility;
            color.G = (byte)visibility;
            color.B = (byte)visibility;
            base.Update(deltaTime);
        }

        public override void Init(int t)
        {
            base.Init(t);

            if (t == 0)
            {
                characterTest = TextureManager.Map["ninja-blue"];
            }
            else
            {
                characterTest = TextureManager.Map["ninja-red"];
            }
        }
    }
}
