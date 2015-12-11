using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace Wrath_of_the_Dodecagons.States.Splash
{
    class Splash : IState
    {
        //Game Class 
        Game1 _game;
        //Our Team Logo
        Texture2D _logo;
        //Float timer for countdown
        float _Timer;

        /// <summary>
        /// Public Constructor for splash screen, Loads up logo as splash
        /// </summary>
        /// <param name="a_game"></param> Game Class
        public Splash(Game1 a_game)
        {
            _game = a_game;
            _logo = _game.Content.Load<Texture2D>("Splash Logo");
            _Timer = 30;
        }
        /// <summary>
        /// Draws the Splash Screen
        /// </summary>
        /// <param name="a_spriteBatch"></param> SpriteBatch
        public void Draw(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.Draw(_logo, new Rectangle(0, 0,
                 _game.GraphicsManager.PreferredBackBufferWidth,
                 _game.GraphicsManager.PreferredBackBufferHeight),
                 Color.White);
        }
        /// <summary>
        /// Updates the Splash Screen
        /// </summary>
        public void Update()
        {
            if (_Timer <= 0)
            {
                GameStateManager.PopState();
                GameStateManager.PushState(_game.GameStates["MENU"]);
            }
            _Timer -= 0.1f;     
        }
    }
}
