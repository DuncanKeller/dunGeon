﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Mapmaker : Player
    {

        public Mapmaker(float x, float y, int id)
            : base(x, y, id)
        {
            upgrade.maxSpeed = 3.6f;
            guns.Add(new Revolver(World.BulletManager, this));
            guns.Add(new Shotgun(World.BulletManager, this));
            characterTest = TextureManager.Map["mapmaker-blue"];
        }

        public override void Init(int t)
        {
            base.Init(t);

            if (t == 0)
            {
                characterTest = TextureManager.Map["mapmaker-blue"];
            }
            else
            {
                characterTest = TextureManager.Map["mapmaker-red"];
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
