using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Wrath_of_the_Dodecagons.States;
using Wrath_of_the_Dodecagons.States.Save_Load;

namespace Wrath_of_the_Dodecagons.States.Menu
{
    class Menu : IState
    {
        //References
        Game1 _game;
        //Button Textures
        Texture2D _playButton;
        Texture2D _scoreButton;
        Texture2D _quitButton;
        Texture2D _optionButton;

        //Button Color
        Color _color;
        //Title Texture
        Texture2D _title;

        //Logo Texture
        Texture2D _logoTexture;
        //Current Selected
        int _currentSelected;
      
        //Game Size
        int _gameHeight;
        int _gameWidth;

        //Button Sizes
        int _buttonWidth;
        int _buttonHeight;

        //Timer for Button press
        float _cdTimer;

        //Bools to Detect Keypresses
        bool _isSelectDown; 
        bool _downKeyPressed;
        bool _upKeyPressed;

        /// <summary>
        /// Constructor of Menu
        /// </summary>
        /// <param name="a_game"></param>
        public Menu(Game1 a_game)
        {
            //Game Class
            _game = a_game;

            //Load Content
            _scoreButton = _game.Content.Load<Texture2D>("Scores");
            _playButton =  _game.Content.Load<Texture2D>("Play");
            _quitButton =  _game.Content.Load<Texture2D>("Quit");
            _optionButton = _game.Content.Load<Texture2D>("Options");         

            _title = _game.Content.Load<Texture2D>("Title");
            _logoTexture = _game.Content.Load<Texture2D>("White Logo");

            //Game Height and width Cut down by 10
            _gameHeight = _game.GraphicsManager.PreferredBackBufferHeight / 10;
            _gameWidth = (_game.GraphicsManager.PreferredBackBufferWidth / 10) * 2;
        
            //Based of the game size
            _buttonWidth = _gameWidth  ;
            _buttonHeight = _gameHeight;

            _cdTimer = 1.0f;
            _color = LoadColor();
        }

        public void Update()
        {

#if PSM
        _isSelectDown = _game.GamePadState.IsButtonDown(Buttons.A) &&
            _game.LastGamePadState.IsButtonUp(Buttons.A);
#else
            _isSelectDown = _game.KeyboardState.IsKeyDown(Keys.Enter) &&
                _game.LastKeyboardState.IsKeyUp(Keys.Enter);
#endif
            //Select Key
            if(_isSelectDown == true)
            {
                //Playing state
                if (_currentSelected == 0)
                {
                    GameStateManager.PopState();
                    GameStateManager.PushState(_game.GameStates["GAME"]);
                }
                 //options menu
                else if(_currentSelected == 1)
                {

                    GameStateManager.PushState(_game.GameStates["OPTIONS"]);
                    _color = LoadColor();
                }
                //End the Torture
                else if(_currentSelected == 2)
                {
                     _game.Exit();
                }
            }
#if PSM
            _downKeyPressed = _game.GamePadState.DPad.Down == ButtonState.Pressed &&
                _game.LastGamePadState.DPad.Down == ButtonState.Released;


#else
            _downKeyPressed = _game.KeyboardState.IsKeyDown(Keys.Down) &&
                _game.LastKeyboardState.IsKeyUp(Keys.Down);
#endif
            //DOWN KEY
            if (_downKeyPressed == true)
            {
                if (_currentSelected < 2)
                {
                    _currentSelected = _currentSelected + 1;
                }
                else
                {
                    _currentSelected = 0;
                }
                _cdTimer = 1.0f;
            }
#if PSM
            _upKeyPressed = _game.GamePadState.DPad.Up == ButtonState.Pressed &&
                _game.LastGamePadState.DPad.Up == ButtonState.Released;


#else
            _upKeyPressed = _game.KeyboardState.IsKeyDown(Keys.Up) &&
                _game.LastKeyboardState.IsKeyUp(Keys.Up);
#endif
            //UP KEY
            if (_upKeyPressed == true)
            {
                if (_currentSelected > 0)
                {
                    _currentSelected = _currentSelected - 1;
                }
                else
                {
                    _currentSelected = 2;
                }
                _cdTimer = 1.0f;
            }

            _cdTimer -= 0.1f;
            //Escape the pain
            if (_game.KeyboardState.IsKeyDown(Keys.Escape) != _game.LastKeyboardState.IsKeyDown(Keys.Escape))
            {
                _game.Exit();
            }
           

        }

        public void Draw(SpriteBatch a_spriteBatch)
        {

            // Current Select Code, Whatever is selected will be yellow
            if (_currentSelected == 0)
            {
                a_spriteBatch.Draw(_playButton, 
                                    new Rectangle(_gameWidth, _gameHeight * 4,_buttonWidth, _buttonHeight),
                                    Color.Yellow);

                a_spriteBatch.Draw(_optionButton,
                                     new Rectangle(_gameWidth, (int)(_gameHeight * 5.5f), _buttonWidth, _buttonHeight),
                                    _color);

                a_spriteBatch.Draw(_quitButton,
                                     new Rectangle(_gameWidth, _gameHeight * 7, _buttonWidth, _buttonHeight),
                                    _color);
            }
            else if (_currentSelected == 1) 
            {
                a_spriteBatch.Draw(_playButton,
                                  new Rectangle(_gameWidth, _gameHeight * 4, _buttonWidth, _buttonHeight),
                                    _color);

                a_spriteBatch.Draw(_optionButton,
                                  new Rectangle(_gameWidth, (int)(_gameHeight * 5.5f), _buttonWidth, _buttonHeight),
                                  Color.Yellow);

                a_spriteBatch.Draw(_quitButton,
                                     new Rectangle(_gameWidth, _gameHeight * 7, _buttonWidth, _buttonHeight),
                                     _color);
            }
            else if (_currentSelected == 2) 
            {
                a_spriteBatch.Draw(_playButton,
                                    new Rectangle(_gameWidth, _gameHeight * 4, _buttonWidth, _buttonHeight),
                                    _color);

                a_spriteBatch.Draw(_optionButton,
                                    new Rectangle(_gameWidth, (int)(_gameHeight * 5.5f), _buttonWidth, _buttonHeight),
                                    _color);

                a_spriteBatch.Draw(_quitButton,
                                    new Rectangle(_gameWidth, _gameHeight * 7, _buttonWidth, _buttonHeight),
                                    Color.Yellow);
            }


            a_spriteBatch.Draw(_title,
                new Rectangle(_gameWidth / 2, _gameHeight, _gameWidth * 4, _gameHeight * 2),
                _color);

            a_spriteBatch.Draw(_logoTexture,
                new Rectangle((int)(_gameWidth * 2.5f), _gameHeight * 4, _gameWidth * 2, _gameHeight * 4),
                _color); 

        }

        private Color LoadColor()
        {
#if PSM
			string directory = "Documents";
#else
			string directory = _game.Content.RootDirectory;
#endif
            Color LoadedColor = new Color();
            if (File.Exists(directory + "/BinaryColor"))
            {
                BinaryReader ReadFile = new BinaryReader(File.Open(directory + "/BinaryColor", FileMode.Open));

                int r, g, b;
                r = ReadFile.ReadInt32();
                g = ReadFile.ReadInt32();
                b = ReadFile.ReadInt32();
                ReadFile.Close();
                ReadFile.Dispose();

                LoadedColor = new Color(r, g, b, 0);
            }
            else
            {
                FileStream fs = File.Open(directory + "/BinaryColor", FileMode.Create);
                BinaryWriter WriteFile = new BinaryWriter(fs);


                WriteFile.Write(255);
                WriteFile.Write(255);
                WriteFile.Write(255);

                WriteFile.Close();
                fs.Dispose();
                fs.Close();

                LoadedColor = Color.White;
            }
            return (LoadedColor);
        }
    }
}
