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
    public class EndScreen : BaseGameState
    {
        public Player Winner;
        Label title;
        Label endText;
        Label endText2;
        Label winnerText;
        LinkLabel playAgain;
        LinkLabel exitToMenu;
        Label credits;

        public EndScreen(Game game, GameStateManager manager)
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

            endText = new Label();
            endText.Text = "Congratulations to";
            endText.SpriteFont = mainFont;
            endText.Position = new Vector2((GameRef.ScreenRectangle.Width - mainFont.MeasureString(endText.Text).X) / 2, 250);
            ControlManager.Add(endText);

            winnerText = new Label();
            winnerText.Text = Winner.Name;
            winnerText.SpriteFont = mainFont;
            winnerText.Position = new Vector2((GameRef.ScreenRectangle.Width - mainFont.MeasureString(winnerText.Text).X) / 2, 325);
            ControlManager.Add(winnerText);

            endText2 = new Label();
            endText2.Text = "for winning the game!";
            endText2.SpriteFont = mainFont;
            endText2.Position = new Vector2((GameRef.ScreenRectangle.Width - mainFont.MeasureString(endText2.Text).X) / 2, 400);
            ControlManager.Add(endText2);

            playAgain = new LinkLabel();
            playAgain.Text = "Play again";
            playAgain.Position = new Vector2(150, 550);
            playAgain.Selected += playAgain_Selected;
            ControlManager.Add(playAgain);

            exitToMenu = new LinkLabel();
            exitToMenu.Text = "Exit Game";
            exitToMenu.Position = new Vector2(600, 550);
            exitToMenu.Selected += exitToMenu_Selected;
            ControlManager.Add(exitToMenu);

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

        void playAgain_Selected(object sender, EventArgs e)
        {
            GameRef.GameScreen.testingMode = true;
            GameRef.GameScreen = new GameScreen(GameRef, StateManager);         // This resets everything we know about the Game screen. We don't reset the player screen
            StateManager.ChangeState(GameRef.PlayerSelectScreen);               // because we're going there and will keep the entered names
        }

        void exitToMenu_Selected(object sender, EventArgs e)
        {
            GameRef.GameScreen = new GameScreen(GameRef, StateManager);                 // reset it all by re-initialising
            GameRef.PlayerSelectScreen = new PlayerSelectScreen(GameRef, StateManager);    
            StateManager.ChangeState(GameRef.MenuScreen);              
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
