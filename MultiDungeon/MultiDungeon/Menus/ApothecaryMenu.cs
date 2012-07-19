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
        int index1;
        int index2;
        IngredientList ingredients;
        Texture2D ingredient;

        Dictionary<Type, IngredientList> cost = new Dictionary<Type, IngredientList>();
        List<MenuItem> costItems = new List<MenuItem>();

        MenuItem ingredientRed;
        MenuItem ingredientBlue;
        MenuItem ingredientGreen;

        string invincibility = "grants 8 secodns of invincibility \nno attack can harm you";
        string confusion = "makes you appear to be in multiple places at once\nlasts 45 seconds";
        string warp = "warps you to team room\ngood for a quick getaway";
        string midas = "you earn twice as much for killing opponents\nlasts 30 seconds";
        string vampire = "leech health from sucessful hits\nlasts 30 seconds";
        string curse = "you will posin any opponent who kills you \nlasts 30 seconds";

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

            cost.Add(typeof(InvincibilityPotion), invincibility);
            cost.Add(typeof(ConfusionPotion), confusion);
            cost.Add(typeof(WarpPotion), warp);
            cost.Add(typeof(MidasPotion), midas);
            cost.Add(typeof(VampirePotion), vampire);
            cost.Add(typeof(CursePotion), curse);

            ingredients.red = 10;
            ingredients.blue = 10;
            ingredients.green = 10;
        }

        public void InitMenuItems()
        {
            player = World.Player;
            menuItems.Clear();
            nonSelectableItems.Clear();
            //if (!initialized)
            {
                // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
                AddFlavorItem("Potion Menu", new Vector2(1, 3));
                if (player.Item != null)
                {
                    AddMenuItem("Discard current item for ingredient", new Vector2(2, 4), 0,
                        delegate() { CreateIndgredient(); });
                    AddFlavorItem(player.Item.Texture, new Vector2(15, 4), 1, 1);
                    AddFlavorItem("=>", new Vector2(16, 4));
                    AddFlavorItem(ingredient, new Vector2(17, 4), 1, 1);
                    nonSelectableItems[3].ChangeColor(GetColor());
                }

                AddMenuItem("Invincibility Potion", new Vector2(3, 5), 0,
                        delegate() { BuyPotion(new InvincibilityPotion()); });
                AddMenuItem("Confusion Potion", new Vector2(3, 6), 0,
                        delegate() { BuyPotion(new ConfusionPotion()); });
                AddMenuItem("Warp Potion", new Vector2(3, 7), 0,
                        delegate() { BuyPotion(new WarpPotion()); });
                AddMenuItem("Midas Potion", new Vector2(3, 8), 0,
                        delegate() { BuyPotion(new MidasPotion()); });
                AddMenuItem("Vampire Potion", new Vector2(3, 9), 0,
                        delegate() { BuyPotion(new VampirePotion()); });
                AddMenuItem("Curse Potion", new Vector2(3, 10), 0,
                        delegate() { BuyPotion(new CursePotion());  });

                AddFlavorItem("______________________________________________",
                    new Vector2(0, 13));
                AddFlavorItem("Cost: ",
                    new Vector2(2, 14));
                AddFlavorItem("",
                    new Vector2(2, 15));

                MenuItem list1 = new MenuItem(ingredient, new Vector2(13, 7), 2, 2, null);
                MenuItem list2 = new MenuItem(ingredient, new Vector2(13, 9), 2, 2, null);
                MenuItem list3 = new MenuItem(ingredient, new Vector2(13, 11), 2, 2, null);
                list1.ChangeColor(Color.Red);
                list2.ChangeColor(Color.Green);
                list3.ChangeColor(Color.Blue);

                nonSelectableItems.Add(list1);
                nonSelectableItems.Add(list2);
                nonSelectableItems.Add(list3);

                MenuItem item1 = new MenuItem("X " + ingredients.red, new Vector2(15, 7.8f), null);
                MenuItem item2 = new MenuItem("X " + ingredients.green, new Vector2(15, 9.8f), null);
                MenuItem item3 = new MenuItem("X " + ingredients.blue, new Vector2(15, 11.8f), null);

                ingredientRed = item1;
                ingredientBlue = item2;
                ingredientGreen = item3;

                nonSelectableItems.Add(item1);
                nonSelectableItems.Add(item2);
                nonSelectableItems.Add(item3);

                index1 = player.Item != null ? 1 : 0;
                index2 = player.Item != null ? 3 : 0;

                menuItems[0][0].Selected = true;
                yIndex = 0;
                //initialized = true;
            }

        }

        public void UpdateIngredients()
        {
            ingredientRed.ChangeText("X " + ingredients.red);
            ingredientBlue.ChangeText("X " + ingredients.green);
            ingredientGreen.ChangeText("X " + ingredients.blue);
        }

        private void BuyPotion(Item i)
        {
            if (player != null)
            {
                if (player.Item == null)
                {
                    if (ingredients.blue >= cost[i.GetType()].blue &&
                        ingredients.red >= cost[i.GetType()].red &&
                        ingredients.green >= cost[i.GetType()].green)
                    {
                        ingredients.blue -= cost[i.GetType()].blue;
                        ingredients.red -= cost[i.GetType()].red;
                        ingredients.green -= cost[i.GetType()].green;
                        player.Item = i;
                        Close();
                    }
                }
            }
            UpdateIngredients();
        }

        private void ChangeText(string s)
        {
            nonSelectableItems[3 + index2].ChangeText(s);
        }

        private void SetCostItems(Type potion)
        {
            if (cost.Count > 0)
            {
                foreach (MenuItem item in costItems)
                {
                    nonSelectableItems.Remove(item);
                }

                costItems.Clear();
                int num = 0;
                if (potion != null)
                {
                    for (int i = 0; i < cost[potion].red; i++)
                    {
                        MenuItem item = new MenuItem(ingredient, new Vector2(4 + num, 14), 1, 1, null);
                        item.ChangeColor(Color.Red);
                        costItems.Add(item);
                        num++;
                    }
                    for (int i = 0; i < cost[potion].green; i++)
                    {
                        MenuItem item = new MenuItem(ingredient, new Vector2(4 + num, 14), 1, 1, null);
                        item.ChangeColor(Color.Green);
                        costItems.Add(item);
                        num++;
                    }
                    for (int i = 0; i < cost[potion].blue; i++)
                    {
                        MenuItem item = new MenuItem(ingredient, new Vector2(4 + num, 14), 1, 1, null);
                        item.ChangeColor(Color.Blue);
                        costItems.Add(item);
                        num++;
                    }
                }

                foreach (MenuItem item in costItems)
                {
                    nonSelectableItems.Add(item);
                }
            }
        }

        public override void BackOut()
        {
            base.BackOut();
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
            nonSelectableItems[1].hidden = true;
            nonSelectableItems[2].hidden = true;
            nonSelectableItems[3].hidden = true;
            UpdateIngredients();
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
            if (menuItems[0][index1 + 0].Selected)
            { ChangeText(invincibility); SetCostItems(typeof(InvincibilityPotion)); }
            if (menuItems[0][index1 + 1].Selected)
            { ChangeText(confusion); SetCostItems(typeof(ConfusionPotion)); }
            if (menuItems[0][index1 + 2].Selected)
            { ChangeText(warp); SetCostItems(typeof(WarpPotion)); }
            if (menuItems[0][index1 + 3].Selected)
            { ChangeText(midas); SetCostItems(typeof(MidasPotion)); }
            if (menuItems[0][index1 + 4].Selected)
            { ChangeText(vampire); SetCostItems(typeof(VampirePotion)); }
            if (menuItems[0][index1 + 5].Selected)
            { ChangeText(curse); SetCostItems(typeof(CursePotion)); }

            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
