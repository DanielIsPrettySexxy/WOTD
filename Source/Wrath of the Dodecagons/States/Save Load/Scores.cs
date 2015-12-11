using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Wrath_of_the_Dodecagons.States.Save_Load
{
    /// <summary>
    /// A highscore state the uses XML files to save and load
    /// </summary>
    class Scores : IState
    {
        //A struct to more easily save, load and draw the variables
        struct PlayerProfile
        {
            public uint LastWaveConquered;
            public uint Money;
        }
        //A sorted list used to draw the highscores 
        List<PlayerProfile> _scoresTable;
        //The current players score
        PlayerProfile _currentPlayerScore;
        //A reference to the game
        Game1 _game;
        //A font to draw the scores with
        SpriteFont _font;
        //The position of the wave and money text
        Vector2 _textPosition, _cashPosition;
        //The name of the save file
        const string _saveLocation = "/HighScores.XML";
        //Bools used to record button presses on both the vita and on windows
        bool _enter, _back;
        //A timer variable to stop repated presses 
        int _tick = 5;
        //The directory of the content file
        readonly string _directory;

        /// <summary>
        /// This function sets the players current wave number and money
        /// </summary>
        /// <param name="a_wave">The wave that the player lost too</param>
        /// <param name="a_money">The money the player had when they died</param>
        public void SetStats(int a_wave, int a_money)
        {
            _currentPlayerScore = new PlayerProfile() { LastWaveConquered = (uint)a_wave, Money = (uint)a_money };
            _scoresTable.Add(_currentPlayerScore);
            Load();
            Save();
        }

        /// <summary>
        /// Constructs the scores state
        /// </summary>
        /// <param name="a_game">A reference to game</param>
        public Scores(Game1 a_game)
        {
            //Sets the proper directory for the proper version 
            #if PSM
                _directory = "Documents";
            #else
            _directory = a_game.Content.RootDirectory;
            #endif
            _currentPlayerScore = new PlayerProfile() { LastWaveConquered = 0, Money = 0};

            _game = a_game;
            _scoresTable = new List<PlayerProfile>();
            _font = a_game.Content.Load<SpriteFont>("debug_font");
            _textPosition.X = (a_game.GraphicsManager.PreferredBackBufferWidth/2) - 200;
            _textPosition.Y = 0;
            _cashPosition = _textPosition;
            _cashPosition.X += 200;
        }
        
        /// <summary>
        /// A function made to compare player profiles
        /// </summary>
        /// <param name="a_LHS">The PlayerProfile on the left</param>
        /// <param name="a_RHS">The PlayerProfile on the right</param>
        /// <returns></returns>
        private int Comparator(PlayerProfile a_LHS, PlayerProfile a_RHS)
        {
            if (a_LHS.LastWaveConquered == a_RHS.LastWaveConquered)
            {
                return 0;
            }
            if (a_LHS.LastWaveConquered > a_RHS.LastWaveConquered)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// Updates the button presses and checks if the user wants to save or exit
        /// </summary>
        public void Update()
        {
            _tick -= 1;
            #region Control Code
            #if PSM
                _enter  = _game.GamePadState.IsButtonDown(Buttons.A);
                _back   = _game.GamePadState.IsButtonDown(Buttons.B);
            #else
                _enter = _game.KeyboardState.IsKeyDown(Keys.Enter);
                _back = _game.KeyboardState.IsKeyDown(Keys.Escape);
            #endif
            #endregion
            if(_tick < 0 && _enter)
            {
                Save();
                _tick = 5;
            }
            if (_tick < 0 && _back)
            {
                _tick = 5;
                GameStateManager.PopState();
            }
            if (_tick < 0)
            {
                _tick = 5;
            }
        }

        /// <summary>
        /// Loops through and draws all the highscores
        /// </summary>
        /// <param name="a_spriteBatch">Batchs the fonts to draw</param>
        public void Draw(SpriteBatch a_spriteBatch)
        {
            a_spriteBatch.DrawString(_font, "Select Menu color", _textPosition, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            _textPosition.Y = 75;
            for (int I = 0; I < _scoresTable.Count; ++I)
            {
                _cashPosition.Y = _textPosition.Y += 75;
                a_spriteBatch.DrawString(_font, "Last Wave: " + _scoresTable[I].LastWaveConquered.ToString(), _textPosition,Color.White);
                a_spriteBatch.DrawString(_font,  "Final Cash: " + _scoresTable[I].Money.ToString(), _cashPosition, Color.White);
            }
            _textPosition.Y = 0;
        }

        /// <summary>
        /// Saves the highscore table as an XML file if the PlayerProfile is blank it will create and empty file
        /// </summary>
        private void Save()
        {
            XmlDocument documentTable = new XmlDocument();
            XmlElement rootElement = documentTable.CreateElement("HighScoreTable");
            documentTable.AppendChild(rootElement);

            if (_currentPlayerScore.LastWaveConquered > 0 && _currentPlayerScore.Money > 0)
            {
                for (int I = 0; I < _scoresTable.Count; ++I )
                {
                    XmlElement profileScore = documentTable.CreateElement("PlayerProfile");
                    //Create attributes
                    XmlAttribute waveAttrib = documentTable.CreateAttribute("WaveNum");
                    XmlAttribute cashAttrib = documentTable.CreateAttribute("Cash");

                    cashAttrib.Value = _scoresTable[I].Money.ToString();
                    waveAttrib.Value = _scoresTable[I].LastWaveConquered.ToString();

                    profileScore.Attributes.Append(cashAttrib);
                    profileScore.Attributes.Append(waveAttrib);
                    rootElement.AppendChild(profileScore);
                }
            }
            documentTable.Save(_directory + _saveLocation);
        }

        /// <summary>
        /// Checks if the file exists the loads the file(being the highscore table) if it doesn't it will call save to create one
        /// </summary>
        /// <returns></returns>
        private bool Load()
        {
            XmlDocument documentTable = new XmlDocument();
            if (File.Exists(_directory + _saveLocation))
            {
                documentTable.Load(_directory + _saveLocation);
                XmlElement rootElement = documentTable.DocumentElement;
                XmlElement profileElement = rootElement.FirstChild as XmlElement;

                while (profileElement != null)
                {
                    PlayerProfile currentProfile = new PlayerProfile();
                    currentProfile.LastWaveConquered = uint.Parse(profileElement.Attributes["WaveNum"].Value);
                    currentProfile.Money             = uint.Parse(profileElement.Attributes["Cash"].Value);

                    _scoresTable.Add(currentProfile);

                    profileElement = profileElement.NextSibling as XmlElement;
                }
                _scoresTable.Sort(Comparator);
                return true;
            }
            else
            {
                Save();
                return false;
            }
        }
    }
}
