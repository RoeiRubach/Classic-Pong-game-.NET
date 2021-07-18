using System;
using System.IO;

namespace ConsoleAppPongFinalProject
{
    class HighscoreManager
    {
        private Player _firstPlayer;
        private Player _secondPlayer;
        private PlayerData _winner;
        private PlayerData _loser;

        private readonly string _path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Highscores.txt");

        public HighscoreManager()
        {

        }

        public HighscoreManager(Player firstPlayer, Player secondPlayer)
        {
            _firstPlayer = firstPlayer;
            _secondPlayer = secondPlayer;
            SetWinnerByScore();

            if(!_winner.Name.Equals("Computer"))
                HighscoreWriter();
        }

        private void SetWinnerByScore()
        {
            if(_firstPlayer.PlayerDataRef.Score > _secondPlayer.PlayerDataRef.Score)
            {
                _winner.Name = _firstPlayer.PlayerDataRef.Name;
                _winner.Score = _firstPlayer.PlayerDataRef.Score;
                _loser.Name = _secondPlayer.PlayerDataRef.Name;
                _loser.Score = _secondPlayer.PlayerDataRef.Score;
            }
            else
            {
                _winner.Name = _secondPlayer.PlayerDataRef.Name;
                _winner.Score = _secondPlayer.PlayerDataRef.Score;
                _loser.Name = _firstPlayer.PlayerDataRef.Name;
                _loser.Score = _firstPlayer.PlayerDataRef.Score;
            }
        }

        private void HighscoreWriter()
        {
            string winningTime = DateTime.Now.ToString("HH:mm_dd-MM-yyyy");
            using (StreamWriter highscoreFile = new StreamWriter(_path, true))
            {
                highscoreFile.WriteLine($"-{_winner.Name}- has beated -{_loser.Name}- || {_winner.Score} to {_loser.Score} || {winningTime}");
            }
        }

        public void HighscoreReader()
        {
            if (File.Exists(_path))
            {
                PrintHighscores();

                Console.SetCursorPosition(left: 0, top: 26);
                Console.WriteLine("If you would like to delete your current highscores press the -Delete- key.");

                ConsoleKeyInfo key = Console.ReadKey(true);
                Console.SetCursorPosition(left: 0, top: 27);
                switch (key.Key)
                {
                    case ConsoleKey.Delete:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Your high scores has been deleted.");
                        Console.ForegroundColor = ConsoleColor.White;
                        File.Delete(_path);
                        break;
                    default:
                        Console.WriteLine("Your high scores has been saved.");
                        break;
                }
            }
            else
            {
                Console.SetCursorPosition(left: 20, top: 7);
                Console.WriteLine("Sad.. Your highscore is currently empty..");
            }
            Console.SetCursorPosition(left: 0, top:28);
            Console.WriteLine("Press any key to get back to the -Main Menu-");
        }

        private void PrintHighscores()
        {
            int topForSetCursor = 7, leftForSetCursor = 20;
            using (StreamReader highscoreFile = new StreamReader(_path))
            {
                while (true)
                {
                    string highscoreReader = highscoreFile.ReadLine();
                    if (highscoreReader == null)
                        break;

                    Console.SetCursorPosition(leftForSetCursor, topForSetCursor);
                    Console.WriteLine(highscoreReader);
                    topForSetCursor++;
                }
            }
        }
    }
}