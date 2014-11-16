using System;

namespace Chips_Challenge
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (ChipsChallengeMain game = new ChipsChallengeMain())
            {
                game.Run();
            }
        }
    }
#endif
}

