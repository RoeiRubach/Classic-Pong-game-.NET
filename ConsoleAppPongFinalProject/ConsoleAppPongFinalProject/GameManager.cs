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
        private Scoreboard _scoreBoard;
        private Player _firstPlayer;
        private AutoPlayer _autoPlayer;
        private Player _secondPlayer;

        private bool _isGoal = false;
        private bool _isFirstPlayerScored = false;

        public GameManager()
        {
            _board = new Board();
            _ball = new Ball(_board);
        }

        public void Start()
        {
            Player.GameOver += OnGameOver;
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;

            SetGameMode();
        }

        private void SetGameMode()
        {
            MainMenu mainMenu = new MainMenu();
            UIUtilities.ClearTitles();

            _firstPlayer = new Player(_board);
            Thread mySingleWorker;
            InputHandler inputHandler;

            switch (GameMode)
            {
                case GameMode.SinglePlayer:
                    _autoPlayer = new AutoPlayer(_board);
                    UIUtilities.PrintPlayerInstructions(_firstPlayer.PlayerDataRef.Name);
                    mySingleWorker = new Thread(ThreadFunctionBall_AutoPlayer);
                    mySingleWorker.Start();
                    inputHandler = new InputHandler(_board);
                    inputHandler.HandlePlayersInput(_firstPlayer, null);
                    break;
                case GameMode.PVP:
                    _secondPlayer = new Player(_board);
                    UIUtilities.PrintPlayersInsructions(_firstPlayer.PlayerDataRef.Name, _secondPlayer.PlayerDataRef.Name);
                    mySingleWorker = new Thread(ThreadFunctionBall_PVP);
                    mySingleWorker.Start();
                    inputHandler = new InputHandler(_board);
                    inputHandler.HandlePlayersInput(_firstPlayer, _secondPlayer);
                    break;
            }
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
            bool isReachTop = false;
            Console.Clear();
            char pixel = CharacterUtilities.EMPTY_PIXEL;
            _scoreBoard = new Scoreboard();

            do
            {
                _autoPlayer.HandleAIMovement(ref isReachTop);
                _board.PrintGameField();

                if (_isGoal)
                {
                    UpdateScoreLabel(_isFirstPlayerScored, _autoPlayer);
                    OnGoalScored(ref pixel);
                }
                UpdateFrame(ref pixel);
                _ball.IsCollidedWithAnObject(pixel, ref _isFirstPlayerScored, ref _isGoal, _firstPlayer.PointRef.y, _autoPlayer.PointRef.y);

            } while (!IsGameOver);
        }

        private void ThreadFunctionBall_PVP()
        {
            Console.Clear();
            char pixel = CharacterUtilities.EMPTY_PIXEL;
            _scoreBoard = new Scoreboard();

            do
            {
                _board.PrintGameField();

                if (_isGoal)
                {
                    UpdateScoreLabel(_isFirstPlayerScored, _secondPlayer);
                    OnGoalScored(ref pixel);
                }
                UpdateFrame(ref pixel);
                _ball.IsCollidedWithAnObject(pixel, ref _isFirstPlayerScored, ref _isGoal, _firstPlayer.PointRef.y, _secondPlayer.PointRef.y);

            } while (!IsGameOver);
        }

        private void UpdateFrame(ref char pixel)
        {
            _board.SetEmptyPixelAtPoint(_ball.PointRef);
            SetIconBackOnBoard(pixel);
            _ball.IncrementBallMovement();
            pixel = _board.GameField[_ball.PointRef.y, _ball.PointRef.x];
            _ball.SetBallPosition();
        }

        private void OnGoalScored(ref char pixel)
        {
            _board.GameField[_ball.PointRef.y, _ball.PointRef.x] = pixel;
            pixel = CharacterUtilities.EMPTY_PIXEL;
            _ball.SetBallInconsistently();
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

        private void SetIconBackOnBoard(char temp)
        {
            if (temp == CharacterUtilities.PLAYER_ICON)
                _board.GameField[_ball.PointRef.y, _ball.PointRef.x] = temp;

            else if (temp == CharacterUtilities.TOP_BOTTOM_BORDER_ICON)
                _board.GameField[_ball.PointRef.y, _ball.PointRef.x] = temp;
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