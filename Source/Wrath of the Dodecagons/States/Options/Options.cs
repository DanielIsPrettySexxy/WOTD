using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Wrath_of_the_Dodecagons.States;

namespace Wrath_of_the_Dodecagons.States.Options
{
    /// <summary>
    /// Options state to tweak and change the game
    /// </summary>
    class Options : IState
    {

        //Primary color values for the menu
        int[] _colorValue;
        //String to draw out the color Values
        string _displayColor;
        //An Enum with an instance to control which color the user is operating on 
        enum GUIOption : int
        {
           Red = 0, Green = 1, Blue = 2
        }
        GUIOption _colorSelecter;
        //A tick rate to move from control to control
        float _tick;
        //Variables to control the position color and font of the draw strings
        Vector2 _stringPos, _titlePos;
        SpriteFont _font;
        Color _stringColor;
        //Reference to the game
        Game1 _game;
        //Whether or not certain controls have been pressed
        bool _up, _down, _left, _right, _enter;

        /// <summary>
        /// Constructs the option menu
        /// </summary>
        ///  <param name="a_game">Reference to the game to load the sprite font</param>
        public Options(Game1 a_game)
        {
            _game = a_game;
            _font = _game.Content.Load<SpriteFont>("debug_font");
            _colorSelecter = new GUIOption();
            _colorSelecter = GUIOption.Red;

            _tick = 5;
            _colorValue = new int[3] {100,100,100};
            _stringPos.X = _game.GraphicsManager.PreferredBackBufferWidth / 2 - 85;
            _stringPos.Y = _game.GraphicsManager.PreferredBackBufferHeight / 2;
            _titlePos.Y = 50;
            _titlePos.X = (_stringPos.X - 85);
            _displayColor = "R:0 G:0 B:0";
        }

        /// <summary>
        /// Updates the current color values and will save based on user input on vita or windows
        /// </summary>
        public void Update()
        {
            #region Control Code
                #if PSM
                    _enter  = _game.GamePadState.IsButtonDown(Buttons.A);
                    _left   = _game.GamePadState.DPad.Left == ButtonState.Pressed;
                    _right  =  _game.GamePadState.DPad.Right == ButtonState.Pressed;
                    _up     =  _game.GamePadState.DPad.Up == ButtonState.Pressed;
                    _down   =  _game.GamePadState.DPad.Down == ButtonState.Pressed;
                #else
                    _enter = _game.KeyboardState.IsKeyDown(Keys.Enter);
                    _left = _game.KeyboardState.IsKeyDown(Keys.Left);
                    _right = _game.KeyboardState.IsKeyDown(Keys.Right);
                    _up = _game.KeyboardState.IsKeyDown(Keys.Up);
                    _down = _game.KeyboardState.IsKeyDown(Keys.Down);
                #endif
            #endregion

            _tick -= 1;
            _stringColor = new Color(_colorValue[0],_colorValue[1],_colorValue[2]);
            if (_tick < 0 && _enter)
            {
                Save();
                GameStateManager.PopState();
            }
            if (_tick < 0)
            {
                EnumControls(_colorSelecter);   
                _tick = 5;
            }
            _displayColor = "R:" + _colorValue[0] + " G:" + _colorValue[1] + " B:" + _colorValue[2];
        }

        /// <summary>
        ///This function automatically _Updates the current selected color value
        /// </summary>
        /// <param name="a_Color">The current color the selecter is on</param>
        private void EnumControls(GUIOption a_Color)
        {
                if (_up)
                {
                    if (_colorValue[(int)a_Color] < 255)
                    {
                        _colorValue[(int)a_Color] += 1;
                    }
                    }
                if (_down)
                {
                    if (_colorValue[(int)a_Color] > 0)
                    {
                        _colorValue[(int)a_Color] -= 1;
                    }
                }
                if (_left)
                {
                    _colorSelecter = ((int)a_Color != 0 )? (_colorSelecter - 1) : GUIOption.Blue;
                }
                if (_right)
                {
                    _colorSelecter = ((int)a_Color != 2) ? (_colorSelecter + 1) : GUIOption.Red;
                }
        }

        /// <summary>
        /// Draws the color to change the menu too and draws the menu title
        /// </summary>
        /// <param name="a_spriteBatch">A spritebatch to batch the font togther</param>
        public void Draw(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.DrawString(_font, _displayColor, _stringPos, _stringColor);
            a_spriteBatch.DrawString(_font,"Select Menu color",_titlePos,Color.White,0,Vector2.Zero,2,SpriteEffects.None,0);
        }

        /// <summary>
        ///This function saves the current color value
        /// </summary>
        private void Save()
        {

#if PSM
			string directory = "Documents";
#else
            string directory = _game.Content.RootDirectory;
#endif
            FileStream fs = File.Open(directory + "/BinaryColor", FileMode.Create);
            BinaryWriter writeFile = new BinaryWriter(fs);

            int rSave = _colorValue[0];
            int gSave = _colorValue[1];
            int bSave = _colorValue[2];


            writeFile.Write(rSave);
            writeFile.Write(gSave);
            writeFile.Write(bSave);

            fs.Dispose();
            fs.Close();
            writeFile.Close();
        }
    
    }
}
