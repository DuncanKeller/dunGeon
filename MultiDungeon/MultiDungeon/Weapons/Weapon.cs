using System;
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

        public abstract void Update();

        public abstract void Draw();
    }
}
