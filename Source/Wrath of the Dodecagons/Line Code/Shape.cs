using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Wrath_of_the_Dodecagons.Line_Code;

namespace Wrath_of_the_Dodecagons
{  
    /// <summary>
    /// A shape draw with our custom line code
    /// </summary>
    class Shape
    {
        //Position of the shape
        Vector2 _position;
        //Rotation value in radians
        float _rotation;
        //Radius of the shape starting at the position
        float _radius;
        //Ammount of vertices that the shape will have to draw lines between
        int _vertCount;
        //The array the actual contains the vertices coordinates
        Vector2[] _verts;

        /// <summary>
        /// Constructs a shape with equadistant points from the center
        /// </summary>
        /// <param name="a_Position">The location of the center of the shape</param>
        /// <param name="a_Size">The radius of the shape</param>
        /// <param name="a_VertsCount">How many points the shape will have</param>
        /// <param name="a_Rotation">The rotation value of the shape</param>
        public Shape(Vector2 a_Position, float a_Size, int a_VertsCount, float a_Rotation = 0)
        {
            _radius = a_Size;
            _rotation = a_Rotation;
            _position = a_Position;
            _vertCount = a_VertsCount;
            _verts = new Vector2[_vertCount];
        }

        /// <summary>
        /// Updates each vertex to be scaled, rotated and in the proper position. 
        /// </summary>
        public void Update()
        {
            float pi2 = (float)Math.PI * 2;
            float seg = (pi2 / _vertCount);

            for (int i = 0; i < _vertCount; ++i)
            {
                float rot = _rotation + (seg * i);
                Vector2 v = new Vector2((float)Math.Cos(rot),(float)-Math.Sin(rot));
                v *= _radius;
                v += _position;
                _verts[i] = v;
            }
        }

        /// <summary>
        /// Draws the shape
        /// </summary>
        /// <param name="a_spriteBatch">A reference to sprite batch to batch up the lines</param>
        /// <param name="a_Color">What color the lines will be</param>
        /// <param name="a_Thickness">The thickness of the lines</param>
        public void DrawShape(SpriteBatch a_spriteBatch, Color a_Color, int a_Thickness = 1)
        {
            LineDraw.DrawPoly(a_spriteBatch, _verts, a_Color, a_Thickness);
        }

        //Getters & Setters
        public Vector2 Position
        {
            get { return _position; }
            set {_position = value;}
        }

        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

    }
}
