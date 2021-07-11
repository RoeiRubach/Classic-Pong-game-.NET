using System;

namespace ConsoleAppPongFinalProject
{
    public static class UserInterfaceUtilities
    {
        private const int LEFT_SET_CURSOR = 28;

        public static void PrintTitles()
        {
            PrintOnePlayer();
            PrintTwoPlayers();
            PrintHighScore();
            MainMenuInstructions();
        }

        public static void PrintPongTitle()
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.SetCursorPosition(33, 0);
            Console.Write("  ___  ___  _  _  ___ ");
            Console.SetCursorPosition(33, 1);
            Console.Write(" | _ \\/ _ \\| \\| |/ __|");
            Console.SetCursorPosition(33, 2);
            Console.Write(" |  _/ (_) | .` | (_ |");
            Console.SetCursorPosition(33, 3);
            Console.WriteLine(" |_|  \\___/|_|\\_|\\___|");

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintOnePlayer()
        {
            Console.SetCursorPosition(LEFT_SET_CURSOR, 7);
            Console.Write("  _   ___ _                   ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 8);
            Console.Write(" / | | _ \\ |__ _ _  _ ___ _ _ ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 9);
            Console.Write(" | | |  _/ / _` | || / -_) '_|");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 10);
            Console.Write(" |_| |_| |_\\__,_|\\_, \\___|_|  ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 11);
            Console.WriteLine("                 |__/         ");
        }

        public static void PrintTwoPlayers()
        {
            Console.SetCursorPosition(LEFT_SET_CURSOR, 13);
            Console.Write("  ___   ___ _                   ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 14);
            Console.Write(" |_  ) | _ \\ |__ _ _  _ ___ _ _ ___");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 15);
            Console.Write("  / /  |  _/ / _` | || / -_) '_(_-<");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 16);
            Console.Write(" /___| |_| |_\\__,_|\\_, \\___|_| /__/");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 17);
            Console.WriteLine("                   |__/         ");
        }

        public static void PrintHighScore()
        {
            Console.SetCursorPosition(LEFT_SET_CURSOR, 19);
            Console.Write("  _  _ _      _                       ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 20);
            Console.Write(" | || (_)__ _| |_  ___ __ ___ _ _ ___ ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 21);
            Console.Write(" | __ | / _` | ' \\(_-</ _/ _ \\ '_/ -_)");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 22);
            Console.Write(" |_||_|_\\__, |_||_/__/\\__\\___/_| \\___|");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 23);
            Console.WriteLine("        |___/                         ");
        }

        public static void PrintPressToStart()
        {
            Console.SetCursorPosition(0, 20);
            Console.WriteLine("Press anything to -start- and good luck!");
            Console.ReadLine();
        }

        public static void MainMenuInstructions()
        {
            Console.SetCursorPosition(0, 27);
            Console.WriteLine("Use the -UpArrow- and the -DownArrow- keys to navigate or the -Escape- key to exit.");
            Console.SetCursorPosition(0, 28);
            Console.WriteLine("Use the -Enter- key to choose.");
        }

        public static void PrintHighscoreAsTitle()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;

            Console.SetCursorPosition(LEFT_SET_CURSOR, 0);
            Console.Write("  _  _ _      _                       ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 1);
            Console.Write(" | || (_)__ _| |_  ___ __ ___ _ _ ___ ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 2);
            Console.Write(" | __ | / _` | ' \\(_-</ _/ _ \\ '_/ -_)");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 3);
            Console.Write(" |_||_|_\\__, |_||_/__/\\__\\___/_| \\___|");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 4);
            Console.WriteLine("        |___/                         ");

            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}