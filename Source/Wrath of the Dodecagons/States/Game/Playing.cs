using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Wrath_of_the_Dodecagons.States.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Wrath_of_the_Dodecagons.GameObjects;
using Wrath_of_the_Dodecagons.ParticleManager;
using Wrath_of_the_Dodecagons.States.Game.Grid_System;
using Wrath_of_the_Dodecagons.States.Save_Load;
using Wrath_of_the_Dodecagons.Line_Code;

namespace Wrath_of_the_Dodecagons.States
{
    /// <summary>
    /// GameState where the main gameplay will occur
    /// </summary>
    class Playing : IState
    {
        #region Member Variables

        //-----------------------------------
        // Grid variables
        //-----------------------------------
        #if PSM
          const uint _col = 8, _row = 10;
#else
         const uint _col = 12, _row = 12;
#endif
        //The number of Grid collums and rows
        //The size of each cell
        const uint _cellSize = 64;

        //-----------------------------------


        // Reference to Game
        Game1 _game;

        // GUI
        GUI _gui;

        // Game lists
        List<Tower> _towerList      = new List<Tower>();
        List<Enemy> _enemyList      = new List<Enemy>();
        List<Bullet> _bulletList    = new List<Bullet>();
        List<Texture2D> _sprites    = new List<Texture2D>();

        // Managers
        EnemyManager _enemy;
        ParticleEngine _particle;
        Collision _collision; 

        // Path which enemies will follow
        List<Vector2> _enemyPath;
         
        //Score reference to pass on wave number and cash
        Scores _referenceToScores;
        #endregion

        #region Properties

        /// <summary>
        /// The Grid used within game for placement of tower
        /// and placement of the enemy path
        /// </summary>
        public GameGrid Grid { get; set; }

        /// <summary>
        /// Whether or not the grid within the game is currently
        /// being rendered to the screen
        /// </summary>
        public bool IsGridDrawing { get; set; }

        #endregion

        #region Constructor

        public Playing(Scores a_scores, Game1 a_game)
        {
            // Adds given reference to game
            _game = a_game;
            _referenceToScores = a_scores;

            // Start up Grid
            Grid = new GameGrid(_col,_row,_cellSize);
            IsGridDrawing = true;
            _enemyPath = new List<Vector2>();
            _enemyPath = Grid.GeneratePath();
            
            // Start up all the managers that will be used.
            _enemy = new EnemyManager(_game,_enemyPath);
            _particle = new ParticleEngine(_sprites, new Vector2(300, 300));
            _collision = new Collision(_enemy.ListedEnemies, _bulletList, _particle);
            _gui = new GUI(a_game, this, _towerList, Grid, _enemy, 50 ,500);

            // Gives enemy Manager Gui functions
            _enemy.InitiliseGui(_gui);
            _collision.InitiliseGui(_gui);

            // Load content for game objects used within this state
            LoadParticles();
            GUI.LoadContent(_game.Content);
        }
        #endregion

        #region Game Loop
        public void Update()
        {
            // Checking Collisions
            if (_enemy.ListedEnemies.Count != 0) 
            {
                _collision.CheckCollision();
            }

            if(_gui.PlayerHealth <= 0)
            {
                _referenceToScores.SetStats(_gui.PlayerWave,_gui.PlayerMoney);
                GameStateManager.PushState(_game.GameStates["SCORES"]);
            }
            // Update Particles
            #if !PSM
                _particle.Update();
            #endif

            // Update all towers
            for (int i = 0; i < _towerList.Count; i++)
            {
                _towerList[i].Update();
            }

            // Update all enemies
            _enemy.UpdateEnemies();

            // Update all bullets
            for (int i = 0; i < _bulletList.Count; i++)
            {
                _bulletList[i].Update();
                if (_bulletList[i].GetPosition.X >
                    _game.GraphicsManager.PreferredBackBufferWidth ||
                    _bulletList[i].GetPosition.X < 0)
                {
                    _bulletList.RemoveAt(i);
                    i--;
                }
                else if((_bulletList[i].GetPosition.Y >
                    _game.GraphicsManager.PreferredBackBufferHeight ||
                    _bulletList[i].GetPosition.Y < 0))
                {
                     _bulletList.RemoveAt(i);
                    i--;

                }

            }

            // GUI update
            _gui.Update();
        }

        public void Draw(SpriteBatch a_spriteBatch)
        {
                // Draws Grid
                if (IsGridDrawing)
                {
                    for (int I = 0; I < Grid.Elements.GetLength(0); ++I)
                    {
                        for (int J = 0; J < Grid.Elements.GetLength(1); ++J)
                        {
                            Vector2[] square = new Vector2[4];
                            Vector2 Xmod = new Vector2(64, 0);
                            Vector2 Ymod = new Vector2(0, 64);
                            square[0] = Grid.Elements[I, J].Position;
                            square[1] = square[0] + Xmod;
                            square[2] = square[1] + Ymod;
                            square[3] = square[0] + Ymod;
                            if (!Grid.Elements[I, J].IsOccupied)
                            {
                                Line_Code.LineDraw.DrawPoly(a_spriteBatch, square, Color.White);
                            }
                            else
                            {
                                Line_Code.LineDraw.DrawPoly(a_spriteBatch, square, Color.Red);
                            }
                        }
                    }
                }
                
                // Draw all towers
                for (int i = 0; i < _towerList.Count; i++)
                {
                    _towerList[i].Draw(a_spriteBatch);
                }
                // Draw all enemies
                _enemy.DrawEnemies(a_spriteBatch);
                // Draw all bullets
                for (int i = 0; i < _bulletList.Count; i++)
                {
                    _bulletList[i].Draw(a_spriteBatch);
                }
                // Draw all Particles
#if !PSM
                _particle.Draw(a_spriteBatch);
#endif
                // Draws GUI
                _gui.Draw(a_spriteBatch);


              
        }
        #endregion

        #region Misc Functions

        /// <summary>
        /// Creates a bullet within the game
        /// </summary>
        /// <param name="a_position">Position to spawn the bullet</param>
        /// <param name="a_direction">Direction this bullet should travel</param>
        /// <param name="a_rotation">Rotation of this bullet's sprite</param>
        /// <param name="a_velocity">Speed at which this bullet will travel</param>
        public void CreateBullet(Vector2 a_position, Vector2 a_direction, float a_rotation, float a_velocity)
        {
            // Create bullet
            Bullet bullet = new Bullet(a_position, a_direction, a_rotation, a_velocity);
            // Add bullet to master bullet list
            _bulletList.Add(bullet);
        }

        /// <summary>
        /// Loads the content for particles.
        /// </summary>
        public void LoadParticles()
        {
            _sprites.Add(new Texture2D(_game.GraphicsDevice, _game.GraphicsManager.PreferredBackBufferHeight / 32,
                                    _game.GraphicsManager.PreferredBackBufferWidth / 32, false, SurfaceFormat.Bgra4444));
        }
        
        #endregion
    }
}