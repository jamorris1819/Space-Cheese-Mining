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
    public class PlayerSelectScreen : BaseGameState
    {
        Label title;
        Label instructions;
        PictureBox player1;
        Textbox player1textbox;
        PictureBox player2;
        Textbox player2textbox;
        PictureBox player3;
        Textbox player3textbox;
        PictureBox player4;
        Textbox player4textbox;
        LinkLabel startGame;

        public PlayerSelectScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {

        }

        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            ContentManager Content = Game.Content;
            base.LoadContent();
            title = new Label();
            title.Text = "Space Cheese Mining";
            title.SpriteFont = titleFont;
            title.Position = new Vector2((GameRef.ScreenRectangle.Width - titleFont.MeasureString("Space Cheese Mining").X) / 2, 100);
            ControlManager.Add(title);

            instructions = new Label();
            instructions.Text = "Enter player names next to desired piece. Minimum 2.";
            instructions.SpriteFont = smallFont;
            instructions.Position = new Vector2(100, 215);
            ControlManager.Add(instructions);

            Texture2D t = Content.Load<Texture2D>("player1");
            player1 = new PictureBox(t, new Rectangle(30, 280, t.Width, t.Height));
            ControlManager.Add(player1);

            player1textbox = new Textbox();
            player1textbox.Position = new Vector2(150, 280);
            player1textbox.Offset = new Vector2(8, 4);
            player1textbox.Text = "";
            player1textbox.MaxCharacters = 28;
            player1textbox.BorderSize = 2;
            player1textbox.Pixel = Content.Load<Texture2D>("pixel");
            player1textbox.Font = mainFont;
            player1textbox.SetHeight();
            ControlManager.Add(player1textbox);

            t = Content.Load<Texture2D>("player2");
            player2 = new PictureBox(t, new Rectangle(35, 380, t.Width, t.Height));
            ControlManager.Add(player2);

            player2textbox = new Textbox();
            player2textbox.Position = new Vector2(150, 380);
            player2textbox.Offset = new Vector2(8, 4);
            player2textbox.Text = "";
            player2textbox.MaxCharacters = 28;
            player2textbox.BorderSize = 2;
            player2textbox.Pixel = Content.Load<Texture2D>("pixel");
            player2textbox.Font = mainFont;
            player2textbox.SetHeight();
            ControlManager.Add(player2textbox);

            t = Content.Load<Texture2D>("player3");
            player3 = new PictureBox(t, new Rectangle(24, 480, t.Width, t.Height));
            ControlManager.Add(player3);

            player3textbox = new Textbox();
            player3textbox.Position = new Vector2(150, 480);
            player3textbox.Offset = new Vector2(8, 4);
            player3textbox.Text = "";
            player3textbox.MaxCharacters = 28;
            player3textbox.BorderSize = 2;
            player3textbox.Pixel = Content.Load<Texture2D>("pixel");
            player3textbox.Font = mainFont;
            player3textbox.SetHeight();
            ControlManager.Add(player3textbox);

            t = Content.Load<Texture2D>("player4");
            player4 = new PictureBox(t, new Rectangle(30, 570, t.Width, t.Height));
            ControlManager.Add(player4);

            player4textbox = new Textbox();
            player4textbox.Position = new Vector2(150, 580);
            player4textbox.Offset = new Vector2(8, 4);
            player4textbox.Text = "";
            player4textbox.MaxCharacters = 28;
            player4textbox.BorderSize = 2;
            player4textbox.Pixel = Content.Load<Texture2D>("pixel");
            player4textbox.Font = mainFont;
            player4textbox.SetHeight();
            ControlManager.Add(player4textbox);

            startGame = new LinkLabel();
            startGame.Text = "Start Game";
            startGame.Position = new Vector2(720, 675);
            startGame.Selected += startGame_Selected;
            ControlManager.Add(startGame);
        }

        void startGame_Selected(object sender, EventArgs e)
        {
            Vector2[] startingLocations = new Vector2[] { new Vector2(0, 0), new Vector2(7, 0), new Vector2(7, 7), new Vector2(0, 7) };
            Textbox[] textboxes = new Textbox[4];
            textboxes[0] = player1textbox;
            textboxes[1] = player2textbox;
            textboxes[2] = player3textbox;
            textboxes[3] = player4textbox;

            GameScreen.playerData = new List<Player>();

            for (int i = 0; i < 4; i++)
            {
                if (textboxes[i].Text != "")
                    GameScreen.playerData.Add(new Player(textboxes[i].Text, startingLocations[i], "player" + (i + 1), i));
            }
            if(textboxes[0].Text != "" && textboxes[1].Text != "")                  // at least 2 players
                StateManager.ChangeState(GameRef.GameScreen);
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
            player2textbox.Enabled = (player1textbox.Text != "");
            player2textbox.Text = (player1textbox.Text != "") ? player2textbox.Text : "";

            player3textbox.Enabled = (player2textbox.Text != "");
            player3textbox.Text = (player2textbox.Text != "") ? player3textbox.Text : "";

            player4textbox.Enabled = (player3textbox.Text != "");
            player4textbox.Text = (player3textbox.Text != "") ? player4textbox.Text : "";
            base.Update(gameTime);
        }
    }
}
