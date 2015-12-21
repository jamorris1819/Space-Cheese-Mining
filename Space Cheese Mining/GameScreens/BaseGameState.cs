using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

using Screen_Management_Library;
using Screen_Management_Library.Controls;

namespace Space_Cheese_Mining.GameScreens
{
    public abstract partial class BaseGameState : GameState
    {
        protected GameBase GameRef;
        protected ControlManager ControlManager;
        protected SoundEffect controlChange;
        protected SoundEffect controlSelect;
        protected SoundEffect click;
        protected SpriteFont mainFont;
        protected SpriteFont titleFont;
        protected SpriteFont smallFont;

        public BaseGameState(Game game, GameStateManager manager)
            : base(game, manager)
        {
            GameRef = (GameBase)game;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ContentManager Content = Game.Content;
            // Load in all data, ie sounds, graphics, etc
            mainFont = Content.Load<SpriteFont>("Fonts/mainFont");
            titleFont = Content.Load<SpriteFont>("Fonts/titleFont");
            smallFont = Content.Load<SpriteFont>("Fonts/smallFont");
            ControlManager = new ControlManager(mainFont);
            base.LoadContent();
        }

        protected override void StateChange(object sender, EventArgs e)
        {
            base.StateChange(sender, e);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
