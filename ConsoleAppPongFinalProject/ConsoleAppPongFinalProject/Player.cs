using System;

namespace ConsoleAppPongFinalProject
{
    class Player
    {
        public static event Action GameOver;
        public PlayerData PlayerDataRef => playerData;
        public Point PointRef => point;

        protected PlayerData playerData;
        protected Point point;
        protected Board board;
        private static int _playersCount;
        private string _computer = "Computer";

        public Player(Board board)
        {
            _playersCount++;
            if (_playersCount > 2)
                _playersCount = 1;
            HandlePlayerSetPosition();
            InitializePlayer(board);
        }

        private void InitializePlayer(Board board)
        {
            playerData.Score = 0;
            playerData.Name = SetPlayerName();
            this.board = board;
            SetPosition();
        }

        public void MoveUp()
        {
            point.y--;
            board.ClearBottomPaddleAfterStep(point);
            SetPosition();
        }

        public void MoveDown()
        {
            point.y++;
            board.ClearTopPaddleAfterStep(point);
            SetPosition();
        }

        public void IncreaseScoreByOne()
        {
            playerData.Score++;
            CheckForGameOver();
        }

        private void CheckForGameOver()
        {
            if (playerData.Score == GameManager.GOALS_TO_REACH)
            {
                UIUtilities.PrintWinner(playerData.Name);
                if (playerData.Name.Equals(_computer))
                {
                    Console.SetCursorPosition(30, 16);
                    Console.WriteLine("Good luck next time...");
                }
                GameOver?.Invoke();
            }
        }

        protected void SetPosition()
        {
            for (int i = point.y; i < point.y + 5; i++)
                board.GameField[i, point.x] = CharacterUtilities.PLAYER_ICON;
        }

        private void HandlePlayerSetPosition()
        {
            point = new Point();
            point.SetSecondPaddlePosition();
            if (_playersCount == 1)
                point.SetFirstPaddlePosition();
        }

        private string SetPlayerName()
        {
            if (IsSecondUserAndPlaySingle()) return _computer;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(3, 7);
            Console.Write($"Enter -player{_playersCount}'s- name: ");

            string playerName = Console.ReadLine();
            UIUtilities.ClearTitles();
            Console.ForegroundColor = ConsoleColor.White;
            return playerName;
        }

        private bool IsSecondUserAndPlaySingle() => _playersCount == 2 && GameManager.GameMode == GameMode.SinglePlayer;
    }
}