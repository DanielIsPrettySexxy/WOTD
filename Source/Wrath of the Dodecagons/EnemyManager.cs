//Author Butters
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Wrath_of_the_Dodecagons.GameObjects;


namespace Wrath_of_the_Dodecagons
{
    class EnemyManager
    {
        //lists used by the enemy
        public List<Vector2> _path = new List <Vector2>();
        public List<Enemy> ListedEnemies { get; set; }
        List<Enemy> _pendingList = new List<Enemy>();
        
        //Random to make the difficulty different
        Random _rand = new Random();
        //Game Address
        Game1 _game;
        //GUI Crap
        GUI _gui;

        //Wave Stuff
        int _counter = 4;
        int _difficulty = 1;
        int _count = 6;
        /// <summary>
        /// The Progress of the wave.
        /// </summary>
        public bool WaveProgress { get; set; }

        //Timer class again...
        int _timer;

        /// <summary>
        /// Default Constructor
        /// Initilises the enemy manager class that controls all the enemies used by the game
        /// </summary>
        /// <param name="a_game"></param>Game1 Reference
        /// <param name="a_path"></param> Enemy Path from the grid.
        public EnemyManager(Game1 a_game, List<Vector2> a_path)
        {
            ListedEnemies = new List<Enemy>();
            _game = a_game;
            _path = a_path;
            _timer = 30;
           
        }

        /// <summary>
        /// Draws all the enemies
        /// </summary>
        /// <param name="a_spritebatch"></param> Spritebatch
        public void DrawEnemies(SpriteBatch a_spritebatch)
        {
            for (int index = 0; index < ListedEnemies.Count; index++)
            {
                ListedEnemies[index].Draw(a_spritebatch);
            }
        
        }
        /// <summary>
        /// Updates all the enemies
        /// </summary>
        public void UpdateEnemies()
        {
            if (_pendingList.Count != 0 && _timer <= 0 && WaveProgress == true)
            {
                ListedEnemies.Add(_pendingList.ElementAt(_pendingList.Count - 1));
                _pendingList.RemoveAt(_pendingList.Count - 1);
                _timer = 30;
            }
            for (int index = 0; index < ListedEnemies.Count; index++)
            {
                ListedEnemies[index].Update();
            
                // If the enemy's health is at 0 it is removed. Does not Recieve the bounty
                if (ListedEnemies[index].Health > 0 && ListedEnemies[index].AtEnd == true)
                {
                    ListedEnemies.RemoveAt(index);
                    index--;
                    _gui.DamagePlayer();
                }
                //Adds money if the player Destroys the Enemy.
                else if (ListedEnemies[index].Health <= 0)
                {
                    _gui.PlayerMoney += 5;
                    ListedEnemies.RemoveAt(index);
                    index--;
                }
            }
            if (_pendingList.Count == 0 && ListedEnemies.Count == 0) 
            {
                WaveProgress = false;
            }
            _timer -= 1;
        }

        /// <summary>
        /// Generates the wave of enemies
        /// </summary>
        /// <param name="a_count"></param> Amount of Enemies
        /// <param name="a_diffculty"></param> MAx health in a range
        /// <param name="a_color"></param>1-6 color range. Do not go Higher
        /// <returns></returns>
        public List<Enemy> GenerateWave()
        {
            //Counter to increase difficulty
            if (_counter <= 0)
            {
                _difficulty = _difficulty + 5;
                _count += 4;
                _counter = 3;
                //Boss Unit
                _pendingList.Add(new Enemy(_path,_difficulty * 10));

            }

            _count += 1;
            //Code to create wave
            
            for (int i = 0; i < _count; i++)
            {  
                _pendingList.Add(new Enemy(_path,_rand.Next(_difficulty) + 3));
              
            }
            _gui.IncrementWaveCount();
            _counter -= 1;
            
            return ListedEnemies;
        }
        /// <summary>
        /// Gives the EnemyManager GUI functions.
        /// </summary>
        public void InitiliseGui(GUI a_gui)
        {
            _gui = a_gui;
        }
    }
}
