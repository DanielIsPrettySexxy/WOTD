using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wrath_of_the_Dodecagons.GameObjects;


namespace Wrath_of_the_Dodecagons.ParticleManager
{
    /// <summary>
    /// Particle Engine Class. Will Create all the extra particle effects within the game.
    /// </summary>
    public class ParticleEngine
    {
        private Random _random;

        /// <summary>
        /// Where the particles are to be created
        /// </summary>
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> _particles;
        private List<Texture2D> _textures;
        private Color _color;
        /// <summary>
        /// Property to Set the engine to Active or Deactive.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Constructor for Particle Engine, Takes in a list of textures to allow multiple types of Particles to be Released
        /// Location of where the particles will spawn.
        /// </summary>
        /// <param name="a_textures"></param> List of Texture2d Images
        /// <param name="a_location"></param> Location of where the particles will spawn.
        public ParticleEngine(List<Texture2D> a_textures, Vector2 a_location)
        {
            EmitterLocation = a_location;
            this._textures = a_textures;
            this._particles = new List<Particle>();
            _random = new Random();
        }
        /// <summary>
        /// Generates all the particle properties
        /// </summary>
        /// <returns> Returns a Particle with New properties</returns>
        private Particle GenerateNewPaticle()
        {
            Texture2D texture = _textures[_random.Next(_textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                1f * (float)(_random.NextDouble() * 2 - 1),
                1f * (float)(_random.NextDouble() * 2 - 1)) * 10;

            float angle = 1;
            float angularVelocity = 0.1f * (float)(_random.NextDouble() * 2 - 1);

            RandomColor();
            float size = (float)_random.NextDouble();
            int TimeToLive = 20 * 2;

            return new Particle(texture, position, velocity, angle,
                                angularVelocity, _color, size, TimeToLive);
        }
        /// <summary>
        /// Draws the particles to the screen.
        /// </summary>
        /// <param name="a_spriteBatch"></param> Spritebatch to draw.
        public void Draw(SpriteBatch a_spriteBatch)
        {
            for (int index = 0 ; index < _particles.Count; index++)
            {
                _particles[index].Draw( a_spriteBatch);
            }
        }
        /// <summary>
        /// Updates the Particles on Screen
        /// </summary>   
        public void Update()
        {
            int total = 10;
            if (Active == true)
            {
                for (int i = 0; i < total; i++)
                {
                    _particles.Add(GenerateNewPaticle());
                }
            }
            for (int particle = 0; particle < _particles.Count; particle++)
            {
                _particles[particle].Update();

                if (_particles[particle].TimeToLive <= 0)
                {
                     _particles.RemoveAt(particle);
                     particle--;
                }
            }
          
        }

        /// <summary>
        /// Sets Color to a prefixed color.
        /// </summary>
        /// <param name="a_color"></param> Parameter that controls color.
        public void SetColor(Color a_color)
        {
            _color = a_color;
        }

        /// <summary>
        /// Sets Color to random RGB Value
        /// </summary>
        public void RandomColor()
        {
            _color = new Color(
               (float)_random.NextDouble(),
               (float)_random.NextDouble(),
               (float)_random.NextDouble());
        }

        /// <summary>
        /// Stop the Engine
        /// </summary>
        public void Stop()
        {
            Active = false;
        }

        /// <summary>
        /// Start the Engine
        /// </summary>
        public void Start(Vector2 a_position,int a_limit)
        {
            EmitterLocation = a_position;
            Active = true;
            for(int i = 0 ; i < a_limit; i++)
            {
                _particles.Add(GenerateNewPaticle());
            }
        }
    }
}
