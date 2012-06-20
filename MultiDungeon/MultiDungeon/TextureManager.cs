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

            // items
            textures.Add("potion", c.Load<Texture2D>("Items\\potion"));

            // gun icons
            textures.Add("crossbow", c.Load<Texture2D>("Guns\\crossbow"));
            textures.Add("pistols", c.Load<Texture2D>("Guns\\pistols"));
            textures.Add("revolver", c.Load<Texture2D>("Guns\\revolver"));
            textures.Add("shotgun", c.Load<Texture2D>("Guns\\shotgun"));
            textures.Add("assault-rifle", c.Load<Texture2D>("Guns\\ar"));

            // dudes
            textures.Add("ninja-blue", c.Load<Texture2D>("Characters\\ninja-blue"));
            textures.Add("ninja-red", c.Load<Texture2D>("Characters\\ninja-red"));
            textures.Add("mapmaker-blue", c.Load<Texture2D>("Characters\\mapmaker-blue"));
            textures.Add("mapmaker-red", c.Load<Texture2D>("Characters\\mapmaker-red"));

            // menu shit
            textures.Add("menu-main", c.Load<Texture2D>("Menus\\titleScreen"));
            textures.Add("menu-default", c.Load<Texture2D>("Menus\\menu"));

            // fonts
            fonts.Add("console", c.Load<SpriteFont>("Fonts\\ConsoleFont"));
        }
    }
}
