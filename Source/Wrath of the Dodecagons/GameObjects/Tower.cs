using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Wrath_of_the_Dodecagons.States;

namespace Wrath_of_the_Dodecagons.GameObjects
{
    /// <summary>
    /// Placeable towers which will eliminate enemies
    /// within its range
    /// </summary>
    class Tower : Entity
    {
        #region Member Variables

        // Game object reference
        Playing _game;

        // Drawing Variables
        Shape _bodyTex;
        Shape _turrentTex;
        const int _size = 32;
        // Rotation of turrent
        float _turrentRot;

        // List of enemies currently within the game
        List<Enemy> _enemyList;

        // Shooting Variables
        Timer _timer = new Timer();
        Enemy _closestEnemy;
        float _range;
        int _shootCool = 15; // 60 = one second
        int _shootCurrent;

        #endregion

        #region  Properties

        /// <summary>
        /// Returns the radius taken up by the rendered shape
        /// </summary>
        public float Radius
        {
            get { return _bodyTex.Radius; }
        }

        #endregion

        #region Constructor
        /// <summary>
        /// Creates an enemy at a given position
        /// </summary>
        /// <param name="a_position">Position to create tower at</param>
        /// <param name="a_range">Shooting range of this tower</param>
        /// <param name="a_enemyList">Lists of enemies within the game</param>
        public Tower(Playing a_game, Vector2 a_position, float a_range, List<Enemy> a_enemyList)
        {
            // Game variables
            _game           = a_game;
            _enemyList      = a_enemyList;
            // Tower variables
            _scale          = new Vector2(0.5f);
            _position       = a_position;
            _range          = a_range;

            _bodyTex = new Shape(_position, _size, 4, 0.7853975f);
            _turrentTex = new Shape(_position,(_size / 2),3);
        }
        #endregion

        #region Private Functions

        /// <summary>
        /// Fires a bullet towards the given enemy
        /// </summary>
        /// <param name="a_enemy">Enemy to shoot at</param>
        /// <param name="a_gameTime">Used for calculating shoot cooldown</param>
        private void Shoot()
        {
            // Get current distance to target
            float targetDist = Vector2.Distance(_closestEnemy.GetPosition, _position);

            if (_shootCurrent <= 0.0d && targetDist <= _range)
            {
                // Rotate turrent towards target
                _turrentRot = AngleBetween(_closestEnemy.GetPosition - _position, new Vector2(0.0f, -1.0f));


                // Bullet spawn position
                Vector3 shootPos = new Vector3(0, -(float)(_turrentTex.Radius * _scale.Y), 1);
                Matrix shootMatrix = Matrix.CreateRotationZ(_turrentRot);
                shootMatrix.Translation = new Vector3(_position, 0);
                shootPos = Vector3.Transform(shootPos, shootMatrix);
                // Bullet spawn direction
                Vector2 shootDir = _closestEnemy.GetPosition - _position;
                shootDir.Normalize();
                // Bullet spawn speed
                float shootVelo = 20.0f;


                

                // Shoot bullet
                _game.CreateBullet(new Vector2(shootPos.X, shootPos.Y), shootDir, _turrentRot, shootVelo);
                // Reset shooting timer
                _shootCurrent = _shootCool;
            }
            else
            {
                // Tick shoot timer
                _shootCurrent -= 1; // 1 millisecond per tick (60 ticks per second)
            }
        }

        /// <summary>
        /// Selects the closest enemy to shoot at
        /// (Must be used with a list which isn't empty or will crash)
        /// </summary>
        /// <returns>Selected Enemy</returns>
        private Enemy GetClosestEnemy()
        {
            // Get distance to first enemy in list
            float shortestDist = (_enemyList[0].GetPosition - _position).LengthSquared();
            Enemy closestEnemy = _enemyList[0];
            for (int i = 1; i < _enemyList.Count; i++)
            {
                // Get distance to current enemy
                float currentDist = (_enemyList[i].GetPosition - _position).LengthSquared();

                // Check if a closer enemy is found so far
                if (currentDist < shortestDist)
                {
                    // Replace closest enemy with this enemy
                    shortestDist = currentDist;
                    closestEnemy = _enemyList[i];
                }
            }


            // Return closest found enemy
            return closestEnemy;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Processes game logic on this game object
        /// </summary>
        /// <param name="a_gameTime">Global game time used for time based processing</param>
        public override void Update()
        {
            _bodyTex.Position = _position;
            _bodyTex.Update();

            _turrentTex.Position = _position;
            _turrentTex.Update();
            _turrentTex.Rotation = (_turrentRot + 0.7853975f);

            if (_enemyList != null)
            {
                if (_enemyList.Count > 0)
                {
                    // Get closest enemy to tower
                    _closestEnemy = GetClosestEnemy();

                    // Shoot at enemy
                    Shoot();
                }
            }
        }

        /// <summary>
        /// Draws this object and executes related rendering processing
        /// </summary>
        /// <param name="a_gameTime">Global game time used for time based processing</param>
        public override void Draw(SpriteBatch a_spriteBatch)
        {
            _bodyTex.DrawShape(a_spriteBatch, Color.Green, 2);
            _turrentTex.DrawShape(a_spriteBatch, Color.Green);
        }

        #endregion

        #region Helper Functions

        private float AngleBetween(Vector2 a_currentPos, Vector2 a_targetPos)
        {
            // Get normalised verions of arguments
            Vector2 currentPos = a_currentPos;
            currentPos.Normalize();
            Vector2 targetPos = a_targetPos;
            targetPos.Normalize();


            // Perpendicular of currentPos
            Vector2 perp = new Vector2(currentPos.Y, -currentPos.X);

            // Dot product of currentPos
            float dot = Vector2.Dot(currentPos, targetPos);
            // Clamp dot product result to within acos limits
            if (dot > 1.0f)
            {
                dot = 1.0f;
            }
            else if (dot < -1.0f)
            {
                dot = -1.0f;
            }


            // Left or right angle decision
            float angle = (float)Math.Acos(dot);
            if (Vector2.Dot(targetPos, perp) > 0.0f)
            {
                return angle;
            }
            else
            {
                return -angle;
            }
        }

        #endregion
    }
}