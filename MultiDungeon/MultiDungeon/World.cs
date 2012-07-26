using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MultiDungeon.Map;
using System.Timers;
using MultiDungeon.Items;
using MultiDungeon.Menus;
using MultiDungeon.Traps;

namespace MultiDungeon
{
    struct PlayerInfo
    {
        public string classType;
        public int team;
        public bool ready;
    }

    struct Endgame
    {
        public int[] teamGold;
        public int maxTeamGold;
    }

    class World
    {
        static ContentManager content;
        public static Random rand = new Random();
        public static int gameId;
        public static MenuManager menuManager;
        public static bool inMenu = false;
        public static Endgame endgame;
        static Dictionary<int, Player> players = new Dictionary<int, Player>();
        static TileSet map;
        static int timer = 0;
        static BulletManager bulletManager = new BulletManager();
        static ItemManager itemManager = new ItemManager();
        static TrapManager trapManager = new TrapManager();
        static Camera cam;
        static Game1 game;
        static float countdown;
        
        static Dictionary<int, PlayerInfo> playerInfo = new Dictionary<int, PlayerInfo>();

        public static Dictionary<int, PlayerInfo> PlayerInfo
        {
            get { return playerInfo; }
            set { playerInfo = value; }
        }

        public static Game1 Game
        {
            get { return game; }
            set { game = value; }
        }

        public static bool ReadyToPlay
        {
            get { return countdown <= 0; }
        }

        public static float Countdown
        {
            get { return countdown; }
        }

        public static BulletManager BulletManager
        {
            get { return bulletManager; }
        }

        public static TrapManager TrapManager
        {
            get { return trapManager; }
        }

        public static ItemManager ItemManager
        {
            get { return itemManager; }
        }

        public static Camera Camera
        {
            get { return cam; }
        }

        public static Player Player
        {
            get { return players[gameId]; }
            set { players[gameId] = value; }
        }

        public static List<Player> Players
        {
            get { return new List<Player>(players.Values); }
        }

        public static Dictionary<int, Player> PlayerHash
        {
            get { return players; }
        }

        public static TileSet Map
        {
            get { return map; }
        }

        public static void Init(GraphicsDeviceManager g, ContentManager c, Game1 g1)
        {
            game = g1;
            map = new TileSet();
            cam = new Camera(g);
            menuManager = new MenuManager(game);
            content = c;

            InitEndgameStruct();
        }

        private static void InitEndgameStruct()
        {
            endgame.teamGold = new int[2];
            endgame.maxTeamGold = 100;
        }

        public static void StartGame(string filename)
        {
            if (filename == "random")
            {
                //map.GenerateMap(45, 45);
                map.GenerateDungeon(45, 45);
            }
            else
            {
                map.ReadMap(filename, content);
            }
            Player.Spawn();
            MultiDungeon.HUD.Map.Init(World.Map);
            game.state = Game1.GameState.game;
            countdown = 3;
        }

        #region NetworkClient
        public static void RecieveData(string[] datum)
        {
            foreach (string data in datum)
            {
                if (data == String.Empty)
                {
                    continue;
                }
                string[] info = data.Split("\n".ToCharArray());
                try
                {
                    if (info[0] == "id")
                    {
                        int id = Int32.Parse(info[1]);

                        gameId = id;
                        //players.Add(gameId, new Mapmaker(100, 100, gameId));
                        playerInfo.Add(gameId, new PlayerInfo());
                    }
                    else if (info[0] == "connect")
                    {
                        lock (PlayerInfo)
                        {
                            int id = Int32.Parse(info[1]);
                            if (!playerInfo.ContainsKey(id))
                            {
                                PlayerInfo.Add(id, new PlayerInfo());
                                Client.Send("response" + "\n" + gameId);
                                Client.Send("team" + "\n" + gameId + "\n" + playerInfo[gameId].team);
                                Client.Send("class" + "\n" + gameId + "\n" + playerInfo[gameId].classType);
                            }
                        }
                    }
                    else if (info[0] == "response")
                    {
                        lock (PlayerInfo)
                        {
                            int id = Int32.Parse(info[1]);
                            if (!playerInfo.ContainsKey(id))
                            {
                                PlayerInfo.Add(id, new PlayerInfo());
                            }
                        }
                    }
                    else if (info[0] == "kill")
                    {
                        lock (players)
                        {
                            int id = Int32.Parse(info[1]);
                            if (!playerInfo.ContainsKey(id))
                            {
                                PlayerHash[id].Die();
                            }
                        }
                    }
                    else if (info[0] == "start")
                    {
                        foreach (var pi in playerInfo)
                        {
                            Player p = null;
                            switch (pi.Value.classType)
                            {
                                case "mapmaker":
                                    p = new Mapmaker(0, 0, pi.Key);
                                    break;
                                case "ninja":
                                    p = new Ninja(0, 0, pi.Key);
                                    break;
                                case "powdermonkey":
                                    p = new Powdermonkey(0, 0, pi.Key);
                                    break;
                                case "capitalist":
                                    p = new Capitalist(0, 0, pi.Key);
                                    break;
                                case "apothecary":
                                    p = new Apothecary(0, 0, pi.Key);
                                    break;
                                case "mystic":
                                    p = new Mystic(0, 0, pi.Key);
                                    break;
                            }
                            p.Init(pi.Value.team);
                            PlayerHash.Add(pi.Key, p);
                        }
                        StartGame(info[1]);
                    }
                    else if (info[0] == "xbox")
                    {
                        int controllerNum = Int32.Parse(info[1]);
                        switch (controllerNum)
                        {
                            case 0:
                                Player.playerIndex = PlayerIndex.One;
                                break;
                            case 1:
                                Player.playerIndex = PlayerIndex.Two;
                                break;
                            case 2:
                                Player.playerIndex = PlayerIndex.Three;
                                break;
                            case 3:
                                Player.playerIndex = PlayerIndex.Four;
                                break;
                        }

                    }
                    else if (info[0] == "class")
                    {
                        int id = Int32.Parse(info[1]);
                        string classType = info[2];
                        PlayerInfo pi = playerInfo[id];
                        pi.classType = classType;
                        playerInfo[id] = pi;
                    }
                    else if (info[0] == "team")
                    {
                        int id = Int32.Parse(info[1]);
                        int team = Int32.Parse(info[2]);
                        PlayerInfo pi = playerInfo[id];
                        pi.team = team;
                        playerInfo[id] = pi;
                    }
                    else if (info[0] == "reset")
                    {
                        foreach (int key in players.Keys)
                        {
                            players[key].Spawn();
                            players[key].Gold = 0;
                            players[key].Item = null;
                        }
                        itemManager.RemoveChests();
                        map.Populate();
                    }
                    else if (info[0] == "disconnect")
                    {
                        int id = Int32.Parse(info[1]);
                        lock (players)
                        {
                            players.Remove(id);
                        }
                        lock (playerInfo)
                        {
                            playerInfo.Remove(id);
                        }
                    }
                    else if (info[0] == "maxed")
                    {
                        Client.Close();
                        // goto maxed out menu
                    }
                    else if (info[0] == "p")
                    {
                        int id = Int32.Parse(info[1]);
                        if (players.ContainsKey(id))
                        {
                            if (id != gameId)
                            {
                                players[id].SetPos(int.Parse(info[2]), int.Parse(info[3]));
                                players[id].Angle = float.Parse(info[4]);
                            }
                        }
                    }
                    else if (info[0] == "b")
                    {
                        int id = Int32.Parse(info[1]);
                        Type type = Type.GetType(info[2]);
                        float damage = float.Parse(info[3]);

                        if (gameId != id)
                        {
                            Bullet b = (Bullet)Activator.CreateInstance(type);
                            Vector2 p = new Vector2(players[id].DrawRect.X, players[id].DrawRect.Y);
                            b.Init(p, players[id].Angle - (float)(Math.PI / 2), damage, id);
                            bulletManager.Add(b);
                        }
                    }
                    else if (info[0] == "slice")
                    {
                        int id = Int32.Parse(info[1]);

                        if (gameId != id)
                        {
                            (players[id] as Ninja).Sword.Slice(
                                players[id].Angle, players[id].Position);
                        }
                    }
                    else if (info[0] == "chest")
                    {
                        int id = Int32.Parse(info[1]);
                        itemManager.OpenChest(id);
                    }
                    else if (info[0] == "rand")
                    {
                        int seed = Int32.Parse(info[1]);
                        GameConst.rand = new Random(seed);
                    }
                    else if (info[0] == "teamgold")
                    {
                        int team = Int32.Parse(info[1]);
                        int gold = Int32.Parse(info[2]);
                        endgame.teamGold[team] += gold;
                        menuManager.teamChest.UpdateGoldAmnt();
                    }
                    else
                    {
                        throw new Exception("command \"" + info[0] + "\" not found");
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message, MessageType.urgent);
                }

                // print all non-position commands to the console
                if (info[0] != "p")
                {
                    string message = "";

                    for (int i = 0; i < info.Length; i++)
                    {
                        message += info[i];
                        if (i < info.Length - 1)
                        {
                            message += ", ";
                        }
                    }
                    Console.Write(message);
                }
            }
        }
        #endregion

        private static void CheckEndgame()
        {
            int gameType = 0;
            bool end = false;

            switch (gameType)
            {
                case 0:
                    if (endgame.teamGold[0] >= endgame.maxTeamGold ||
                        endgame.teamGold[1] >= endgame.maxTeamGold)
                    {
                        game.Menu.endgame.SetTeam(endgame.teamGold[0] > endgame.teamGold[1] ? 0 : 1);
                        game.state = Game1.GameState.menu;
                        game.Menu.SwitchMenu(game.Menu.endgame);
                        end = true;
                    }
                    break;
            }

            if (end)
            {
                Reset();
            }
        }

        public static void Reset()
        {
            inMenu = false;
            for (int i = 0; i < endgame.teamGold.Length; i++ )
            {
                endgame.teamGold[i] = 0;
            }
            players.Clear();
            map.Reset();
            HUD.Map.initialized = false;
            itemManager.Reset();
            countdown = 3;
            Client.Send("reset!");
        }

        public static void Disconnect()
        {
            players.Clear();
            Client.Close();
            if (game.Menu.serverSetup.Server != null)
            {
                game.Menu.serverSetup.Server.Close();
            }
        }

        public static bool InitNetwork(string ip)
        {
            try
            {
                Client.Connect(ip);
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return false;
            }
            return true;
        }

        public static void UpdateNetwork(float deltaTime)
        {
            timer++;
            if (players.ContainsKey(gameId))
            {
                
                Client.Send("p" + "\n" + gameId.ToString() + "\n" + ((int)players[gameId].Position.X).ToString()
                + "\n" + ((int)players[gameId].Position.Y).ToString() + "\n" + 
                (players[gameId].Angle).ToString());
                timer = 0; 
            }
        }

        public static void Update(float deltaTime)
        {
            if (countdown > -1)
            {
                countdown -= deltaTime / 1000;
            }
            else
            {
                countdown = -1;
            }

            UpdateNetwork(deltaTime);
            bulletManager.Update(map, deltaTime);
            itemManager.Update(deltaTime);
            trapManager.Update(deltaTime);

            lock (players)
            {
                foreach (var v in players)
                {
                    Player p = v.Value;
                    p.Update(deltaTime);
                    p.UpdateCollisions(map.GetTilesNear((int)p.Position.X, (int)p.Position.Y));
                }
            }
            // camera
            UpdateCamera();

            if (inMenu)
            {
                menuManager.Update();
            }

            CheckEndgame();
        }

        public static void UpdateCamera()
        {
            cam.pos = players[gameId].LightPosition;
        }

        public static void DrawGroundTiles(SpriteBatch sb)
        {
            map.DrawGround(sb);
        }

        public static void DrawWallTiles(SpriteBatch sb)
        {
            map.DrawWalls(sb);
        }

        public static void DrawSceneBehindPlayer(SpriteBatch sb)
        {
            //DrawWallTiles(sb);
            itemManager.DrawBackdrop(sb);
            itemManager.Draw(sb);
            trapManager.Draw(sb);
        }

        public static void DrawSceneInFrontOfPlayer(SpriteBatch sb)
        {
            //DrawWallTiles(sb);
            bulletManager.Draw(sb);
        }

        public static void DrawPlayers(SpriteBatch sb)
        {
            foreach (var p in players)
            {
                p.Value.Draw(sb);
            }
        }

    }
}
