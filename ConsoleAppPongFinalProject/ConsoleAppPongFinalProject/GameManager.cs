using System;
using System.Threading;

namespace ConsoleAppPongFinalProject
{
    class GameManager
    {
        public const int GOALS_TO_REACH = 5;
        public static GameMode GameMode = GameMode.None;
        public static bool IsGameOver = false;

        private Board _board;
        private Ball _ball;
        private ScoreDisplayHandler _scoreBoard;
        private Player _firstPlayer;
        private AutoPlayer _autoPlayer;
        private Player _secondPlayer;

        private bool _isGoal = false;
        private bool _isFirstPlayerScored = false;
        private char _lastPixel = CharacterUtilities.EMPTY_PIXEL;

        public void Start()
        {
            Player.GameOver += OnGameOver;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;

            InitializeInstances();
            SetGameMode();
        }

        private void InitializeInstances()
        {
            _board = new Board();
            _ball = new Ball(_board);
            _scoreBoard = new ScoreDisplayHandler();
            _ = new MainMenu();
            _firstPlayer = new Player(_board);
        }

        private void SetGameMode()
        {
            UIUtilities.ClearTitles();

            Thread mySingleWorker;
            InputHandler inputHandler = new InputHandler(_board);

            switch (GameMode)
            {
                case GameMode.SinglePlayer:
                    _autoPlayer = new AutoPlayer(_board);
                    UIUtilities.PrintPlayerInstructions(_firstPlayer.PlayerDataRef.Name);
                    mySingleWorker = new Thread(ThreadFunctionBall_AutoPlayer);
                    mySingleWorker.Start();
                    inputHandler.HandlePlayersInput(_firstPlayer, null);
                    break;
                case GameMode.PVP:
                    _secondPlayer = new Player(_board);
                    UIUtilities.PrintPlayersInsructions(_firstPlayer.PlayerDataRef.Name, _secondPlayer.PlayerDataRef.Name);
                    mySingleWorker = new Thread(ThreadFunctionBall_PVP);
                    mySingleWorker.Start();
                    inputHandler.HandlePlayersInput(_firstPlayer, _secondPlayer);
                    break;
            }
            Console.Clear();
        }

        private void OnGameOver()
        {
            Player.GameOver -= OnGameOver;
            IsGameOver = true;
            HighscoreManager highscore;

            if (GameMode == GameMode.PVP)
                highscore = new HighscoreManager(_firstPlayer, _secondPlayer);
            else
                highscore = new HighscoreManager(_firstPlayer, _autoPlayer);
        }

        private void ThreadFunctionBall_AutoPlayer()
        {
            do
            {
                _autoPlayer.HandleAIMovement();
                _board.PrintGameField();
                if (_isGoal)
                {
                    UpdateScoreLabel(_isFirstPlayerScored, _autoPlayer);
                    OnGoalScored();
                }
                else
                {
                    UpdateFrame();
                    _ball.CheckCollision(_lastPixel, ref _isFirstPlayerScored, ref _isGoal, _firstPlayer.PointRef, _autoPlayer.PointRef);
                }
            } while (!IsGameOver);
        }

        private void ThreadFunctionBall_PVP()
        {
            do
            {
                _board.PrintGameField();
                if (_isGoal)
                {
                    UpdateScoreLabel(_isFirstPlayerScored, _secondPlayer);
                    OnGoalScored();
                }
                else
                {
                    UpdateFrame();
                    _ball.CheckCollision(_lastPixel, ref _isFirstPlayerScored, ref _isGoal, _firstPlayer.PointRef, _secondPlayer.PointRef);
                }
            } while (!IsGameOver);
        }

        private void UpdateFrame()
        {
            _board.SetEmptyPixelAtPoint(_ball.PointRef);
            SetIconBackOnBoard();
            _ball.IncrementBallMovement();
            _lastPixel = _board.GameField[_ball.PointRef.Y, _ball.PointRef.X];
            _ball.SetBallPosition();
        }

        private void OnGoalScored()
        {
            _board.GameField[_ball.PointRef.Y, _ball.PointRef.X] = _lastPixel;
            _lastPixel = CharacterUtilities.EMPTY_PIXEL;
            _ball.SpawnBallRandomPosition();
            _isGoal = false;
            Thread.Sleep(1300);
        }

        private void UpdateScoreLabel(bool isFirstPlayerScored, Player player)
        {
            if (isFirstPlayerScored)
            {
                _firstPlayer.IncreaseScoreByOne();
                PrintScore(_firstPlayer.PlayerDataRef.Score, location: 0);
            }
            else
            {
                player.IncreaseScoreByOne();
                PrintScore(player.PlayerDataRef.Score, location: 83);
            }
        }

        private void SetIconBackOnBoard()
        {
            if (_lastPixel == CharacterUtilities.PLAYER_ICON)
                _board.GameField[_ball.PointRef.Y, _ball.PointRef.X] = _lastPixel;

            else if (_lastPixel == CharacterUtilities.TOP_BOTTOM_BORDER_ICON)
                _board.GameField[_ball.PointRef.Y, _ball.PointRef.X] = _lastPixel;
        }

        private void PrintScore(int currentScore, int location) => _scoreBoard.PrintScore(currentScore, location);

        public bool IsGameRestart()
        {
            GameStatus gameStatus = GameStatus.None;
            Console.CursorVisible = true;

            SwitchGameStatus(GetUserOption(), ref gameStatus);

            switch (gameStatus)
            {
                case GameStatus.Restart:
                    IsGameOver = false;
                    break;
                case GameStatus.End:
                    Console.SetCursorPosition(28, 10);
                    Console.WriteLine("Thank you for playing. Goodbye.");
                    IsGameOver = true;
                    Console.ReadKey();
                    break;
            }
            return IsGameOver;
        }

        private void SwitchGameStatus(int oneOrTwo, ref GameStatus gameStatus)
        {
            switch (oneOrTwo)
            {
                case 1:
                    gameStatus = GameStatus.Restart;
                    break;
                case 2:
                    gameStatus = GameStatus.End;
                    break;
                default:
                    SwitchGameStatus(GetUserOption(), ref gameStatus);
                    break;
            }
        }

        private int GetUserOption()
        {
            int result;
            do
            {
                Console.Clear();
                UIUtilities.PrintPongTitle();
                Console.SetCursorPosition(28, 7);
                Console.Write("Will you want to restart the game?");
                Console.SetCursorPosition(26, 8);
                Console.Write("Enter -1- to restart or -2- to exit: ");
            } while (!int.TryParse(Console.ReadLine(), out result));
            return result;
        }
    }
}