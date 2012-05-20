using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Ninja : Player
    {
        int visibility;
        int maxInvis = 30;
        float invisTimer;
        int maxTimer = 1000;
        public Ninja(float x, float y, int id)
            : base(x, y, id)
        {

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
                GamePad.GetState(playerIndex).Triggers.Right > 0.1)
            {
                invisTimer = 0;
                visibility = 255;
            }
            c.A = (byte)visibility;
            base.Update(deltaTime);
        }
    }
}
