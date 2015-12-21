using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Screen_Management_Library;

namespace Space_Cheese_Mining
{
    public class Die
    {
        public string[] textureNames;
        public bool Visible;
        public Rectangle DrawRectangle;
        Texture2D[] sides;
        int currentSide;
        Vector2 position;

        public Die(Vector2 pos)
        {
            position = pos;
            currentSide = 0;
            Visible = false;
        }

        public void LoadContent(GameBase GameRef)
        {
            sides = new Texture2D[6];
            for(int i = 0; i < textureNames.Length; i++)                                // Load all texture files in
            {
                sides[i] = GameRef.Content.Load<Texture2D>("Die/" + textureNames[i]);
            }
            DrawRectangle = new Rectangle((int)position.X, (int)position.Y, sides[0].Width, sides[0].Height);
        }

        public int Roll()
        {
            Random random = new Random();
            currentSide = random.Next(0, 6);
            return currentSide;
        }

        public void Draw(GameBase GameRef)
        {
            if(Visible)
                GameRef.spriteBatch.Draw(sides[currentSide], new Rectangle((int)position.X, (int)position.Y, sides[0].Width, sides[0].Height), Color.White);
        }
    }
}
