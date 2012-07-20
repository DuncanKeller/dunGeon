using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon.Traps
{
    abstract class Trap
    {
        public abstract void Update(float deltaTime);

        public abstract void Draw(SpriteBatch sb);
    }
}
