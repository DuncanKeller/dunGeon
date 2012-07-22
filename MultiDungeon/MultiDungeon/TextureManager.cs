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
            textures.Add("floor", c.Load<Texture2D>("floor"));
            textures.Add("chest-closed", c.Load<Texture2D>("Items\\chestClosed"));
            textures.Add("chest-open", c.Load<Texture2D>("Items\\chestOpen"));

            // icons
            textures.Add("cursor", c.Load<Texture2D>("Icons\\cursor"));
            textures.Add("countdown-1", c.Load<Texture2D>("Icons\\countdown1"));
            textures.Add("countdown-2", c.Load<Texture2D>("Icons\\countdown2"));
            textures.Add("countdown-3", c.Load<Texture2D>("Icons\\countdown3"));
            textures.Add("countdown-go", c.Load<Texture2D>("Icons\\countdownGo"));
            textures.Add("health-icon", c.Load<Texture2D>("Icons\\health"));
            textures.Add("gold-icon", c.Load<Texture2D>("Icons\\coin"));
            textures.Add("health-bar", c.Load<Texture2D>("Icons\\healthbar"));
            textures.Add("gold-bar", c.Load<Texture2D>("Icons\\goldbar"));

            // items
            textures.Add("potion", c.Load<Texture2D>("Items\\potion"));
            textures.Add("stoneskin", c.Load<Texture2D>("Items\\stoneskin"));
            textures.Add("juice", c.Load<Texture2D>("Items\\juice"));
            textures.Add("eye", c.Load<Texture2D>("Items\\eye"));
            textures.Add("cig", c.Load<Texture2D>("Items\\cig"));
            textures.Add("mag", c.Load<Texture2D>("Items\\mag"));
            textures.Add("ingredient", c.Load<Texture2D>("Items\\ingredients"));

            // traps
            textures.Add("spikes-low", c.Load<Texture2D>("Traps\\spikes-tiny"));
            textures.Add("spikes-high", c.Load<Texture2D>("Traps\\spikes-large"));
            textures.Add("spikes-retracted", c.Load<Texture2D>("Traps\\spikes-retracted"));

            // potions
            textures.Add("potion-confuse", c.Load<Texture2D>("Items\\Potions\\confusion"));
            textures.Add("potion-midas", c.Load<Texture2D>("Items\\Potions\\midas"));
            textures.Add("potion-curse", c.Load<Texture2D>("Items\\Potions\\curse"));
            textures.Add("potion-invincibility", c.Load<Texture2D>("Items\\Potions\\invincibility"));
            textures.Add("potion-vampire", c.Load<Texture2D>("Items\\Potions\\vampire"));
            textures.Add("potion-warp", c.Load<Texture2D>("Items\\Potions\\warp"));

            // gun icons
            textures.Add("crossbow", c.Load<Texture2D>("Guns\\crossbow"));
            textures.Add("pistols", c.Load<Texture2D>("Guns\\pistols"));
            textures.Add("revolver", c.Load<Texture2D>("Guns\\revolver"));
            textures.Add("shotgun", c.Load<Texture2D>("Guns\\shotgun"));
            textures.Add("assault-rifle", c.Load<Texture2D>("Guns\\ar"));
            textures.Add("sword", c.Load<Texture2D>("Guns\\sword"));
            textures.Add("grenade-launcher", c.Load<Texture2D>("Guns\\grenadeLauncher"));
            textures.Add("rocket-launcher", c.Load<Texture2D>("Guns\\rocketLauncher"));
            textures.Add("dartgun", c.Load<Texture2D>("Guns\\dartGun"));
            textures.Add("flames", c.Load<Texture2D>("Guns\\fire"));

            // bullets
            textures.Add("flame", c.Load<Texture2D>("Effects\\explosion2"));
            textures.Add("grenade", c.Load<Texture2D>("Bullets\\grenade"));
            textures.Add("rocket", c.Load<Texture2D>("Bullets\\rocket"));
            textures.Add("bullet", c.Load<Texture2D>("Bullets\\silver-bullet"));
            textures.Add("dart", c.Load<Texture2D>("Bullets\\dart"));

            // dudes
            textures.Add("ninja-blue", c.Load<Texture2D>("Characters\\ninja-blue"));
            textures.Add("ninja-red", c.Load<Texture2D>("Characters\\ninja-red"));
            textures.Add("mapmaker-blue", c.Load<Texture2D>("Characters\\mapmaker-blue"));
            textures.Add("mapmaker-red", c.Load<Texture2D>("Characters\\mapmaker-red"));
            textures.Add("powdermonkey-blue", c.Load<Texture2D>("Characters\\powdermonkey-blue"));
            textures.Add("powdermonkey-red", c.Load<Texture2D>("Characters\\powdermonkey-red"));
            textures.Add("capitalist-blue", c.Load<Texture2D>("Characters\\capitalist-blue"));
            textures.Add("capitalist-red", c.Load<Texture2D>("Characters\\capitalist-red"));
            textures.Add("apothecary-blue", c.Load<Texture2D>("Characters\\apothecary-blue"));
            textures.Add("apothecary-red", c.Load<Texture2D>("Characters\\apothecary-red"));
            textures.Add("mystic-blue", c.Load<Texture2D>("Characters\\mystic-blue"));
            textures.Add("mystic-red", c.Load<Texture2D>("Characters\\mystic-red"));

            // effects
            textures.Add("explosion-lens", c.Load<Texture2D>("Effects\\explosion1"));
            textures.Add("explosion-smoke", c.Load<Texture2D>("Effects\\explosion2"));
            textures.Add("explosion-circle", c.Load<Texture2D>("Effects\\explosion3"));
            textures.Add("bubble", c.Load<Texture2D>("Effects\\bubble"));
            textures.Add("purpleBubble", c.Load<Texture2D>("Effects\\purpleBubble"));
            textures.Add("star", c.Load<Texture2D>("Effects\\star"));
            textures.Add("sparkle", c.Load<Texture2D>("Effects\\sparkle"));
            textures.Add("cross", c.Load<Texture2D>("Effects\\cross"));
            textures.Add("slice", c.Load<Texture2D>("Effects\\sword-slice"));

            // menu shit
            textures.Add("menu-main", c.Load<Texture2D>("Menus\\titleScreen"));
            textures.Add("menu-default", c.Load<Texture2D>("Menus\\menu"));

            // fonts
            fonts.Add("console", c.Load<SpriteFont>("Fonts\\ConsoleFont"));
        }
    }
}
