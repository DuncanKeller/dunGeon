using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Bullet
    {
        Vector2 pos;
        float angle;

        public Bullet()
        {
           
        }

        public void Init(Vector2 pos, float angle)
        {
            this.pos = pos;
            this.angle = angle;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            
        }
    }
}
