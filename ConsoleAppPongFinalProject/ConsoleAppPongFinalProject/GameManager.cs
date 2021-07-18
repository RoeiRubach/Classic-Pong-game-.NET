using System;
using System.Threading;

namespace ConsoleAppPongFinalProject
{
    class GameManager
    {
        public const int GOALS_TO_REACH = 5;

        public static UserChoice UserChoice = UserChoice.None;

        private bool _isGoal = false;
        private bool _isGameOver = false;
        private bool _isFirstPlayerScored = false;

        private Board _board;
        private Ball _ball;
        private Scoreboard _scoreboard;
        private Player _firstPlayer;
        private AutoPlayer _autoPlayer;
        private Player _secondPlayer;

        public GameManager()
        {
            _board = new Board();
            _ball = new Ball(_board);
        }

        public void Start()
        {
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

            switch (UserChoice)
            {
                case UserChoice.SinglePlayer:
                    _autoPlayer = new AutoPlayer(_board);
                    UIUtilities.PrintPlayerInstructions(_firstPlayer.Name);
                    mySingleWorker = new Thread(ThreadFunctionForTheBall_AutoPlayer);
                    mySingleWorker.Start();
                    HandlePlayerInput();
                    break;
                case UserChoice.PVP:
                    _secondPlayer = new Player(_board);
                    UIUtilities.PrintPlayersInsructions(_firstPlayer.Name, _secondPlayer.Name);
                    mySingleWorker = new Thread(ThreadFunctionForTheBall_PVP);
                    mySingleWorker.Start();
                    HandlePlayersInput();
                    break;
            }
        }

        private void HandlePlayerInput()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (!_board.IsPaddleReachTopBorder(_firstPlayer.PointRef))
                            _firstPlayer.MoveUp();
                        break;

                    case ConsoleKey.DownArrow:
                        if (!_board.IsPaddleReachBottomBorder(_firstPlayer.PointRef))
                            _firstPlayer.MoveDown();
                        break;
                }
                _firstPlayer.SetPlayerPosition();
            } while (!_isGameOver);
        }

        private void HandlePlayersInput()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (!_board.IsPaddleReachTopBorder(_firstPlayer.PointRef))
                            _firstPlayer.MoveUp();
                        break;

                    case ConsoleKey.DownArrow:
                        if (!_board.IsPaddleReachBottomBorder(_firstPlayer.PointRef))
                            _firstPlayer.MoveDown();
                        break;

                    case ConsoleKey.W:
                        if (!_board.IsPaddleReachTopBorder(_secondPlayer.PointRef))
                            _secondPlayer.MoveUp();

                        break;
                    case ConsoleKey.S:
                        if (!_board.IsPaddleReachBottomBorder(_secondPlayer.PointRef))
                            _secondPlayer.MoveDown();
                        break;
                }
                _firstPlayer.SetPlayerPosition();
                _secondPlayer.SetPlayerPosition();

            } while (!_isGameOver);
        }

        private void ThreadFunctionForTheBall_AutoPlayer()
        {
            bool isReachTop = false;
            Console.Clear();
            char pixel = CharacterUtilities.EMPTY_PIXEL;
            _scoreboard = new Scoreboard();

            do
            {
                _autoPlayer.HandleAIMovement(ref isReachTop);
                _board.PrintGameField();
                _ball.IsCollidedWithAnObject(pixel, ref _isFirstPlayerScored, ref _isGoal, _firstPlayer.PointRef.y, _autoPlayer.PointRef.y);

                if (_isGoal)
                {
                    UpdateScoreLabel(_isFirstPlayerScored);
                    OnGoalScored(ref pixel);

                    if (IsReachMaxGoals(_firstPlayer.Score))
                    {
                        _isGameOver = true;
                        UIUtilities.PrintWinner(_firstPlayer.Name);
                        Highscore highscore = new Highscore();
                        highscore.HighscoreWriter(_firstPlayer.Name, _firstPlayer.Score, _autoPlayer.Score);
                        break;
                    }
                    else if (IsReachMaxGoals(_autoPlayer.Score))
                    {
                        _isGameOver = true;
                        UIUtilities.PrintWinner("Computer");
                        Console.SetCursorPosition(30, 16);
                        Console.WriteLine(_firstPlayer.Name + ", good luck next time.");
                        break;
                    }
                }

                SetIconBackOnBoard(pixel);
                _board.SetEmptyPixelAtPoint(_ball.PointRef);
                _ball.IncrementBallMovement();
                pixel = _board.GameField[_ball.PointRef.y, _ball.PointRef.x];
                _ball.SetBallPosition();

            } while (!_isGameOver);

            _board.SetEmptyPixelAtPoint(_ball.PointRef);
            _ball.ResetBallValue();
            isReachTop = false;
        }

        private void ThreadFunctionForTheBall_PVP()
        {
            Console.Clear();
            char pixel = CharacterUtilities.EMPTY_PIXEL;
            _scoreboard = new Scoreboard();

            do
            {
                _board.PrintGameField();
                _ball.IsCollidedWithAnObject(pixel, ref _isFirstPlayerScored, ref _isGoal, _firstPlayer.PointRef.y, _secondPlayer.PointRef.y);

                if (_isGoal)
                {
                    UpdateScoreLabel(_isFirstPlayerScored);
                    OnGoalScored(ref pixel);

                    if (IsReachMaxGoals(_firstPlayer.Score))
                    {
                        _isGameOver = true;
                        UIUtilities.PrintWinner(_firstPlayer.Name);
                        Highscore highscore = new Highscore();
                        highscore.HighscoreWriter(_firstPlayer.Name, _secondPlayer.Name, _firstPlayer.Score, _secondPlayer.Score);
                        break;
                    }
                    else if (IsReachMaxGoals(_secondPlayer.Score))
                    {
                        _isGameOver = true;
                        UIUtilities.PrintWinner(_secondPlayer.Name);
                        Highscore highscore = new Highscore();
                        highscore.HighscoreWriter(_firstPlayer.Name, _secondPlayer.Name, _firstPlayer.Score, _secondPlayer.Score);
                        break;
                    }
                }

                _board.SetEmptyPixelAtPoint(_ball.PointRef);
                SetIconBackOnBoard(pixel);
                _ball.IncrementBallMovement();
                pixel = _board.GameField[_ball.PointRef.y, _ball.PointRef.x];
                _ball.SetBallPosition();

            } while (!_isGameOver);

            _ball.ResetBallValue();
        }

        private void OnGoalScored(ref char pixel)
        {
            _board.GameField[_ball.PointRef.y, _ball.PointRef.x] = pixel;
            pixel = CharacterUtilities.EMPTY_PIXEL;
            _ball.SetBallInconsistently();
            _isGoal = false;
            Thread.Sleep(1300);
        }

        private bool IsReachMaxGoals(int currentGoals) => currentGoals == GOALS_TO_REACH;

        private void UpdateScoreLabel(bool isFirstPlayerScored)
        {
            if (isFirstPlayerScored)
            {
                _firstPlayer.IncreaseScoreByOne();
                PrintScore(_firstPlayer.Score, location: 0);
            }
            else if (UserChoice == UserChoice.PVP)
            {
                _secondPlayer.IncreaseScoreByOne();
                PrintScore(_secondPlayer.Score, location: 83);
            }
            else
            {
                _autoPlayer.IncreaseScoreByOne();
                PrintScore(_autoPlayer.Score, location: 83);
            }
        }

        private void SetIconBackOnBoard(char temp)
        {
            if (temp == CharacterUtilities.PLAYER_ICON)
                _board.GameField[_ball.PointRef.y, _ball.PointRef.x] = temp;

            else if (temp == CharacterUtilities.TOP_BOTTOM_BORDER_ICON)
                _board.GameField[_ball.PointRef.y, _ball.PointRef.x] = temp;
        }

        private void PrintScore(int currentScore, int location) => _scoreboard.PrintScore(currentScore, location);

        //Creates a switch statement in a Do-While loop to set the isRestarting boolean.
        public bool IsGameRestart()
        {
            GameStatus gameStatus = GameStatus.None;
            Console.CursorVisible = true;

            SwitchGameStatus(GetUserOption(), ref gameStatus);

            switch (gameStatus)
            {
                case GameStatus.Restart:
                    _isGameOver = false;
                    break;
                case GameStatus.End:
                    Console.SetCursorPosition(28, 10);
                    Console.WriteLine("Thank you for playing. Goodbye.");
                    _isGameOver = true;
                    Console.ReadKey();
                    break;
            }
            return _isGameOver;
        }

        private int GetUserOption()
        {
            int oneOrTwo;
            do
            {
                Console.Clear();
                UIUtilities.PrintPongTitle();
                Console.SetCursorPosition(28, 7);
                Console.Write("Will you want to restart the game?");
                Console.SetCursorPosition(26, 8);
                Console.Write("Enter -1- to restart or -2- to exit: ");
            } while (!int.TryParse(Console.ReadLine(), out oneOrTwo));
            return oneOrTwo;
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
    }
}