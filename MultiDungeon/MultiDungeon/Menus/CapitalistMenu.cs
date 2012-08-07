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
    public class CapitalistMenu : Menu
    {
        Player player = null;
        bool initialized = false;

        public CapitalistMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            
        }

        public void InitMenuItems()
        {
            player = World.Player;
            if (!initialized)
            {
                // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
                AddFlavorItem("Weapons", new Vector2(2, 4));
                AddFlavorItem("________", new Vector2(2, 5));

                AddMenuItem("100G -Revolver", new Vector2(2, 6), 0,
                    delegate() { BuyGun(new Revolver(World.BulletManager, player), 100); });
                AddMenuItem("150G -Sawed-Off Shotgun", new Vector2(2, 7), 0,
                   delegate() { BuyGun(new Shotgun(World.BulletManager, player), 150); });
                AddMenuItem("150G -Auto Shotgun", new Vector2(2, 8), 0,
                   delegate() { BuyGun(new Shotgun(World.BulletManager, player), 150); });
                AddMenuItem("200G -Crossbow", new Vector2(2, 9), 0,
                   delegate() { BuyGun(new Crossbow(World.BulletManager, player), 200); });
                AddMenuItem("200G -Grenade Launcher", new Vector2(2, 10), 0,
                   delegate() { BuyGun(new GrenadeLauncher(World.BulletManager, player), 200); });
                AddMenuItem("250G -Rocket Launcher", new Vector2(2, 11), 0,
                   delegate() { BuyGun(new RocketLauncher(World.BulletManager, player), 250); });

                AddFlavorItem("Items", new Vector2(12, 4));
                AddFlavorItem("________", new Vector2(12, 5));

                AddMenuItem("100G -Health Potion", new Vector2(12, 6), 1,
                   delegate() { BuyItem(new HealthPotion(HealingLevel.regular), 100); });
                AddMenuItem("75G -Stoneskin Potion", new Vector2(12, 7), 1,
                  delegate() { BuyItem(new Stoneskin(), 75); });
                AddMenuItem("75G -The Acumen", new Vector2(12, 8), 1,
                 delegate() { BuyItem(new SeeingEye(), 75); });
                AddMenuItem("50G -Juice", new Vector2(12, 9), 1,
                  delegate() { BuyItem(new Juice(), 50); });
                AddMenuItem("50G -Spare Mag", new Vector2(12, 10), 1,
                  delegate() { BuyItem(new SpareMag(), 50); });
                initialized = true;
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
            if (player.Gold >= price)
            {
                SoundManager.PlaySound("purchase");
                player.Gold -= price;
                player.Guns.Add(g);
            }
        }

        public void BuyItem(Item i, int price)
        {
            if (player.Item == null)
            {
                if (player.Gold >= price)
                {
                    SoundManager.PlaySound("purchase");
                    player.Gold -= price;
                    player.Item = i;
                }
                
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
            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
