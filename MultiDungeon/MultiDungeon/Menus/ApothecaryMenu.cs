using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MultiDungeon.Items;

namespace MultiDungeon.Menus
{
    public struct IngredientList
    {
        public int blue;
        public int red;
        public int green;
    }

    public class ApothecaryMenu : Menu
    {
        Player player = null;
        bool initialized = false;
        IngredientList ingredients;
        Texture2D ingredient;

        Dictionary<Type, IngredientList> cost = new Dictionary<Type, IngredientList>();

        string invincibility = "grants 8 secodns of invincibility.\n no attack can harm you.";
        string confusion = "makes you appear to be in multiple places at once to opponents.\n lasts 45 seconds.";
        string warp = "warps you to team room.\n good for a quick getaway";
        string midas = "you earn twice as much for killing opponents\n for 30 secodns";
        string vampire = "leech health from sucessful hits\n lasts 30 seconds";
        string curse = "you will posin any opponent who kills you for 30 seconds.";

        public ApothecaryMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            ingredient = TextureManager.Map["ingredient"];

            IngredientList invincibility = new IngredientList();
            IngredientList confusion = new IngredientList();
            IngredientList warp = new IngredientList();
            IngredientList midas = new IngredientList();
            IngredientList vampire = new IngredientList();
            IngredientList curse = new IngredientList();
            invincibility.red = 3;
            confusion.green = 1; confusion.blue = 1;
            warp.blue = 2;
            midas.red = 1; midas.blue = 1;
            vampire.red = 1; vampire.green = 1;
            curse.red = 1; curse.green = 1; curse.blue = 1;
        }

        public void InitMenuItems()
        {
            player = World.Player;
            if (!initialized)
            {
                // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
                AddFlavorItem("Potion Menu", new Vector2(1, 1));
                if (player.Item != null)
                {
                    AddMenuItem("Discard current item for ingredient", new Vector2(2, 2), 0,
                        delegate() { CreateIndgredient(); });
                    AddFlavorItem(player.Item.Texture, new Vector2(2, 15), 1, 1);
                    AddFlavorItem("=>", new Vector2(2, 16));
                    AddFlavorItem(ingredient, new Vector2(2, 16), 1, 1);
                    nonSelectableItems[3].ChangeColor(GetColor());
                }

                AddMenuItem("Invincibility Potion", new Vector2(4, 2), 0,
                        delegate() {  });
                AddMenuItem("Confusion Potion", new Vector2(4, 3), 0,
                        delegate() {  });
                AddMenuItem("Warp Potion", new Vector2(4, 4), 0,
                        delegate() {  });
                AddMenuItem("Midas Potion", new Vector2(4, 5), 0,
                        delegate() {  });
                AddMenuItem("Vampire Potion", new Vector2(4, 6), 0,
                        delegate() {  });
                AddMenuItem("Curse Potion", new Vector2(4, 7), 0,
                        delegate() {   });

                AddFlavorItem("______________________________________________",
                    new Vector2(0, 9));
                AddFlavorItem("Cost: ",
                    new Vector2(2, 10));
                AddFlavorItem("",
                    new Vector2(2, 11));
            }

        }

        private void BuyPotion(Item i)
        {
            if (player.Item == null)
            {
                if (ingredients.blue <= cost[i.GetType()].blue &&
                    ingredients.red <= cost[i.GetType()].red &&
                    ingredients.green <= cost[i.GetType()].green)
                {
                    ingredients.blue -= cost[i.GetType()].blue;
                    ingredients.red -= cost[i.GetType()].red;
                    ingredients.green -= cost[i.GetType()].green;
                    player.Item = i;
                    Close();
                }
            }
        }

        private void ChangeText(string s)
        {
            int index = player.Item != null ? 3 : 0;

            nonSelectableItems[2 + index].ChangeText(s);
        }

        public override void BackOut()
        {
            Close();
            World.inMenu = false;
        }

        public void BuyGun(Gun g, int price)
        {
            player.Guns.Add(g);
        }

        public Color GetColor()
        {
            if (player.Item != null)
            {
                Item i = player.Item;
                if (i is HealthPotion ||
                    i is SeeingEye)
                {
                    return Color.Red;
                }
                else if (i is SpareMag)
                {
                    return Color.Blue;
                }
                else if (i is Juice ||
                    i is Stoneskin)
                {
                    return Color.Green;
                }
                player.Item = null;
            }
            return Color.White;
        }

        public void CreateIndgredient()
        {
            if (player.Item != null)
            {
                Item i = player.Item;
                if (i is HealthPotion ||
                    i is SeeingEye)
                {
                    ingredients.red++;
                }
                else if (i is SpareMag)
                {
                    ingredients.blue++;
                }
                else if (i is Juice ||
                    i is Stoneskin)
                {
                    ingredients.green++;
                }
                player.Item = null;
            }
        }
      

        public void Open(Player p)
        {
            player = p;
        }

        public void Close()
        {
            player = null;
        }

        public override void Init()
        {
            InitMenuItems();

            yIndex = 0; xIndex = 0;
            menuItems[0][0].Select();
            base.Init();
        }

        public override void Update()
        {
            int index = player.Item != null ? 1 : 0;
            if (menuItems[0][index + 0].Selected)
            { ChangeText(invincibility); }
            if (menuItems[0][index + 1].Selected)
            { ChangeText(confusion); }
            if (menuItems[0][index + 2].Selected)
            { ChangeText(warp); }
            if (menuItems[0][index + 3].Selected)
            { ChangeText(midas); }
            if (menuItems[0][index + 4].Selected)
            { ChangeText(vampire); }
            if (menuItems[0][index + 5].Selected)
            { ChangeText(curse); }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
