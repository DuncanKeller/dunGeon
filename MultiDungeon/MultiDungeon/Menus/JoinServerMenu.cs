using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json.Linq;


namespace MultiDungeon.Menus
{
    public class JoinServerMenu : Menu
    {
        List<string> names = new List<string>();
        List<string> ips = new List<string>();

        public JoinServerMenu(Game1 g, MenuManager mm)
            : base(g, mm)
        {
            LoadServerInfo();
            // pos is proportional to screen size (IE, SCREENWIDTH / 20 * pos)
            AddMenuItem("Join Server", new Vector2(1, 2), 0,
                delegate() { });
            AddMenuItem("Quick Join", new Vector2(1, 3), 0,
                delegate() { menuManager.SwitchMenu(menuManager.quickJoin); });
            AddMenuItem("Add Server", new Vector2(1, 4), 0,
                delegate() { menuManager.SwitchMenu(menuManager.enterServer); });

          
            menuItems[0][0].Select();
        }

        public override void BackOut()
        {
            base.BackOut();
            menuManager.SwitchMenu(menuManager.main);
        }

        public void ConnectToServer(string ip)
        {
            if (World.InitNetwork(ip))
            {
                Console.Write("Connected to Server", MessageType.special);
                menuManager.SwitchMenu(menuManager.lobby);
            }
            else
            {
                menuManager.SwitchMenu(menuManager.failure);
            }
        }

        public override void Init()
        {
            if (menuManager.enterServer.Name != "")
            {
                names.Add(menuManager.enterServer.Name);
                ips.Add(menuManager.enterServer.IP);
            }
            if (menuItems.Count > 1)
            {
                menuItems[1].Clear();
            }
            for (int i = 0; i < names.Count; i++)
            {
                AddMenuItem(names[i], new Vector2(10, 2 + (i * 2)), 1,
                delegate() { ConnectToServer(ips[yIndex]); });
            }
            for (int i = 0; i < ips.Count; i++)
            {
                AddFlavorItem("> " + ips[i], new Vector2(10, 3 + (i * 2)) );
            }

            SaveServerInfo();

            base.Init();
        }

        public override void Update()
        {
            base.Update();
        }

        public void LoadServerInfo()
        {
            if(File.Exists(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData) + "\\dunGeon\\servers.json"))
            {
                StreamReader sr = new StreamReader(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData) + "\\dunGeon\\servers.json");
                string unparsedJson = sr.ReadToEnd();
                sr.Close();
                
                JObject json = JObject.Parse(unparsedJson);

                JArray serverN = (JArray)json["ServerNames"];

                for (int i = 0; i < serverN.Count; i++)
                {
                    string name = (string)serverN[i];
                    names.Add(name);
                }

                JArray serverIP = (JArray)json["ServerIPs"];

                for (int i = 0; i < serverIP.Count; i++)
                {
                    string ip = (string)serverIP[i];
                    ips.Add(ip);
                }
                
            }
           
        }

        public void SaveServerInfo()
        {
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\dunGeon"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\dunGeon");
            }

            FileStream fs = File.Open(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData) + "\\dunGeon\\servers.json", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            JsonWriter jw = new JsonTextWriter(sw);
            jw.Formatting = Formatting.Indented;

            jw.WriteStartObject();

            jw.WritePropertyName("ServerNames");
            jw.WriteStartArray();
            for (int i = 0; i < names.Count; i++)
            {
                jw.WriteValue(names[i]);
            }
            jw.WriteEndArray();

            jw.WritePropertyName("ServerIPs");
            jw.WriteStartArray();
            for (int i = 0; i < ips.Count; i++)
            {
                jw.WriteValue(ips[i]);
            }
            jw.WriteEndArray();

            jw.WriteEndObject();
            jw.Close();

        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
        }
    }
}
