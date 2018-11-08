using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleAppPongFinalProject
{
    class Highscore
    {
        string pathString = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Highscores.txt");

        public void PrintsHighscoreAsTitle()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.SetCursorPosition(29, 0);
            Console.Write("  _  _ _      _                       ");
            Console.SetCursorPosition(29, 1);
            Console.Write(" | || (_)__ _| |_  ___ __ ___ _ _ ___ ");
            Console.SetCursorPosition(29, 2);
            Console.Write(" | __ | / _` | ' \\(_-</ _/ _ \\ '_/ -_)");
            Console.SetCursorPosition(29, 3);
            Console.Write(" |_||_|_\\__, |_||_/__/\\__\\___/_| \\___|");
            Console.SetCursorPosition(29, 4);
            Console.WriteLine("        |___/                         ");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void HighscoreWriter(string playerName, int manualGoalCount, int autoGoalCount)
        {
            string winningTime = DateTime.Now.ToString("HH:mm_dd-MM-yyyy");
            using (StreamWriter highscoreFile = new StreamWriter(pathString, true))
            {
                highscoreFile.WriteLine('-' + playerName + "- has beated the Computer for: " + manualGoalCount + " to " + autoGoalCount + " - " + winningTime);
            }
        }

        public void HighscoreReader()
        {
            int topForSetCursor = 7, leftForSetCursor = 20;
            if (File.Exists(pathString))
            {
                using (StreamReader highscoreFile = new StreamReader(pathString))
                {
                    while (true)
                    {
                        string highscoreReader = highscoreFile.ReadLine();
                        if (highscoreReader == null)
                        {
                            break;
                        }
                        Console.SetCursorPosition(leftForSetCursor, topForSetCursor);
                        Console.WriteLine(highscoreReader);
                        topForSetCursor++;
                    }
                }
                Console.SetCursorPosition(0, 26);
                Console.WriteLine("If you would like to delete your current highscores press the -Delete- key.");
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Delete:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.SetCursorPosition(0, 27);
                        Console.WriteLine("Your high scores has been deleted.");
                        Console.ForegroundColor = ConsoleColor.White;
                        File.Delete(pathString);
                        break;
                    default:
                        Console.SetCursorPosition(0, 27);
                        Console.WriteLine("Your high scores has been saved.");
                        break;
                }
            }
            else
            {
                Console.SetCursorPosition(leftForSetCursor, topForSetCursor);
                Console.WriteLine("Sad.. Your highscore is currently empty..");
            }
            Console.SetCursorPosition(0, 28);
            Console.WriteLine("Press any key to get back to the -Main Menu-");
        }
    }
}
