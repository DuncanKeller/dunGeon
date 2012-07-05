using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Mystic : Player
    {

        public Mystic(float x, float y, int id)
            : base(x, y, id)
        {
            upgrade.maxSpeed = 3.65f;
            guns.Add(new Flamethrower(World.BulletManager, this));
        }

        public override void Init(int t)
        {
            base.Init(t);

            if (t == 0)
            {
                characterTest = TextureManager.Map["mystic-blue"];
            }
            else
            {
                characterTest = TextureManager.Map["mystic-red"];
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }
}
