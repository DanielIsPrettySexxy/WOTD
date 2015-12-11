using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Wrath_of_the_Dodecagons.States;
using Wrath_of_the_Dodecagons.GameObjects;
using Wrath_of_the_Dodecagons.States.Game.Grid_System;

namespace Wrath_of_the_Dodecagons
{
    /// <summary>
    /// The GUI of the game, which allows the player to 
    /// interact with the game (placing towers, etc)
    /// as well as keeps track of the player's stats
    /// (player's money, health, etc) 
    /// </summary>
    class GUI
    {
        #region Member Variables

        // Game references
        Game1 _game;
        Playing _gameState;
        EnemyManager _enemyManager;
        List<Tower> _towerList;
        GameGrid _grid;

        // Player Variables
        int _playerHealth;
        int _waveNumber     = 1;

        // Whether or not the player is buying/placing towers
        bool _inBuyMenu     = false;
        // Whether or not the player is within a wave in progress
        bool _waveStarted   = false;

        // Tower placement variables
        Tower _towerToBePlaced;
        int _towerRow       = 0;
        int _towerCollum    = 0;

        // Font to render text (temporary)
        static SpriteFont _font;

        #endregion

        #region Properties

        /// <summary>
        /// The current health of the player
        /// </summary>
        public int PlayerHealth
        {
            get { return _playerHealth; }
        }
        /// <summary>
        /// The players current amount of money
        /// </summary>
        public int PlayerMoney
        {
            get;
            set;
        }
        /// <summary>
        /// Current wave which the player is playing within
        /// </summary>
        public int PlayerWave
        {
            get { return _waveNumber; }
        }

        /// <summary>
        /// Shows whether or not the player is current
        /// placing/buying towers
        /// </summary>
        public bool InBuyMenu
        {
            get { return _inBuyMenu; }
        }
        #endregion

        #region Constructor

        /// <summary>
        /// Constructs the in-game GUI and begins the game with
        /// the specified amount of health and money to give to the
        /// player, for difficulty variability
        /// </summary>
        /// <param name="a_game">Reference to Game1</param>
        /// <param name="a_towerList">Referece to the list of towers within the main game state</param>
        /// <param name="a_grid">Reference to grid of cells within the main game state</param>
        /// <param name="a_playerHealth">Player health to spawn with</param>
        /// <param name="a_playerMoney">Player money to spawn with</param>
        public GUI(Game1 a_game, Playing a_gameState, List<Tower> a_towerList, GameGrid a_grid,
            EnemyManager a_enemyManager, int a_playerHealth, int a_playerMoney)
        {
            // Assign references to game
            _game = a_game;
            _gameState = a_gameState;
            _towerList = a_towerList;
            _grid = a_grid;
            _enemyManager = a_enemyManager;

            // Assign player stats
            _playerHealth = a_playerHealth;
            PlayerMoney = a_playerMoney;

            // Hide Grid
            _gameState.IsGridDrawing = false;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Increments the wave counter by one
        /// </summary>
        public void IncrementWaveCount()
        {
            _waveNumber++;
        }
        /// <summary>
        /// Subtracts one life from the player
        /// </summary>
        public void DamagePlayer()
        {
            _playerHealth--;
        }

        /// <summary>
        /// Loads all content used by this object
        /// </summary>
        /// <param name="a_content">ContentManager object to load content into</param>
        public static void LoadContent(ContentManager a_content)
        {
            _font = a_content.Load<SpriteFont>("debug_font");
        }

        /// <summary>
        /// Updates the GUI
        /// </summary>
        public void Update()
        {
            // Player is in buy menu
            if (_inBuyMenu && !_waveStarted)
            {
                // Get input
                bool exitBuy, placeTower, leftCell, rightCell, upCell, downCell;
#if PSM
                exitBuy     = _game.GamePadState.IsButtonDown(Buttons.B) &&
                                _game.LastGamePadState.IsButtonUp(Buttons.B);

                placeTower  = _game.GamePadState.IsButtonDown(Buttons.A) &&
                                _game.LastGamePadState.IsButtonUp(Buttons.A);

                leftCell    = _game.GamePadState.DPad.Left == ButtonState.Pressed &&
                                _game.LastGamePadState.DPad.Left == ButtonState.Released;

                rightCell   = _game.GamePadState.DPad.Right == ButtonState.Pressed &&
                                _game.LastGamePadState.DPad.Right == ButtonState.Released;

                upCell      = _game.GamePadState.DPad.Up == ButtonState.Pressed &&
                                _game.LastGamePadState.DPad.Up == ButtonState.Released;

                downCell    = _game.GamePadState.DPad.Down == ButtonState.Pressed &&
                                _game.LastGamePadState.DPad.Down == ButtonState.Released;
#else
                exitBuy     = _game.KeyboardState.IsKeyDown(Keys.Escape) &&
                                _game.LastKeyboardState.IsKeyUp(Keys.Escape);
                placeTower  = _game.KeyboardState.IsKeyDown(Keys.Enter) &&
                                _game.LastKeyboardState.IsKeyUp(Keys.Enter);
                leftCell    = _game.KeyboardState.IsKeyDown(Keys.Left) &&
                                _game.LastKeyboardState.IsKeyUp(Keys.Left);
                rightCell   = _game.KeyboardState.IsKeyDown(Keys.Right) &&
                                _game.LastKeyboardState.IsKeyUp(Keys.Right);
                upCell      = _game.KeyboardState.IsKeyDown(Keys.Up) &&
                                _game.LastKeyboardState.IsKeyUp(Keys.Up);
                downCell    = _game.KeyboardState.IsKeyDown(Keys.Down) &&
                                _game.LastKeyboardState.IsKeyUp(Keys.Down);
#endif


                if (exitBuy)
                {
                    // Get out of buy menu
                    _inBuyMenu = false;

                    // Hide Grid
                    _gameState.IsGridDrawing = false;
                }
                else
                {
                    if (placeTower)
                    {
                        if (_grid.Elements[_towerRow, _towerCollum].IsOccupied == false &&
                            PlayerMoney >= 100)
                        {
                            // Set cell taken up by this tower, to occupied
                            _grid.Elements[_towerRow, _towerCollum].IsOccupied = true;

                            // Place tower and subtract money
                            _towerList.Add(_towerToBePlaced);
                            PlayerMoney -= 100;

                            // Initialise tower to be placed
                            _towerToBePlaced = new Tower(_gameState, _grid.Elements[_towerRow, _towerCollum].Position,
                                                            488.0f, _enemyManager.ListedEnemies);
                        }
                    }
                    else if (leftCell && _towerCollum != 0)
                    {
                        // Move tower placement left one cell
                        _towerCollum--;
                    }
                    else if (rightCell && 
                        _towerCollum + 1 < _grid.Elements.GetLength(1))
                    {
                        // Move tower placement right one cell
                        _towerCollum++;
                    }
                    else if (upCell && _towerRow != 0)
                    {
                        // Move tower placement up one cell
                        _towerRow--;
                    }
                    else if (downCell &&
                        _towerRow + 1 < _grid.Elements.GetLength(0))
                    {
                        // Move tower placement down one cell
                        _towerRow++;
                    }

                    // Move tower to current cell position
                    _towerToBePlaced.Update();
                    Vector2 towerNewPos = _grid.Elements[_towerRow, _towerCollum].Position;
                    towerNewPos.X += _towerToBePlaced.Radius;
                    towerNewPos.Y += _towerToBePlaced.Radius / 2.0f + 16;
                    _towerToBePlaced.Position = towerNewPos;
                }
            }
            // Player is outside of buy menu
            else
            {
                // Get input
                bool startWave, enterBuyMenu;
#if PSM
                startWave       = _game.GamePadState.IsButtonDown(Buttons.X) &&
                                    _game.LastGamePadState.IsButtonUp(Buttons.X);
                enterBuyMenu    = _game.GamePadState.IsButtonDown(Buttons.Y) &&
                                    _game.LastGamePadState.IsButtonUp(Buttons.Y);
#else
                startWave       = _game.KeyboardState.IsKeyDown(Keys.Space) &&
                                    _game.KeyboardState.IsKeyDown(Keys.Space);
                enterBuyMenu    = _game.KeyboardState.IsKeyDown(Keys.B) &&
                                    _game.LastKeyboardState.IsKeyUp(Keys.B);
#endif

                if (!_waveStarted)
                {
                    // Start wave
                    if (startWave)
                    {
                        _enemyManager.GenerateWave();
                        _enemyManager.WaveProgress = true;

                        _waveStarted = true;
                    }
                    // Go into buy menu
                    else if (enterBuyMenu)
                    {
                        // Enable buy menu
                        _inBuyMenu = true;

                        // Show Grid
                        _gameState.IsGridDrawing = true;

                        // Initialise tower to be placed
                        _towerToBePlaced = new Tower(_gameState, _grid.Elements[_towerRow, _towerCollum].Position, 
                                                        100.0f, _enemyManager.ListedEnemies);
                    }
                }
            }


            // Update wave progress bool
            _waveStarted = _enemyManager.WaveProgress;
        }

        /// <summary>
        /// Draws the GUI
        /// </summary>
        /// <param name="a_spriteBatch">SpriteBatch to render to</param>
        public void Draw(SpriteBatch a_spriteBatch)
        {
            // Player is in buy menu
            if (_inBuyMenu)
            {
                //-------------------------------------------------
                // PLACING TOWERS TEXT
                //-------------------------------------------------

                // PLACING TOWERS position
                Vector2 buyMenu_pos = new Vector2();
                buyMenu_pos.X = _game.GraphicsManager.PreferredBackBufferWidth / 2;
                buyMenu_pos.Y = _font.MeasureString("PLACING TOWERS").Y * 2.0f;
                // PLACING TOWERS origin
                Vector2 buyMenu_origin = new Vector2();
                buyMenu_origin.X = _font.MeasureString("PLACING TOWERS").X / 2;
                buyMenu_origin.Y = _font.MeasureString("PLACING TOWERS").Y / 2;
                // Draw text
                a_spriteBatch.DrawString(_font, "PLACING TOWERS", buyMenu_pos, Color.Red, 0.0f, buyMenu_origin, Vector2.One * 2.0f, SpriteEffects.None, 0.0f);

                //-------------------------------------------------


                _towerToBePlaced.Draw(a_spriteBatch);
            }


            //---------------------------------
            // Player is not in buy menu
            //---------------------------------

            // Draws current player health
            a_spriteBatch.DrawString(_font, "Health: " + _playerHealth, Vector2.Zero, Color.Red);

            // Draws current wave number
            Vector2 waveNumber_pos = new Vector2();
            waveNumber_pos.Y = _font.MeasureString("Health: " + _playerHealth).Y;
            a_spriteBatch.DrawString(_font, "Wave: " + _waveNumber, waveNumber_pos, Color.White);

            // Draws current player money
            Vector2 playerMoney_pos = new Vector2();
            playerMoney_pos.Y = _font.MeasureString("Wave: " + _waveNumber).Y + waveNumber_pos.Y;
            a_spriteBatch.DrawString(_font, "Money: " + PlayerMoney, playerMoney_pos, Color.LightGreen);

            //---------------------------------


            // Draw text displaying if a wave is in progress or not
            string waveProgress = "Wave Started: ";
            if (_enemyManager.WaveProgress)
            {
                waveProgress += "Yes";
            }
            else
            {
                waveProgress += "No";
            }

            a_spriteBatch.DrawString(_font, waveProgress, new Vector2(0.0f, playerMoney_pos.Y + _font.MeasureString("Money: ").Y), Color.Red);
        }

        #endregion
    }
}