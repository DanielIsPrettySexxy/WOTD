using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Wrath_of_the_Dodecagons.States.Game
{
    /// <summary>
    /// This class is the tile pieces that make up the Grid system
    /// </summary>
    class Cell
    {
        //Where the cell is on the second dimention of the gird
        readonly private int _row;
        //The cells position on the second dimention of the gird
        readonly private int _col;
        //On screen location of the cell in pixels
        private Vector2 _position;

        //Describes whther or not a tower can be placed on this Cell
        private bool _occupied;

        /// <summary>
        /// This class is the individual pieces that make up the Grid system
        /// </summary>
        ///  <param name="a_row">An integer describing the row this cell is in</param>
        ///  <param name="a_collum">An integer describing the Collum this cell is in</param>
        ///  <param name="a_position">This Vector2 is the on screen space that this cell occupies</param>
        ///  <param name="a_occupied">A bool that determines whether or not a tower can be placed on this cell</param>
        public Cell(int a_row, int a_collum, Vector2 a_position, bool a_occupied) 
        {
            _row = a_row;
            _col = a_collum;
            _position = a_position;
            _occupied = a_occupied;
        }

        //Getters and Setters

        /// <summary>
        /// Returns a bool as to whether or not a Tower can be placed there
        /// </summary>
        public bool IsOccupied
        {
            get
            {
                return _occupied;
            }
            set
            {
                _occupied = value;
            }
        }

        /// <summary>
        /// Returns the row number of this cell
        /// </summary>
        public int Row
        {
            get
            {
                return _row;
            }
        }

        /// <summary>
        /// Returns the Collum number of this cell
        /// </summary>
        public int Collum
        {
            get
            {
                return _col;
            }
        }


        /// <summary>
        /// Returns the actual screen possition of this cell
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

    }
}
