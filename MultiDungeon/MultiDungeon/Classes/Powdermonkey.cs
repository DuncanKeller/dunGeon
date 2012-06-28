using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Powdermonkey : Player
    {

        public Powdermonkey(float x, float y, int id)
            : base(x, y, id)
        {
            upgrade.maxSpeed = 3.5f;
            guns.Add(new GrenadeLauncher(World.BulletManager, this));
            guns.Add(new RocketLauncher(World.BulletManager, this));

            //characterTest = TextureManager.Map["powdermonkey-blue"];
        }

        public override void Init(int t)
        {
            base.Init(t);

            if (t == 0)
            {
                characterTest = TextureManager.Map["powdermonkey-blue"];
            }
            else
            {
                characterTest = TextureManager.Map["powdermonkey-red"];
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
