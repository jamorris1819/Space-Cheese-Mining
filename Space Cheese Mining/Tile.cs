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

using Space_Cheese_Mining.GameScreens;

namespace Space_Cheese_Mining
{
    public class Tile
    {
        public List<Player> PlayersOnTile;
        public static Texture2D Background;
        public static Texture2D BackgroundHover;
        public static Texture2D CheeseTexture;
        public Vector2 Position;
        public Vector2 ArrayPosition;
        bool hovering = false;
        public bool containsCheese = false;
        Rectangle rect;
        Rectangle cheeseRect;

        public Tile(Vector2 pos)
        {
            PlayersOnTile = new List<Player>();
            Position = pos;
            rect = new Rectangle((int)Position.X, (int)Position.Y, Background.Width, Background.Height);
            cheeseRect = new Rectangle((int)Position.X + 8, (int)Position.Y + 8, Background.Width - 16, Background.Height - 16);
        }

        public void Draw(GameBase GameRef)
        {
            if(hovering)
                GameRef.spriteBatch.Draw(BackgroundHover, rect, Color.White);
            else
                GameRef.spriteBatch.Draw(Background, rect, Color.White);
            if (containsCheese)
                GameRef.spriteBatch.Draw(CheeseTexture, cheeseRect, Color.White);
        }

        public void Update(GameBase GameRef)
        {
            hovering = InputHandler.Mouse.Intersects(rect);
            if (hovering && InputHandler.LeftMousePressed() && GameRef.GameScreen.gameState == GameScreens.BoardGameState.WAITING_FOR_CHEESE && containsCheese == false)
                PlaceCheese(GameRef);
        }

        public bool PlaceCheese(GameBase GameRef)
        {
            if (ArrayPosition == Vector2.Zero || ArrayPosition == Vector2.One * 7f || ArrayPosition == new Vector2(0, 7) || ArrayPosition == new Vector2(7, 0) || containsCheese)
                return false;
            GameScreen.currentPlayer.cheesePlacesLeft -= 1;
            containsCheese = true;
            GameRef.GameScreen.NextPlayer();
            return true;
        }
    }
}
