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
    /// Provides basic update and draw functionality
    /// </summary>
    abstract class Entity
    {
        #region Member Variables
        /// <summary>
        /// Position of this entity within the game environment
        /// </summary>
        protected Vector2 _position;

        /// <summary>
        /// Scale of this entity
        /// </summary>
        protected Vector2 _scale;

        /// <summary>
        /// Rotation of this entity relative to the game environement
        /// </summary>
        protected float _rotation;
        #endregion

        #region
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Processes game logic on this game object
        /// </summary>
        public abstract void Update();
        /// <summary>
        /// Draws this object and executes related rendering processing
        /// </summary>
        /// <param name="a_spriteBatch">Spritebatch to render to</param>
        public abstract void Draw(SpriteBatch a_spriteBatch);
        #endregion
    }
}
