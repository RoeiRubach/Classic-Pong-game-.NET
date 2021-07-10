using System;
using System.IO;

namespace ConsoleAppPongFinalProject
{
    class Highscore
    {
        private readonly string _path;

        public Highscore()
        {
            _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Highscores.txt");
            GameManager.GameOver += OnGameOver;
        }

        private void OnGameOver()
        {
            GameManager.GameOver -= OnGameOver;
        }

        //First player aginst the computer writer.
        public void HighscoreWriter(string playerName, int firstPlayerGoalCount, int autoGoalCount)
        {
            string winningTime = DateTime.Now.ToString("HH:mm_dd-MM-yyyy");
            using (StreamWriter highscoreFile = new StreamWriter(_path, true))
            {
                highscoreFile.WriteLine('-' + playerName + "- has beated the Computer for: " + firstPlayerGoalCount + " to " + autoGoalCount + " - " + winningTime);
            }
        }

        //First player aginst the second player writer.
        public void HighscoreWriter(string firstPlayerName, string secondPlayerName ,int firstPlayerGoalCount, int secondPlayerGoalCount)
        {
            string winningTime = DateTime.Now.ToString("HH:mm_dd-MM-yyyy");
            using (StreamWriter highscoreFile = new StreamWriter(_path, true))
            {
                if (firstPlayerGoalCount > secondPlayerGoalCount)
                {
                    highscoreFile.WriteLine('-' + firstPlayerName + "- has beated -" + secondPlayerName + "- for: " + firstPlayerGoalCount + " to " + secondPlayerGoalCount + " - " + winningTime);
                }
                else
                {
                    highscoreFile.WriteLine('-' + secondPlayerName + "- has beated -" + firstPlayerName + "- for: " + firstPlayerGoalCount + " to " + secondPlayerGoalCount + " - " + winningTime);
                }
            }
        }

        public void HighscoreReader()
        {
            int topForSetCursor = 7, leftForSetCursor = 20;
            if (File.Exists(_path))
            {
                using (StreamReader highscoreFile = new StreamReader(_path))
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
                        File.Delete(_path);
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