using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiDungeon.HUD
{
    class UpgradeScreen
    {
        Player player;

        public void UpHealth()
        {
            var classType = player.GetType();
            if (classType == typeof(Mapmaker))
            {
                player.upgrade.maxHealth += 15;
            }
            else if (classType == typeof(Ninja))
            {
                player.upgrade.maxHealth += 10;
            }
        }

        public void UpSpeed()
        {
            var classType = player.GetType();
            if (classType == typeof(Mapmaker))
            {
                player.upgrade.speed += 1;
            }
            else if (classType == typeof(Ninja))
            {
                player.upgrade.speed += 1.5f;
            }
        }

        public void UpDamage()
        {
            var classType = player.GetType();
            if (classType == typeof(Mapmaker))
            {
                player.upgrade.damage += 8;
            }
            else if (classType == typeof(Ninja))
            {
                player.upgrade.damage += 4;
            }
        }

        public void UpReload()
        {
            var classType = player.GetType();
            if (classType == typeof(Mapmaker))
            {
                player.upgrade.reload /= 1.75f;
            }
            else if (classType == typeof(Ninja))
            {
                player.upgrade.reload /= 1.2f;
            }
        }

        public void UpClass1()
        {
            
        }

        public void UpClass2()
        {
            
        }

        public void UpClass3()
        {
            
        }
    }
}
