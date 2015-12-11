///Author Butters.!!!!


using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Wrath_of_the_Dodecagons.GameObjects;

namespace Wrath_of_the_Dodecagons.ParticleManager
{
    public class Particle
    {
        //Properties
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public int TimeToLive { get; set; } 
        //Member Variables
        Vector2 _velocity;
        float _angle;
        float _angularVelocity;
        Color _color;
        float _size;
        
        /// <summary>
        /// Particle Constructor. 
        /// Sets its Lifespan and Velocity within the constructor.
        /// Takes in a Position at where they are to spawn,
        /// The Direction that is used for this should be the exact oppisite of the Bullet Direction
        /// </summary>
        /// <param name="a_pos"></param>
        /// <param name="a_dir"></param>
       public Particle(Texture2D a_texture, Vector2 a_position, Vector2 a_velocity,
            float a_angle, float a_angularVelocity, Color a_color, float a_size, int a_ttl)
        {
            Texture = a_texture;
            Position = a_position;
            _velocity = a_velocity;
            _angle = a_angle;
            _angularVelocity = a_angularVelocity;
            _color = a_color;
            _size = a_size;
            TimeToLive = a_ttl;
        }

        /// <summary>
        /// Upadtes the Particles On Screen
        /// </summary>      
        public void Update()
        {
            TimeToLive--;
            Position += _velocity;
            _angle += _angularVelocity;
        }

        /// <summary>
        /// Creates a new Source Rectangle and Origin then draws the Particle
        /// </summary>
        /// <param name="a_spriteBatch"></param> Spritebatch
        public  void Draw(SpriteBatch a_spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);


            a_spriteBatch.Draw(Texture, Position,sourceRectangle, 
                             _color,_angle,origin,_size,SpriteEffects.None,0f);
        }
    }
}

