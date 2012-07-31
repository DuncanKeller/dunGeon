using System;
using System.IO;
using System.Windows.Forms;

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
            //try
            {
                Console.Enabled = false;
                using (Game1 game = new Game1())
                {
                    game.Run();
                }
            }
            //catch (Exception e)
            //{
            //    CrashForm crashForm = new CrashForm();
            //    crashForm.SetInfo(e.Message, e.StackTrace);
            //    Application.Run(crashForm);
            //}
        }
    }
#endif
}

