using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MultiDungeon
{
    public class TextureManager
    {
        static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public static Dictionary<string, Texture2D> Map
        {
            get { return textures; }
        }

        public static void Initialize(ContentManager c)
        {
            textures.Add("blank", c.Load<Texture2D>("white"));
            textures.Add("circle", c.Load<Texture2D>("circle"));
            textures.Add("tile", c.Load<Texture2D>("tile"));
        }
    }
}
