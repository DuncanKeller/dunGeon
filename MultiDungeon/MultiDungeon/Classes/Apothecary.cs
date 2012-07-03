using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Apothecary : Player
    {
        GamePadState gamePad;
        GamePadState oldGamePad;

        public Apothecary(float x, float y, int id)
            : base(x, y, id)
        {
            upgrade.maxSpeed = 3.7f;
            guns.Add(new Pistols(World.BulletManager, this, new Pistols(World.BulletManager, this)));
            guns.Add(new AssaultRifle(World.BulletManager, this));

            //characterTest = TextureManager.Map["powdermonkey-blue"];
        }

        public override void Init(int t)
        {
            base.Init(t);

            if (t == 0)
            {
                characterTest = TextureManager.Map["apothecary-blue"];
            }
            else
            {
                characterTest = TextureManager.Map["apothecary-red"];
            }
        }

        public override void Update(float deltaTime)
        {
            gamePad = GamePad.GetState(PlayerIndex.One);

            if (gamePad.Buttons.Y == ButtonState.Pressed &&
                oldGamePad.Buttons.Y == ButtonState.Released &&
                World.inMenu == false)
            {
                World.inMenu = true;
                World.menuManager.SwitchMenu(World.menuManager.potion);
            }

            oldGamePad = gamePad;
            base.Update(deltaTime);
        }
    }
}
