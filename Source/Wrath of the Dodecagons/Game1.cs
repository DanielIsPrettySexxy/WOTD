using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Wrath_of_the_Dodecagons;
using Wrath_of_the_Dodecagons.States;
using Wrath_of_the_Dodecagons.States.Game;
using Wrath_of_the_Dodecagons.States.Pause;
using Wrath_of_the_Dodecagons.States.Splash;
using Wrath_of_the_Dodecagons.States.Menu;
using Wrath_of_the_Dodecagons.States.Options;
using Wrath_of_the_Dodecagons.States.Save_Load;
using Wrath_of_the_Dodecagons.Line_Code;

namespace Wrath_of_the_Dodecagons
{
    /// <summary>
    /// Primary game class, housing the entire game itself
    /// </summary>
    public class Game1 : Game
    {
        #region Member Variables

		public static string contentDir;

        // Graphics Variables
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        

        // GameStates
        IState _splashState;
        IState _menuState;
        IState _pauseState;
        IState _gameState;
        IState _scoresState;
        IState _OptionState;
        Scores _referenceToScore;
        // GameStates dictionary
        Dictionary<string, IState> _gameStates = new Dictionary<string,IState>();


        // Input Variables
        KeyboardState _keyboardState, _lastKeyboardState;
        GamePadState _gamePadState, _lastGamePadState;

        #endregion

        #region Properties

        /// <summary>
        /// Current keyboard state during this frame
        /// </summary>
        public KeyboardState KeyboardState
        {
            get { return _keyboardState; }
        }
        /// <summary>
        /// Keyboard state of previous frame
        /// </summary>
        public KeyboardState LastKeyboardState
        {
            get { return _lastKeyboardState; }
        }
        /// <summary>
        /// Current mouse state during this frame
        /// </summary>
        public GamePadState GamePadState
        {
            get { return _gamePadState; }
        }
        /// <summary>
        /// Mouse state of previous frame
        /// </summary>
        public GamePadState LastGamePadState
        {
            get { return _lastGamePadState; }
        }
        /// <summary>
        /// Graphics device manager of game, giving access to various
        /// graphics options, as well as the current screen size
        /// </summary>
        public GraphicsDeviceManager GraphicsManager
        {
            get { return _graphics; }
        }

        /// <summary>
        /// Allows access to all game states
        /// 
        /// SPLASH  = Splash screen
        /// MENU    = Main Menu
        /// GAME    = Main gameplay state
        /// PAUSE   = Pause Screen
        /// SCORES  = Highscores screen
        /// </summary>
        public Dictionary<string, IState> GameStates
        {
            get { return _gameStates; }
        }

        #endregion

        #region Constructor

        public Game1() : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";


            // Set Save files directory
            #if PSM
			    contentDir = "Documents/";
            #else
			    contentDir = Content.RootDirectory + "/";
            #endif
        }

        #endregion

        #region Non-Gameplay Functions

        protected override void Initialize()
        {
            // Sets game resolution
            #if PSM
                _graphics.PreferredBackBufferWidth  = 960;
                _graphics.PreferredBackBufferHeight = 544;
            #else
                _graphics.PreferredBackBufferWidth = 1024;
                _graphics.PreferredBackBufferHeight = 768;
            #endif
            _graphics.ApplyChanges();


            // Initialise input variables
            _keyboardState      = Keyboard.GetState();
            _lastKeyboardState  = _keyboardState;
            _gamePadState       = GamePad.GetState(PlayerIndex.One);
            _lastGamePadState   = _gamePadState;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Construct a new spritebatch for rendering
            _spriteBatch    = new SpriteBatch(GraphicsDevice);

            // Intialise the line drawing class
            LineDraw.Load(this);
                                                           
            // Load all content from each state
            _splashState    = new Splash(this);
            _menuState      = new Menu(this);
            _pauseState     = new Pause(this);
            _scoresState    = _referenceToScore = new Scores(this);
            _gameState      = new Playing(_referenceToScore, this);
            _OptionState    = new Options(this);
            // Push first state onto the Game State Manager
            GameStateManager.PushState(_splashState);

            // Registers game states with the game state dictionary
            _gameStates.Add("SPLASH", _splashState);
            _gameStates.Add("MENU", _menuState);
            _gameStates.Add("GAME", _gameState);
            _gameStates.Add("PAUSE", _pauseState);
            _gameStates.Add("SCORES", _scoresState);
            _gameStates.Add("OPTIONS",_OptionState);
        }

        protected override void UnloadContent()
        {

        }

        #endregion

        #region Game Loop

        protected override void Update(GameTime a_gameTime)
        {
            // Preserve last tick's input state
            _lastKeyboardState  = _keyboardState;
            _lastGamePadState   = _gamePadState;

            // Update input state
            _keyboardState      = Keyboard.GetState();
            _gamePadState       = GamePad.GetState(PlayerIndex.One);


            // Update active game state within GameState manager
            GameStateManager.UpdateStates();
            base.Update(a_gameTime);
        }

        protected override void Draw(GameTime a_gameTime)
        {
            // Clear screen
            GraphicsDevice.Clear(Color.Black);


            // Draw active game state within GameState manager
            _spriteBatch.Begin();
            GameStateManager.DrawStates(_spriteBatch);
            _spriteBatch.End();
            base.Draw(a_gameTime);
        }

        #endregion
    }
}