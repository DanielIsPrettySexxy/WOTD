///Author Jason Butterfield
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wrath_of_the_Dodecagons.GameObjects;
using Microsoft.Xna.Framework;
using Wrath_of_the_Dodecagons.ParticleManager;

namespace Wrath_of_the_Dodecagons
{
    class Collision
    {
        //Enemy list
        List<Enemy> _enemies = new List<Enemy>();
        //Bullet list
        List<Bullet> _bullets = new List<Bullet>();
        //Particle Engine
        ParticleEngine _particle;
        //GUI
        GUI _gui;
        /// <summary>
        /// Default Constructor for Collision Checks
        /// </summary>
        /// <param name="a_enemies"></param> Enemy List
        /// <param name="a_bullets"></param> Bullet list
        /// <param name="a_engine"></param> Particle Engine
        public Collision(List<Enemy> a_enemies, List<Bullet> a_bullets,ParticleManager.ParticleEngine a_engine)
        {
            _enemies = a_enemies;
            _bullets = a_bullets;
            _particle = a_engine;
        }

        /// <summary>
        /// Checks all the current enemies and bullets to see if any of them are colliding.
        /// </summary>
        public void CheckCollision()
        {
            for (int enemy = 0; enemy < _enemies.Count; enemy++)
            {
                //Grabs Current Enemy
                Enemy currentEnemy = _enemies.ElementAt(enemy);
                //Checks everysingle bullet to that enemy
                for (int bullet = 0; bullet < _bullets.Count; bullet++)
                {
                    //Checks the distance of the bullet and enemy. If they are close it will annouce Collision
                    if (Vector2.Distance(currentEnemy.GetPosition, _bullets.ElementAt(bullet).Position) 
                                            < currentEnemy.Size / 2 + _bullets.ElementAt(bullet).Size / 2) 
                    {
                        //Start up Particle Engine
                        _particle.Start(_bullets.ElementAt(bullet).Position, 10);
                        //Enemy takes damage
                        currentEnemy.TakeDamage();
                        //Removes the bullet that collided with enemy.
                        _bullets.RemoveAt(bullet);
                        bullet--;
                        //Stop the particle flow.
                        _gui.PlayerMoney += 2;
                        _particle.Stop();
                       
                    }

                }
             
            }
        
        }

        /// <summary>
        /// Allows Collision to do Gui functions
        /// </summary>
        /// <param name="a_gui"></param> GUI
        public void InitiliseGui(GUI a_gui)
        {
            _gui = a_gui;
        }
    }
}
