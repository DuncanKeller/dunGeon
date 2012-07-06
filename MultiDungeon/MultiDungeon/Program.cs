using System;
using System.IO;

namespace MultiDungeon
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
            catch (Exception e)
            {
                StreamWriter sw = new StreamWriter("crashLog.txt");
                sw.WriteLine(e.Message);
                sw.WriteLine(e.StackTrace);
                sw.Close();
            }
        }
    }
#endif
}

