using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    class Dart : Bullet
    {
        public Dart()
            : base()
        {
            speed = 600;
        }

        public new Rectangle Rect
        {
            get { return new Rectangle((int)pos.X - 2, (int)pos.Y - 8, 4, 16); }
        }

        public override void Draw(SpriteBatch sb)
        {
            Texture2D texture = TextureManager.Map["dart"];
            Rectangle source = new Rectangle(0, 0, texture.Width, texture.Height);
            sb.Draw(texture, Rect, source, Color.White, angle + (float)(Math.PI / 2),
                new Vector2(0, 0), SpriteEffects.None, 0);
        }
    }
}
