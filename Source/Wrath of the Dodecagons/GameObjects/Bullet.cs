//Author Butters

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Wrath_of_the_Dodecagons.GameObjects
{
    /// <summary>
    /// Class Bullet that will be shoot from towers to hurt enemy's
    /// </summary>
    class Bullet : Entity
    {
        //Direction
        Vector2 _dir;
        //Speed
        float _velocity;
        //Texture
        Shape _sprite;
        const int _size = 5;

        /// <summary>
        /// Bullet takes in a position, Direction and the rotation that determine where the bullet will work
        /// </summary>
        /// <param name="a_pos"></param> Position
        /// <param name="a_dir"></param> Direction
        /// <param name="a_rot"></param> Rotation 
        public Bullet(Vector2 a_pos, Vector2 a_dir, float a_rot, float a_velocity)
        {
            _position = a_pos;
            _dir = a_dir;
            _velocity = a_velocity;
            _rotation = a_rot;
            _sprite = new Shape(_position, _size, 3);
            _dir.Normalize();                
        }   
        /// <summary>
        /// Updates the Bullets on screen.
        /// </summary>    
        public override void Update()
        {
            _position += _dir * _velocity;
            _sprite.Position = _position;
            _sprite.Update();
        }

        /// <summary>
        /// Draws the Bullets on screen
        /// </summary>     
        /// <param name="a_spriteBatch"></param> Spritebatch
        public override void Draw( SpriteBatch a_spriteBatch)
        {
            _sprite.DrawShape(a_spriteBatch,Color.White);
        }

        /// <summary>
        /// Returns The position of the bullet
        /// </summary>
        public Vector2 GetPosition
        {
            get { return _position;}
        }

        /// <summary>
        /// Returns size of shape
        /// </summary>
        public int Size 
        {
            get { return _size;}
        }
        
    }
}
