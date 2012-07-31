using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MultiDungeon.Graphics;
using MultiDungeon.HUD;
using MultiDungeon.Menus;

namespace MultiDungeon
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public MenuManager menu;
        public Color shadowColor = Color.Black;

        public enum GameState
        {
            menu,
            game
        }

        public GameState state = GameState.menu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        public MenuManager Menu
        {
            get { return menu; }
            set { menu = value; }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Window.AllowUserResizing = false;
            graphics.PreferredBackBufferWidth = GameConst.SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = GameConst.SCREEN_HEIGHT;
            graphics.ApplyChanges();
            if (Console.Enabled)
            { Console.Init(); }
            TextureManager.Initialize(Content);
            World.Init(graphics, Content, this);
            Shadowmap.Init(this, graphics.GraphicsDevice, Content);
            Hud.Init();
            menu = new MenuManager(this);

            base.Initialize();
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            base.OnExiting(sender, args);
            if (Console.sw != null)
            {
                Console.sw.Flush();
                Console.sw.Close();
            }
            Stats.PrintStats();
            World.Disconnect();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Client.Close();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (state)
            {
                case GameState.menu:
                    menu.Update();
                    break;
                case GameState.game:
                    Shadowmap.Update(World.Player.LightPosition);
                    Hud.Update(gameTime.ElapsedGameTime.Milliseconds);
                    World.Update(gameTime.ElapsedGameTime.Milliseconds);
                    break;
            }

            Console.Update(gameTime.ElapsedGameTime.Milliseconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            switch (state)
            {
                case GameState.menu: 
                    spriteBatch.Begin();
                    menu.Draw(spriteBatch);  
                    spriteBatch.End();
                    break;
                case GameState.game:
                    Shadowmap.Draw(spriteBatch, World.Camera, shadowColor);
                    spriteBatch.Begin();
                    Hud.Draw(spriteBatch);
                    spriteBatch.End();
                    break;
            }

            spriteBatch.Begin();
            Console.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
