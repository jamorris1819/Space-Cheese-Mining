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

using Screen_Management_Library;
using Screen_Management_Library.Controls;

using Space_Cheese_Mining.GameScreens;

namespace Space_Cheese_Mining
{

    public class GameBase : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        const int screenWidth = 1024;
        const int screenHeight = 768;
        public readonly Rectangle ScreenRectangle;

        GameStateManager stateManager;
        public MenuScreen MenuScreen;
        public GameScreen GameScreen;
        public PlayerSelectScreen PlayerSelectScreen;
        public EndScreen EndScreen;

        Texture2D background;

        public GameBase()
        {
            this.Window.Title = "Space Cheese Mining";
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            this.IsMouseVisible = true;

            Content.RootDirectory = "Content";
            ScreenRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            Components.Add(new InputHandler(this));

            stateManager = new GameStateManager(this);
            Components.Add(stateManager);

            MenuScreen = new MenuScreen(this, stateManager);
            GameScreen = new GameScreen(this, stateManager);
            PlayerSelectScreen = new PlayerSelectScreen(this, stateManager);
            EndScreen = new GameScreens.EndScreen(this, stateManager);

            stateManager.ChangeState(MenuScreen);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("blue");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            double wide = Math.Ceiling((double)(screenWidth / background.Width));
            double heigh = Math.Ceiling((double)(screenHeight / background.Height));
            for (int i = 0; i < wide; i++)
                for (int j = 0; j < heigh; j++)
                    spriteBatch.Draw(background, new Rectangle(i * background.Width, j * background.Height, background.Width, background.Height), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
