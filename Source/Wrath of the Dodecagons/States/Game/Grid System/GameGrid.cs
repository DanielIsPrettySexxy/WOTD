using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Wrath_of_the_Dodecagons.States.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Wrath_of_the_Dodecagons.GameObjects;


namespace Wrath_of_the_Dodecagons.States.Game.Grid_System
{
    /// <summary>
    /// The grid that the game will run on
    /// </summary>
    class GameGrid
    {
        //Amount of rows and collums the grid will have. These can only be set once
        readonly uint _rows, _collums;
        //How big each cell will be, this defaults to one and cannot be rest
        readonly uint _cellSize;
        //Actual elements that the gird contains
        public Cell[,] Elements;

        /// <summary>
        /// Constructs a Grid for the game to use, for placing towers and enemy paths
        /// </summary>
        /// <param name="a_rows">The amount of rows the grid contains</param>
        /// <param name="a_collums">The amount of collums the grid contains</param>
        /// <param name="a_size">The size of each cell element</param>
        public GameGrid(uint a_rows, uint a_collums, uint a_size = 1) 
        {
            _rows = a_rows;
            _collums = a_collums;
            _cellSize = a_size;

            Elements = new Cell[_rows, _collums];
            Vector2 gridPosition = new Vector2();
            for (int I = 0; I < _rows; I++)
            {
                for (int J = 0; J < _collums; J++)
                {
                    Elements[I, J] = new Cell(I, J, gridPosition,false);
                    gridPosition.X += _cellSize;
                }
                gridPosition.Y += _cellSize;
                gridPosition.X = 0;
            }
        }

        /// <summary>
        /// Generates a path for enemies to follow
        /// </summary>
        /// <param name="a_Path">Array of points to create a path between</param>
        /// <returns></returns>
        public List<Vector2> GeneratePath()
        {
            List<Vector2> returnPath = new List<Vector2>();
            for (uint I = 0; I < Elements.GetLength(0); ++I)
            {
                for (uint J = 0; J < Elements.GetLength(1); ++J)
                {
                    if(I == J)
                    {
                        returnPath.Add(Elements[I, J].Position);
                        Elements[I, J].IsOccupied = true;
                    }
                }
            }
                return (returnPath);
        }
    }
}