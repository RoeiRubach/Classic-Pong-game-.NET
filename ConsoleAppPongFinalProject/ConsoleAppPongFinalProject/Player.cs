using System;

namespace ConsoleAppPongFinalProject
{
    class Player
    {
        public string Name { get; private set; }
        public int Score { get; private set; }
        public Point PointRef => point;

        protected Point point;
        protected Board _board;
        private static int _playersCount;

        public Player(Board board)
        {
            _playersCount++;
            if (_playersCount > 2)
                _playersCount = 1;
            HandlePlayerSetPosition();

            Score = 0;
            Name = SetPlayerName();
            _board = board;
            SetPlayerPosition();
        }

        public void MoveUp()
        {
            point.y--;
            _board.ClearBottomPaddleAfterStep(point);
        }

        public void MoveDown()
        {
            point.y++;
            _board.ClearTopPaddleAfterStep(point);
        }

        public void IncreaseScoreByOne()
        {
            Score++;

            if (Score == GameManager.GOALS_TO_REACH)
            {
                //end game
            }
        }

        private void HandlePlayerSetPosition()
        {
            point = new Point();
            point.SetSecondPlayerPosition();
            if (_playersCount == 1)
                point.SetFirstPlayerPosition();
        }

        public void SetPlayerPosition()
        {
            for (int i = point.y; i < point.y + 5; i++)
                _board.GameField[i, point.x] = CharacterUtilities.PLAYER_ICON;
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