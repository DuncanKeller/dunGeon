﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace MultiDungeon.Menus
{
    public class GameLobby : Menu
    {
        string classType = "mapmaker";
        int team = 0;
        bool ready = false;
        Rectangle checkRect = new Rectangle(
                        (int)((GameConst.SCREEN_WIDTH / 20) * 14.8f), (GameConst.SCREEN_HEIGHT / 20) * 13,
                        GameConst.SCREEN_WIDTH / 12, GameConst.SCREEN_HEIGHT / 15);

        public GameLobby(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddFlavorItem("Class Type", new Vector2(1, 11));
            AddFlavorItem("_________", new Vector2(1, 12));
            AddMenuItem("Mapmaker", new Vector2(1, 13), 0,
                delegate() { classType = "mapmaker"; SendClass(); ActivateItem(0, 0); });
            AddMenuItem("Ninja", new Vector2(1, 14), 0,
                delegate() { classType = "ninja"; SendClass(); ActivateItem(1, 0); });
            AddMenuItem("Powdermonkey", new Vector2(1, 15), 0,
                delegate() { classType = "powdermonkey"; SendClass(); ActivateItem(2, 0); });
            AddMenuItem("Capitalist", new Vector2(1, 16), 0,
                delegate() { classType = "capitalist"; SendClass(); ActivateItem(3, 0); });
            AddMenuItem("Apothecary", new Vector2(1, 17), 0,
                delegate() { classType = "apothecary"; SendClass(); ActivateItem(4, 0); });
            AddMenuItem("Mystic", new Vector2(1, 18), 0,
               delegate() { classType = "mystic"; SendClass(); ActivateItem(5, 0); });

            AddFlavorItem("Team", new Vector2(7, 11));
            AddFlavorItem("_________", new Vector2(7, 12));
            AddMenuItem("Blue", new Vector2(7, 13), 1,
                delegate() { team = 0; SendTeam(); ActivateItem(0, 1); });
            AddMenuItem("Red", new Vector2(7, 14), 1,
                delegate() { team = 1; SendTeam(); ActivateItem(1, 1); });

            AddMenuItem("OK", new Vector2(15, 12), 2,
               delegate() { SendReady();  });

            AddFlavorItem("____________________________________________________________________", new Vector2(0, 9.5f));
        
            
        }

        public void SendClass()
        {
            Client.Send("class" + "\n" + World.gameId + "\n" + classType );
        }

        public void SendTeam()
        {
            Client.Send("team" + "\n" + World.gameId + "\n" + team );
        }

        public void SendReady()
        {
            ready = true;
            Client.Send("ready");
        }

        public override void Init()
        {
            yIndex = 0; xIndex = 0;
            menuItems[0][0].Select();
            ready = false;
            base.Init();
            Thread.Sleep(150);
            Client.Send("connect" + "\n" + World.gameId );
            SendClass();
        }

        public override void Update()
        {
            if (!ready)
            {
                base.Update();
            }
        }

        static Vector2 GetPos(float x, float y)
        {
            return new Vector2((GameConst.SCREEN_WIDTH / 20f) * x, (GameConst.SCREEN_HEIGHT / 20f) * y);
        }

        public Texture2D GetTexture(string classType, int t)
        {
            string team = t == 0 ? "blue" : "red";
            return TextureManager.Map[classType + "-" + team];
        }

        private void DrawPlayerIcon(SpriteBatch sb, KeyValuePair<int, PlayerInfo> player, 
            ref int spacingB, ref int spacingR)
        {
            Texture2D texture = GetTexture(player.Value.classType, player.Value.team);
            int spacingX = player.Value.team == 0 ? spacingB : spacingR;
            int spacingY = player.Value.team == World.PlayerInfo[World.gameId].team ?
                120 : 200;
            int sizex = player.Key == World.gameId ?
                100 : 50;
            int sizey = player.Key == World.gameId ?
               120 : 60;

            if (player.Key != World.gameId)
            {
                spacingY += 60;
            }

            Rectangle rect = new Rectangle(spacingX, spacingY, sizex, sizey);

            sb.Draw(TextureManager.Map["blank"], new Rectangle(spacingX + 3, spacingY + 3,
                sizex - 6, sizey - 6), new Color(5, 5, 5, 10));
            sb.Draw(texture, rect, Color.White);

            if (player.Value.team == 0)
            {
                spacingB += sizex + 20;
            }
            else
            {
                spacingR += sizex + 20;
            }
        }


        public override void Draw(SpriteBatch sb)
        {
            int spacingR = 50;
            int spacingB = 50;
            lock (World.PlayerInfo)
            {
                try
                {
                    sb.DrawString(TextureManager.Fonts["console"], "Lobby", new Vector2(
                        GameConst.SCREEN_WIDTH / 20, GameConst.SCREEN_HEIGHT / 20), Color.White);

                    var p =  new KeyValuePair<int, PlayerInfo>(World.gameId, World.PlayerInfo[World.gameId]);
                    DrawPlayerIcon(sb, p, ref spacingR, ref spacingB);

                    foreach (var player in World.PlayerInfo)
                    {
                        if (player.Key != World.gameId)
                        {
                            DrawPlayerIcon(sb, player, ref spacingR, ref spacingB);
                        }
                    }
                    
                    Texture2D checkTexture = ready ? TextureManager.Map["check-true"] : TextureManager.Map["check-false"];
                    sb.Draw(checkTexture, checkRect, Color.White);
                }
                catch (Exception e) { }
            }

            base.Draw(sb);
        }
    }
}
