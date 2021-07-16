using System;

namespace ConsoleAppPongFinalProject
{
    class Player
    {
        public string Name { get; private set; }
        public int Score { get; private set; }
        public int XAxis { get; set; }
        public int YAxis { get; set; }

        protected Board _board;
        private static int _playersCount;

        public Player(int x, int y, Board board)
        {
            _playersCount++;
            if (_playersCount > 2)
                _playersCount = 1;

            Score = 0;
            XAxis = x;
            YAxis = y;
            Name = SetPlayerName();
            _board = board;
            SetPlayerPosition();
        }

        public void IncreaseScoreByOne()
        {
            Score++;

            if (Score == GameManager.GOALS_TO_REACH)
            {
                //end game
            }
        }

        public void SetPlayerPosition()
        {
            for (int i = YAxis; i < YAxis + 5; i++)
                _board.GameField[i, XAxis] = CharacterUtilities.PLAYER_ICON;
        }

        private string SetPlayerName()
        {
            if (IsSecondUserAndPlaySingle()) return "Computer";

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(3, 7);
            Console.Write($"Enter -player{_playersCount}'s- name: ");

            string playerName = Console.ReadLine();
            UIUtilities.ClearTitles();
            Console.ForegroundColor = ConsoleColor.White;
            return playerName;
        }

        private bool IsSecondUserAndPlaySingle() => _playersCount == 2 && GameManager.UserChoice == UserChoice.SinglePlayer;
    }
}