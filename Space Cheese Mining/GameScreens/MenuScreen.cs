using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

using Screen_Management_Library;
using Screen_Management_Library.Controls;

namespace Space_Cheese_Mining.GameScreens
{
    public class MenuScreen : BaseGameState
    {
        Label title;
        LinkLabel classicGame;
        LinkLabel testingGame;
        LinkLabel exitGame;
        Label credits;
        Textbox textbox;

        public MenuScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            ContentManager Content = Game.Content;

            title = new Label();
            title.Text = "Space Cheese Mining";
            title.SpriteFont = titleFont;
            title.Position = new Vector2((GameRef.ScreenRectangle.Width - titleFont.MeasureString("Space Cheese Mining").X) / 2, 100);
            ControlManager.Add(title);

            classicGame = new LinkLabel();
            classicGame.Text = "Classic Mode";
            classicGame.Position = new Vector2((GameRef.ScreenRectangle.Width - mainFont.MeasureString("Classic Mode").X) / 2, 250);
            classicGame.Selected += classicGame_Selected;
            ControlManager.Add(classicGame);

            testingGame = new LinkLabel();
            testingGame.Text = "Testing Mode";
            testingGame.Position = new Vector2((GameRef.ScreenRectangle.Width - mainFont.MeasureString("Testing Mode").X) / 2, 350);
            testingGame.Selected += testingGame_Selected;
            ControlManager.Add(testingGame);

            exitGame = new LinkLabel();
            exitGame.Text = "Exit Game";
            exitGame.Position = new Vector2((GameRef.ScreenRectangle.Width - mainFont.MeasureString("Exit Game").X) / 2, 450);
            exitGame.Selected += exitGame_Selected;
            ControlManager.Add(exitGame);

            credits = new Label();
            credits.Text = "Programmed by Jacob Morris (jamorris.co.uk)";
            credits.SpriteFont = smallFont;
            credits.Position = new Vector2(10, GameRef.ScreenRectangle.Height - 50);
            ControlManager.Add(credits);
        }

        void classicGame_Selected(object sender, EventArgs e)
        {
            StateManager.ChangeState(GameRef.PlayerSelectScreen);
        }

        void testingGame_Selected(object sender, EventArgs e)
        {
            GameRef.GameScreen.testingMode = true;
            StateManager.ChangeState(GameRef.PlayerSelectScreen);
        }

        void exitGame_Selected(object sender, EventArgs e)
        {
            GameRef.Exit();
        }

        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();
            base.Draw(gameTime);
            ControlManager.Draw(GameRef.spriteBatch);
            GameRef.spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime);
            base.Update(gameTime);
        }
    }
}
