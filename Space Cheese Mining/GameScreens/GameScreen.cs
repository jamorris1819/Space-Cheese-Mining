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
using System.Runtime.InteropServices;



namespace Space_Cheese_Mining.GameScreens
{
    public enum BoardGameState { WAITING_FOR_CHEESIEST, WAITING_FOR_INPUT, WAITING_FOR_ROLL, WAITING_FOR_CHEESE, WAITING_FOR_MOVE, WAITING_FOR_GRAPPLE_CHOICE }
    public class GameScreen : BaseGameState
    {
        #region Board and Game management
        const int BOARD_WIDTH = 8;
        const int BOARD_HEIGHT = 8;
        public static List<Player> playerData;
        public static Player currentPlayer;
        public int HEIGHT_OFFSET = 16;
        public static Tile[,] board;
        public BoardGameState gameState;
        Die die;
        int lastRolled;
        List<Player> playersOnSameTile = new List<Player>();                                                    // We want to see if we are sharing a space with another player, we store the variable here.
        public bool testingMode = false;

        #endregion
        #region Game Panel Buttons
        Label mainOut; // Our output line

        LinkPictureBox player1iconChoose;
        LinkPictureBox player2iconChoose;
        LinkPictureBox player3iconChoose;
        LinkPictureBox player4iconChoose;

        LinkPictureBox upButton;
        LinkPictureBox downButton;
        LinkPictureBox leftButton;
        LinkPictureBox rightButton;
        #endregion
        #region Testing region
        Random random;
        [DllImport("kernel32")]
        static extern bool AllocConsole();
        #endregion
        #region Constructor
        public GameScreen(Game game, GameStateManager manager)
            : base(game, manager)
        {
            board = new Tile[BOARD_WIDTH, BOARD_HEIGHT];
            random = new Random();
        }
        #endregion
        #region Initialize
        public override void Initialize()
        {
            base.Initialize();
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                for (int y = 0; y < BOARD_HEIGHT; y++)
                {
                    Tile tileToCreate = new Tile(pos:new Vector2(228 + x * (Tile.Background.Width + 8), HEIGHT_OFFSET + y * (Tile.Background.Height + 8)));
                    tileToCreate.ArrayPosition = new Vector2(x, y);
                    board[x, y] = tileToCreate;
                }
            }
            gameState = BoardGameState.WAITING_FOR_CHEESIEST;
        }
        #endregion
        #region Movement button events
        void upButton_Selected(object sender, EventArgs e)
        {
            TakeMove(direction:new Vector2(0, -1));
        }
        void downButton_Selected(object sender, EventArgs e)
        {
            TakeMove(direction:new Vector2(0, 1));
        }
        void leftButton_Selected(object sender, EventArgs e)
        {
            TakeMove(direction:new Vector2(-1, 0));
        }
        void rightButton_Selected(object sender, EventArgs e)
        {
            TakeMove(direction:new Vector2(1, 0));
        }
        #endregion
        #region Player select events
        void player1iconChoose_Selected(object sender, EventArgs e)
        {
            PlayerButtonPressed(index: 0);
        }

        void player2iconChoose_Selected(object sender, EventArgs e)
        {
            PlayerButtonPressed(index: 1);
        }

        void player3iconChoose_Selected(object sender, EventArgs e)
        {
            PlayerButtonPressed(index: 2);
        }

        void player4iconChoose_Selected(object sender, EventArgs e)
        {
            PlayerButtonPressed(index: 3);
        }
        #endregion
        #region Load
        protected override void LoadContent()
        {
            base.LoadContent();
            #region GUI
            #region Die
            die = new Die(pos: new Vector2(0.5f * (1024 - 68), 656));
            die.textureNames = new string[6] { "die1", "die2", "die3", "die4", "die5", "die6" };
            die.LoadContent(GameRef: GameRef);
            #endregion
            #region Tile
            Tile.Background = GameRef.Content.Load<Texture2D>("tile");
            Tile.BackgroundHover = GameRef.Content.Load<Texture2D>("tileHover");
            Tile.CheeseTexture = GameRef.Content.Load<Texture2D>("cheese");
            #endregion
            #region Player select icons
            Texture2D p1Icon = GameRef.Content.Load<Texture2D>("player1");
            Texture2D p2Icon = GameRef.Content.Load<Texture2D>("player2");
            Texture2D p3Icon = GameRef.Content.Load<Texture2D>("player3");
            Texture2D p4Icon = GameRef.Content.Load<Texture2D>("player4");
            player1iconChoose = new LinkPictureBox(image: p1Icon, destination: new Rectangle(170, 660, p1Icon.Width, p1Icon.Height));
            player1iconChoose.Selected += player1iconChoose_Selected;
            ControlManager.Add(player1iconChoose);

            player2iconChoose = new LinkPictureBox(image: p2Icon, destination: new Rectangle(370, 660, p2Icon.Width, p2Icon.Height));
            player2iconChoose.Selected += player2iconChoose_Selected;
            ControlManager.Add(player2iconChoose);

            if (playerData.Count >= 3)
            {
                player3iconChoose = new LinkPictureBox(image: p3Icon, destination: new Rectangle(570, 660, p3Icon.Width, p3Icon.Height));
                player3iconChoose.Selected += player3iconChoose_Selected;
                ControlManager.Add(player3iconChoose);
            }
            if (playerData.Count == 4)
            {
                player4iconChoose = new LinkPictureBox(image: p4Icon, destination: new Rectangle(770, 660, p4Icon.Width, p4Icon.Height));
                player4iconChoose.Selected += player4iconChoose_Selected;
                ControlManager.Add(player4iconChoose);
            }
            // maybe if I weren't such a barbarian, I would make these Link Picture Boxes align in the middle.
            #endregion
            #region Output line
            mainOut = new Label();
            mainOut.Text = "";
            mainOut.SpriteFont = smallFont;
            mainOut.Position = new Vector2((GameRef.ScreenRectangle.Width - smallFont.MeasureString(mainOut.Text).X) / 2, 600);
            ControlManager.Add(mainOut);
            #endregion
            #region Movement buttons
            upButton = new LinkPictureBox(image: GameRef.Content.Load<Texture2D>("Buttons/up"), destination: new Rectangle(280, 650, 80, 80));
            upButton.Selected += upButton_Selected;
            ControlManager.Add(upButton);
            downButton = new LinkPictureBox(image: GameRef.Content.Load<Texture2D>("Buttons/down"), destination: new Rectangle(380, 650, 80, 80));
            downButton.Selected += downButton_Selected;
            ControlManager.Add(downButton);
            leftButton = new LinkPictureBox(image: GameRef.Content.Load<Texture2D>("Buttons/left"), destination: new Rectangle(1024 - 380 - 80, 650, 80, 80));
            leftButton.Selected += leftButton_Selected;
            ControlManager.Add(leftButton);
            rightButton = new LinkPictureBox(image: GameRef.Content.Load<Texture2D>("Buttons/right"), destination: new Rectangle(1024 - 280 - 80, 650, 80, 80));
            rightButton.Selected += rightButton_Selected;
            ControlManager.Add(rightButton);
            #endregion
            #endregion
            #region Player
            foreach (Player player in playerData)
                player.LoadContent(GameRef: GameRef);
            #endregion
        }
        #endregion
        #region Draw
        public override void Draw(GameTime gameTime)
        {
            GameRef.spriteBatch.Begin();
            base.Draw(gameTime:gameTime);
            ControlManager.Draw(spriteBatch:GameRef.spriteBatch);                   // Draw all the controls (labels, buttons, etc)
            foreach (Tile tile in board)
                tile.Draw(GameRef:GameRef);
            foreach (Player player in playerData)
                player.Draw(GameRef:GameRef, gameTime:gameTime);
            die.Draw(GameRef:GameRef);
            // Draw scoreboard
            GameRef.spriteBatch.DrawString(smallFont, "Scoreboard", new Vector2(25, 20), Color.White);
            for (int i = 0; i < playerData.Count; i++)
            {
                GameRef.spriteBatch.Draw(playerData[i].texture, new Rectangle(20, 70 + (70 * i), 40, 40), playerData[i] == currentPlayer ? Color.White : Color.Black);
                GameRef.spriteBatch.DrawString(smallFont, playerData[i].CheeseCount.ToString(), new Vector2(70, 70 + (70 * i)), Color.White);
            }
            GameRef.spriteBatch.End();
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            player1iconChoose.Visible = false;                                  // We must display the individual controls for each state
            player2iconChoose.Visible = false;                                  // So we set everything to invisible, so we can reset the desired controls to be visible
            if(playerData.Count >= 3)                                           // If there's 3 or more players
                player3iconChoose.Visible = false;                              // Then show the third player icon
            if (playerData.Count == 4)                                          // If there are 4 players
                player4iconChoose.Visible = false;                              // Show the fourth player icon
            die.Visible = false;
            upButton.Visible = false;
            downButton.Visible = false;
            leftButton.Visible = false;
            rightButton.Visible = false;
            switch (gameState)                                                  // Manage the game by splitting it down into states
            {   
                case BoardGameState.WAITING_FOR_CHEESIEST:                      // This state manages the choosing of which player goes first, or is most cheesiest
                    mainOut.Text = "Choose the cheesiest looking player below to go first!";
                    mainOut.Position = new Vector2((GameRef.ScreenRectangle.Width - smallFont.MeasureString(mainOut.Text).X) / 2, 600);
                    player1iconChoose.Visible = true;
                    player2iconChoose.Visible = true;
                    if (playerData.Count >= 3)
                        player3iconChoose.Visible = true;
                    if (playerData.Count == 4)
                        player4iconChoose.Visible = true;
                    break;
                case BoardGameState.WAITING_FOR_CHEESE:
                    mainOut.Text = "Place some cheese, " + currentPlayer.Name + ". " + currentPlayer.cheesePlacesLeft + " pieces remain.";
                    mainOut.Position = new Vector2((GameRef.ScreenRectangle.Width - smallFont.MeasureString(mainOut.Text).X) / 2, 600);
                    int totalCheeseRemaining = 0;                               // This is how many pieces of cheese are still to be placed by all players
                    foreach (Player player in playerData)
                        totalCheeseRemaining += player.cheesePlacesLeft;        // We add it all up
                    if (totalCheeseRemaining == 0)                              // If there's none left to place, let's forward the state
                        gameState = BoardGameState.WAITING_FOR_ROLL;
                    break;
                case BoardGameState.WAITING_FOR_ROLL:                           // Player is waiting to roll die
                    mainOut.Text = "Click the die to roll, " + currentPlayer.Name + ".";
                    mainOut.Position = new Vector2((GameRef.ScreenRectangle.Width - smallFont.MeasureString(mainOut.Text).X) / 2, 600);
                    die.Visible = true;
                    
                    if (testingMode)                                            // If we're testing it, we want to run things differently
                    {   
                        Console.WriteLine("What number would you like rolled for " + currentPlayer.Name + "?");
                        int rolledNumber = int.Parse(s:Console.ReadLine());
                        lastRolled = rolledNumber;
                        gameState = BoardGameState.WAITING_FOR_MOVE;
                    }
                    else
                    {
                        int rolledNumber = 0;
                        if (die.DrawRectangle.Intersects(InputHandler.Mouse) && InputHandler.LeftMousePressed())    // If the user has clicked on the die
                        {
                            rolledNumber = die.Roll() + 1;                                                          // This is a number starting from 0, so offset
                            lastRolled = rolledNumber;                                                              // rolledNumber will reset to 0 each game loop, so we store it in our special variable lastRolled
                            Console.WriteLine(currentPlayer.Name + " rolls a " + lastRolled);
                            gameState = BoardGameState.WAITING_FOR_MOVE;
                        }
                    }
                    break;
                case BoardGameState.WAITING_FOR_MOVE:                           // We're waiting for our player to take their go
                    mainOut.Text = "You rolled " + lastRolled + "! Make your move.";
                    mainOut.Position = new Vector2((GameRef.ScreenRectangle.Width - smallFont.MeasureString(mainOut.Text).X) / 2, 600);
                    die.Visible = true;
                    upButton.Visible = true;
                    downButton.Visible = true;
                    leftButton.Visible = true;
                    rightButton.Visible = true;
                    break;
                case BoardGameState.WAITING_FOR_GRAPPLE_CHOICE:                 // Who do we grapple from?
                    mainOut.Text = "Choose whom to grapple cheese from.";
                    mainOut.Position = new Vector2((GameRef.ScreenRectangle.Width - smallFont.MeasureString(mainOut.Text).X) / 2, 600);
                    // Darken all the players who aren't on the same tile as the player
                    player1iconChoose.Visible = true;
                    player1iconChoose.Color = (playersOnSameTile.Contains(playerData[0])) ? Color.White : new Color(30, 30, 30);
                    player2iconChoose.Visible = true;
                    player2iconChoose.Color = (playersOnSameTile.Contains(playerData[1])) ? Color.White : new Color(30, 30, 30);
                    if (playerData.Count >= 3)
                    {
                        player3iconChoose.Visible = true;
                        player3iconChoose.Color = (playersOnSameTile.Contains(playerData[2])) ? Color.White : new Color(30, 30, 30);
                    }
                    if(playerData.Count == 4)
                    {
                        player4iconChoose.Visible = true;
                        player4iconChoose.Color = (playersOnSameTile.Contains(playerData[3])) ? Color.White : new Color(30, 30, 30);
                    }
                    break;
                default:
                    mainOut.Text = "";
                    die.Visible = false;
                    break;
            }
            ControlManager.Update(gameTime:gameTime);   // Here we update all our control buttons, images, etc
            foreach (Tile tile in board)
                tile.Update(GameRef:GameRef);           
            base.Update(gameTime:gameTime);
            CheckIfWon();                               // Check if anyone run before we repeat the game loop
        }
        #endregion
        #region Player functions
        /// <summary>
        /// Take a move for the current player
        /// </summary>
        /// <param name="direction">A normalised direction vector</param>
        void TakeMove(Vector2 direction)
        {
            Vector2 newPosition = currentPlayer.Position + direction * lastRolled;                                  // Calculate where we're going
            Console.Write(currentPlayer.Name + " moves from (" + (currentPlayer.Position.X + 1) + ", " + (currentPlayer.Position.Y + 1) + ")");
            currentPlayer.Position = GetTile(newPosition.X, newPosition.Y).ArrayPosition;                           // The position of the tile isn't the same as it's array position.
            Console.WriteLine(" to (" + (currentPlayer.Position.X + 1) + ", " + (currentPlayer.Position.Y + 1) + ")");
            Tile landedOn = GetTile(xx: currentPlayer.Position.X, yy: currentPlayer.Position.Y);
            if (landedOn.containsCheese)
            {
                landedOn.containsCheese = false;
                currentPlayer.CheeseCount++;
                CheckIfWon();
            }
            foreach (Player player in playerData)
                if (player.Position == currentPlayer.Position && player != currentPlayer && player.CheeseCount >= 1)                           // If we share a position with a player, and it's not with ourself and they have cheese
                    playersOnSameTile.Add(player);
            // Then stick them in the designated list
            if (playersOnSameTile.Count == 1)
            {
                Console.WriteLine(currentPlayer.Name + " landed on " + playersOnSameTile[0].Name + "!");
                GrappleCheese(playersOnSameTile[0].index);
            }
            else if (playersOnSameTile.Count > 1)
            {
                foreach (Player p in playersOnSameTile)
                    Console.WriteLine(currentPlayer.Name + " landed on " + p.Name + "!");
                gameState = BoardGameState.WAITING_FOR_GRAPPLE_CHOICE;
            }
            else
            {
                NextPlayer();
                gameState = BoardGameState.WAITING_FOR_ROLL;
            }
        }

        /// <summary>
        /// Grapple some of that cheese
        /// </summary>
        /// <param name="index">The player index of the player who is losing cheese</param>
        void GrappleCheese(int index)
        {
            // I don't bother to check anything here, because it is checked before this method is called
            currentPlayer.CheeseCount += 1;
            playerData[index].CheeseCount -= 1;
            Console.WriteLine("Cheese has been stolen from " + playerData[index].Name + " by " + currentPlayer.Name);
            NextPlayer();
            CheckIfWon();
            gameState = BoardGameState.WAITING_FOR_ROLL;
            playersOnSameTile = new List<Player>();
        }
        #endregion
        #region Game functions
        void PlayerButtonPressed(int index)                                         // Triggered when one of the 4 player menu buttons is pressed
        {                                                                           // To save on code we recycle the button
            if (gameState == BoardGameState.WAITING_FOR_CHEESIEST)                  // This is the process for choosing the cheesiest player
            {
                currentPlayer = playerData[index];                                  // Update the currentPlayer variable
                gameState = BoardGameState.WAITING_FOR_CHEESE;                      // Alert the game that we're now in cheese placing mode
                CheckIfExtraCheeseNeeded(index: index);                             // This assigns the correct number of cheese for each player
                CheckIfAutoCheese();                                                // Are we automatically placing the cheese?
            }
            else if (gameState == BoardGameState.WAITING_FOR_GRAPPLE_CHOICE)        // In this case the player is choosing who to steal cheese from
                GrappleCheese(index: index);                                        // Grapple the player selected
        }

        void CheckIfExtraCheeseNeeded(int index) // If there are 16 pieces of cheese to be placed and 3 to place them, they each get an irrational amount - that's not right!
        {
            if (playerData.Count == 3)
            {
                foreach (Player player in playerData)
                {
                    player.cheesePlacesLeft = (int)Math.Floor(16f / playerData.Count);      // We divide 16f as opposed to 16 - this stops the ambiguity between doubles and decimals. We floor it because it's irrational
                }
                playerData[0].cheesePlacesLeft++; // To compensate for each player only having 5 pieces (5 * 3 != 16), we assign player 1 (or 0) an extra piece
            }
            else
            {
                foreach (Player player in playerData)
                {
                    player.cheesePlacesLeft = 16 / playerData.Count;
                }
            }
        }

        void CheckIfAutoCheese()
        {
            if (testingMode == false)
                return;
            AllocConsole();
            Console.WriteLine("Would you like to auto place cheese? (Y/N)");
            string input = Console.ReadKey().KeyChar.ToString().ToLower();          // Why make the user press enter?
            if (input == "y")
            {
                AllocConsole();
                Console.WriteLine("Enter a seed for this generation (or type \"DEMO\"): ");
                string seed = Console.ReadLine();                       // We want a seed for this string
                if (seed.ToLower() == "demo")
                {
                    foreach (Player player in playerData)
                        player.cheesePlacesLeft = 0;                        // If we're placing their cheese for them, they don't need this (it counts how many cheese pieces they can place)
                    GetTile(1, 0).containsCheese = true;
                    GetTile(3, 6).containsCheese = true;
                    GetTile(5, 3).containsCheese = true;
                    GetTile(2, 7).containsCheese = true;
                    GetTile(0, 5).containsCheese = true;
                    GetTile(0, 1).containsCheese = true;
                    GetTile(5, 5).containsCheese = true;
                    GetTile(2, 4).containsCheese = true;
                    GetTile(1, 3).containsCheese = true;
                    GetTile(6, 1).containsCheese = true;
                    GetTile(6, 2).containsCheese = true;
                    GetTile(6, 7).containsCheese = true;
                    GetTile(4, 5).containsCheese = true;
                    GetTile(5, 0).containsCheese = true;
                    GetTile(7, 6).containsCheese = true;
                    GetTile(6, 0).containsCheese = true;
                    gameState = BoardGameState.WAITING_FOR_ROLL;            // All cheese is placed now, so we can skip the cheese placing stage!
                    if (playerData.Count == 4)
                        playerData[3].CheeseCount = 3;
                    return;
                }
                int seedInteger = 0;                                    // But it only takes an integer
                foreach (char character in seed.ToCharArray())          // So we'll find the character value of each letter
                    seedInteger += (int)character;                      // And append it to the seed integer
                Random cheesePlacementRandom = new Random(seedInteger); // because then I don't need to check if their input is an integer!
                int cheeseToPlace = 16;
                while (cheeseToPlace > 0)                               // We're not stopping until all the cheese is placed
                {
                    int x = cheesePlacementRandom.Next(0, 7 + 1);
                    int y = cheesePlacementRandom.Next(0, 7 + 1);
                    if (!((x == 0 && y == 0) || (x == 7 && y == 0) || (x == 7 && y == 7) || (x == 0 && y == 7)) && !GetTile(x, y).containsCheese)
                    {
                        // Awesome, we're not on a starting square and there's no cheese on this square either
                        GetTile(x, y).containsCheese = true;
                        cheeseToPlace -= 1;
                    }
                    // If a piece of cheese hasn't been placed by now... it doesn't matter. This will keep looping until all pieces are placed.
                }
                gameState = BoardGameState.WAITING_FOR_ROLL; // All cheese is placed now, so we can skip the player cheese placing stage!
            }
            // For testing purposes I won't do extensive input validation
            // If they don't want to auto place the cheese, then this method can terminate here.
        }

        void CheckIfWon()
        {
            foreach (Player player in playerData)
            {
                if (player.CheeseCount == 6)
                {
                    //END GAME
                    Console.WriteLine("Game has been won by " + player.Name + "!");
                    GameRef.EndScreen.Winner = player;
                    StateManager.ChangeState(GameRef.EndScreen);
                }
            }
        }

        public void NextPlayer()
        {
            int nextIndex = currentPlayer.index + 1;
            if (nextIndex == playerData.Count)
                nextIndex = 0;
            currentPlayer = playerData[nextIndex];
        }

        Tile GetTile(float xx, float yy)                            // This function allows me to input 'invalid' data and correct it
        {                                                           // IE to make the map loop. Entering (1, 0) is valid, whilst (9, 0) is invalid, which is rectified.
            int x = (int)xx;
            int y = (int)yy;
            if (x >= 0 && x < 8 && y >= 0 && y < 8)
                return board[x, y];
            while (x < 0)
                x += 8;
            while (x > 7)
                x -= 8;
            while (y < 7)
                y += 8;
            while (y > 7)
                y -= 8;
            return board[x, y];
        }
        #endregion
    }
}
