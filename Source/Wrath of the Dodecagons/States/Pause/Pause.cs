using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Wrath_of_the_Dodecagons.States.Pause
{
    /// <summary>
    /// Pause State, allowing the user to pause gameplay
    /// and either resume gameplay or quit to main menu
    /// </summary>
    class Pause : IState
    {
        #region Member Variables

        // Reference to main game class
        Game1 _game;
        // Drawing font
        SpriteFont _font;
        // Menu selection
        bool _resumeSelected = true;

        #endregion

        #region Constructor
        public Pause(Game1 a_game)
        {
            _game = a_game;

            // Load assets
            _font = _game.Content.Load<SpriteFont>("debug_font");
        }
        #endregion

        #region Main Loop
        public void Update()
        {
            // Check menu selection input
            if (_game.KeyboardState.IsKeyDown(Keys.Up) && _game.LastKeyboardState.IsKeyUp(Keys.Up) ||
                _game.KeyboardState.IsKeyDown(Keys.Down) && _game.LastKeyboardState.IsKeyUp(Keys.Down))
            {
                // Swap current menu selection
                _resumeSelected = !_resumeSelected;
            }
            // Check menu advancement input
            else if (_game.KeyboardState.IsKeyDown(Keys.Enter) && _game.LastKeyboardState.IsKeyUp(Keys.Enter))
            {
                // CONTINUE selected
                if (_resumeSelected)
                {
                    GameStateManager.PopState();
                    GameStateManager.PushState(_game.GameStates["GAME"]);
                }
                // BACK TO MENU selected
                else
                {
                    GameStateManager.PopState();
                    GameStateManager.PushState(_game.GameStates["MENU"]);
                }
            }
        }

        public void Draw(SpriteBatch a_spriteBatch)
        {
            //-------------------------------------------------------------------
            // Title drawing variables
            //-------------------------------------------------------------------

            // Position
            Vector2 titlePos = new Vector2();
            titlePos.X = _game.GraphicsManager.PreferredBackBufferWidth / 2;
            titlePos.Y = (_game.GraphicsManager.PreferredBackBufferHeight / 2) - _font.MeasureString("Continue").Y;

            // Origin
            Vector2 titleOrigin = new Vector2();
            titleOrigin.X = _font.MeasureString("PAUSED").X / 2;
            titleOrigin.Y = _font.MeasureString("PAUSED").Y / 2;

            //-------------------------------------------------------------------
            
            
            //-------------------------------------------------------------------
            // CONTINUE drawing variables
            //-------------------------------------------------------------------

            // Position
            Vector2 continuePos = new Vector2();
            continuePos.X = titlePos.X;
            continuePos.Y = _game.GraphicsManager.PreferredBackBufferHeight / 2;

            // Origin
            Vector2 continueOrigin = new Vector2();
            continueOrigin.X = _font.MeasureString("Continue").X / 2;
            continueOrigin.Y = _font.MeasureString("Continue").Y / 2;

            // Colour
            Color continueColour;
            if (_resumeSelected)
            {
                continueColour = Color.Green;
            }
            else
            {
                continueColour = Color.White;
            }

            //-------------------------------------------------------------------


            //-------------------------------------------------------------------
            // QUIT TO MENU drawing variables
            //-------------------------------------------------------------------

            // Position
            Vector2 menuPos = new Vector2();
            menuPos.X = continuePos.X;
            menuPos.Y = continuePos.Y + _font.MeasureString("Continue").Y;

            // Origin
            Vector2 menuOrigin = new Vector2();
            menuOrigin.X = _font.MeasureString("Back To Menu").X / 2;
            menuOrigin.Y = _font.MeasureString("Back To Menu").Y / 2;

            // Colour
            Color menuColour;
            if (!_resumeSelected)
            {
                menuColour = Color.Green;
            }
            else
            {
                menuColour = Color.White;
            }

            //-------------------------------------------------------------------


                a_spriteBatch.DrawString(_font, "PAUSED", titlePos, Color.White, 0.0f, titleOrigin, Vector2.One, SpriteEffects.None, 0.0f);
                a_spriteBatch.DrawString(_font, "Continue", continuePos, continueColour, 0.0f, continueOrigin, Vector2.One, SpriteEffects.None, 0.0f);
                a_spriteBatch.DrawString(_font, "Back To Menu", menuPos, menuColour, 0.0f, menuOrigin, Vector2.One, SpriteEffects.None, 0.0f);
        }
        #endregion
    }
}
