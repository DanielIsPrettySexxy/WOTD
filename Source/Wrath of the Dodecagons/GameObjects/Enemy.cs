///Auther Jason A.k.a Butters



using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Wrath_of_the_Dodecagons.States.Game.Grid_System;


namespace Wrath_of_the_Dodecagons.GameObjects
{
    class Enemy : Entity
    {
        int _health;
        float _velocity;
        const int _size = 32;
        int _index;

        //Texture
        Shape _texture;
       
        //If at the end or not
        public bool AtEnd { get; set; }
        //The Enemy path
        List<Vector2> _path;      
       
        /// <summary>
        /// Public Constructor for Enemy.
        /// Sets Enemy posistion based of the Path.
        /// Sets the Scale of the enemy to 32width, 32 height.
        /// Sets Enemy health based of input.
        /// </summary>
        /// <param name="a_path"></param> The path that the enemy will run along
        /// <param name="a_health"></param> the value of the Total hp's of the enemy
        /// <param name="content"></param> Content Manager for loading the spritesheet
        public Enemy(List<Vector2> a_path,int a_health)
        {
            _health = a_health;
            _path = a_path;
            _velocity = 4.0f;
            AtEnd = false;
            _index = 1;
            _position = new Vector2(1000,500);
            _scale = new Vector2(0.4f, 0.4f);
            //Shape Stuff
            _texture = new Shape(_position, _size, a_health,_rotation);

         
        }

        /// <summary>
        /// Draws the enemy to screen
        /// </summary>
        /// <param name="a_spriteBatch"></param> Spritebatch
        public override void Draw(SpriteBatch a_spriteBatch)
        {
            _texture.DrawShape(a_spriteBatch, Color.White, 3);
        }

        /// <summary>
        /// Updates the enemy on screen
        /// </summary>
        public override void Update()
        {
            _texture.Position = _position;
            _texture.Update();
            if (_index <= _path.Count())
            {
                //Grabs Current node
                Vector2 endNode = _path.ElementAt(_path.Count - _index);
                Vector2 toMove = endNode - _position;

                toMove.Normalize();

                _position += toMove * _velocity;
                Vector2 distance = _position - endNode;
              
                if (distance.Length() <= 7.0f)
                {
                    _position.X += 0.0f;
                    _position.Y += 0.0f;
                    _index = _index + 1;
                }
            }
            else
            {
                AtEnd = true;        
            }
        }

        /// <summary>
        /// Function to allow damage to be dealt
        /// </summary>
        public void TakeDamage()
        {
            _health = _health - 1;
        }

        /// <summary>
        /// Sets HP of enemy based of the Parameters entered.
        /// </summary>
        /// <param name="a_hp"></param>
        public void SetHealth(int a_hp)
        {
            _health = a_hp;
        }

        /// <summary>
        /// Returns the Position of the Enemy.
        /// </summary>
        public Vector2 GetPosition
        {
             get
             {
                 return new Vector2(_position.X,_position.Y); 
             }
        }

        /// <summary>
        /// Returns health
        /// </summary>
        public int Health
        {
            get { return _health; }
        }
        /// <summary>
        /// Returns size
        /// </summary>
        public int Size
        {
            get { return _size; }
        }
    }

}
