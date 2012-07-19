using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Capitalist : Player
    {
        GamePadState gamePad;
        GamePadState oldGamePad;
        KeyboardState keyboard;
        KeyboardState oldKeyboard;

        public Capitalist(float x, float y, int id)
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
                characterTest = TextureManager.Map["capitalist-blue"];
            }
            else
            {
                characterTest = TextureManager.Map["capitalist-red"];
            }
        }

        public override void Update(float deltaTime)
        {
            gamePad = GamePad.GetState(PlayerIndex.One);
            keyboard = Keyboard.GetState();

            if (gamePad.Buttons.Y == ButtonState.Pressed &&
                oldGamePad.Buttons.Y == ButtonState.Released &&
                World.inMenu == false ||
                keyboard.IsKeyDown(Keys.Tab) &&
                oldKeyboard.IsKeyUp(Keys.Tab) &&
                World.inMenu == false)
            {
                World.inMenu = true;
                World.menuManager.SwitchMenu(World.menuManager.shop);
            }
            else if (keyboard.IsKeyDown(Keys.Tab) &&
               oldKeyboard.IsKeyUp(Keys.Tab) &&
               World.inMenu == true)
            {
                World.menuManager.CurrentMenu.BackOut();
            }

            oldGamePad = gamePad;
            oldKeyboard = keyboard;
            base.Update(deltaTime);
        }
    }
}
