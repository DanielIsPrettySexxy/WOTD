using System;
using System.Collections.Generic;
using System.Linq;


namespace Wrath_of_the_Dodecagons
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var game = new Game1())
                game.Run();
        }
    }

}
