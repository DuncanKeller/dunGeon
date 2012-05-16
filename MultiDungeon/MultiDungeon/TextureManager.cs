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
        static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();

        public static Dictionary<string, Texture2D> Map
        {
            get { return textures; }
        }
        public static Dictionary<string, SpriteFont> Fonts
        {
            get { return fonts; }
        }

        public static void Initialize(ContentManager c)
        {
            // textures
            textures.Add("blank", c.Load<Texture2D>("white"));
            textures.Add("circle", c.Load<Texture2D>("circle"));
            textures.Add("tile", c.Load<Texture2D>("tile"));
            textures.Add("chest-closed", c.Load<Texture2D>("Items\\chestClosed"));
            textures.Add("chest-open", c.Load<Texture2D>("Items\\chestOpen"));

            // fonts
            fonts.Add("console", c.Load<SpriteFont>("Fonts\\ConsoleFont"));
        }
    }
}
