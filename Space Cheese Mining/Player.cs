using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Space_Cheese_Mining
{
    public class Player
    {
        public string Name;
        public Vector2 Position;
        public int CheeseCount;
        public Texture2D texture;
        string textureName;
        Vector2 offset;
        public int index;
        public int cheesePlacesLeft = 4;

        public Player(string name, Vector2 pos, string tex, int index)
        {
            Name = name;
            Position = pos;
            textureName = tex;
            this.index = index;
            CheeseCount = 0;
        }

        public void LoadContent(GameBase GameRef)
        {
            texture = GameRef.Content.Load<Texture2D>(textureName);
            offset.X = (64 - texture.Width / 2) / 2;
            offset.Y = (64 - texture.Height / 2) / 2;
        }

        public void Draw(GameBase GameRef, GameTime gameTime)
        {
            GameRef.spriteBatch.Draw(texture, new Rectangle(228 + (int)Position.X * 72 + (int)offset.X, GameRef.GameScreen.HEIGHT_OFFSET + (int)Position.Y * 72 + (int)offset.Y, texture.Width / 2, texture.Height / 2), Color.White);
        }
    }
}
