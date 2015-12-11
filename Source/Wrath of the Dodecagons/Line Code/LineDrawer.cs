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

namespace Wrath_of_the_Dodecagons.Line_Code
{
    /// <summary>
    /// This static class will allow us to draw lines and polygon object
    /// </summary>
    static class LineDraw
    {
        /// <summary>
        /// This is the sole image in the game, it's in a bitmap file to save memory. We use this sprite to draw lines
        /// </summary>
        static Texture2D _pixel;

        /// <summary>
        /// This loads and sets the pixel used to create lines
        /// </summary>
        /// <param name="a_Game">The A reference to the game to load the tecture using the graphics device</param>
        static public void Load(Game1 a_Game)
        {
            _pixel = new Texture2D(a_Game.GraphicsDevice, 1, 1);
            Color[] ColorArray = new Color[1]{Color.White};
            _pixel.SetData<Color>(ColorArray);
        }

        /// <summary>
        /// This static function will draw all our lines and batch it up
        /// </summary>
        /// <param name="a_spriteBatch">A reference to the SpriteBatch to batch up the lines</param>
        /// <param name="a_orginPoint"> The Orgin of the line</param>
        /// <param name="a_terminalPoint">Where the line will end</param>
        /// <param name="a_color">The Color of the line this will defalt to white</param>
        /// <param name="a_width">The a_width in pixels of the given line</param>
        static public void DrawLine(SpriteBatch a_spriteBatch, Vector2 a_orginPoint, Vector2 a_terminalPoint, Color a_color, int a_width = 2)
        {
            Rectangle r = new Rectangle((int)a_orginPoint.X, (int)a_orginPoint.Y, (int)(a_terminalPoint - a_orginPoint).Length() + a_width, a_width);
            Vector2 v = Vector2.Normalize(a_orginPoint - a_terminalPoint);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (a_orginPoint.Y > a_terminalPoint.Y) angle = MathHelper.TwoPi - angle;
            a_spriteBatch.Draw(_pixel, r, null, a_color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

        /// <summary>
        /// This will draw a polygon from a array of Vector2s, it will draw a closed polygon by defult
        /// </summary>
        /// <param name="a_spriteBatch">A reference to the SpriteBatch to batch up the lines</param>
        /// <param name="a_points">The positions to draw each line between</param>
        /// <param name="a_color">The Color of the lines</param>
        /// <param name="a_width">The width of the lines</param>
        /// <param name="a_closed">Whether or not the last line will conect to the first making a closed shape</param>
        static  public void DrawPoly(SpriteBatch a_spriteBatch, Vector2[] a_points, Color a_color, int a_width = 1, bool a_closed = true)
         {
             for (int i = 0; i < a_points.Length - 1; i++)
             {
                 DrawLine(a_spriteBatch, a_points[i], a_points[i + 1], a_color, a_width);
             }
            if (a_closed)
            {
                DrawLine(a_spriteBatch, a_points[a_points.Length - 1], a_points[0], a_color, a_width);
            }
         }
        
    }
}