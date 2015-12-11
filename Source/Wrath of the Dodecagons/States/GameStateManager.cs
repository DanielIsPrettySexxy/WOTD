using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Wrath_of_the_Dodecagons
{
    /// <summary>
    /// A base state class, anything inheriting from this will be considered a state
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// All draw code should go here, this will batch and render Textures. This function needs to be overloaded
        /// </summary>
        /// <param name="a_spriteBatch">SpriteBatch to be passed in so textures can be drawn</param>
        void Draw(SpriteBatch a_spriteBatch);

        /// <summary>
        /// Code that needs to go into the main game loop should be here. This function must be overloaded
        /// </summary>
        void Update();
    }

    /// <summary>
    /// A state manager utilizing a stack, this works in conjunction with children of the State Class
    /// </summary>
    public static class GameStateManager
    {
       //The actual stack that stores the states
        private static Stack<IState> _states = new Stack<IState>();

        /// <summary>
        ///Pushes a game state to the top of the stack
        /// </summary>
        /// <param name="a_state">The state to add to the stack</param>
        public static void PushState(IState a_state)
        {
            _states.Push(a_state);
        }

        /// <summary>
        /// Removes a game state from the top of the stack
        /// </summary>
        public static void PopState()
        {
            _states.Pop();
        }

        /// <summary>
        /// Updates the current top of the stack
        /// </summary>
        public static void UpdateStates()
       {
           _states.First().Update();
       }

        /// <summary>
        /// Draws the current top of the stack
        /// <param name="a_spriteBatch">SpriteBatch to be passed in so textures can be drawn</param>
        /// </summary>
        public static void DrawStates(SpriteBatch a_spriteBatch)
        {
            _states.First().Draw(a_spriteBatch);
        }
    }
}
