using System;
using System.Threading;

namespace ConsoleAppPongFinalProject
{
    class GameManager
    {
        public static event Action CollisionCheck;
        public static event Action UpdateUnits;

        public const int GOALS_TO_REACH = 5;
        public static bool IsGameOver = false;
        public static UserChoice UserChoice = UserChoice.None;

        private GameUI _gameUI;
        private BoardManager _boardManager;
        private Ball _ball;
        private FirstPlayer _firstPlayer;
        private SecondPlayer secondPlayer;
        private AutoPlayer _autoPlayer;
        private ScoreDisplayHandler _scoreboard;
        private Highscore _highscore;

        private bool _isGoal = false;
        private bool _isFirstPlayer = false;
        private bool _isAI = false;
        private bool _isPlayerVSPlayer = false;

        public GameManager()
        {
            _gameUI = new GameUI();
            _boardManager = new BoardManager();
            _ball = new Ball();
            _firstPlayer = new FirstPlayer();
            _scoreboard = new ScoreDisplayHandler();
            _highscore = new Highscore();
            Ball.GoalScored += OnGoalScored;
        }

        private void OnGoalScored(int num)
        {
        }

        public void Start()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            _gameUI.SetUserChoice();
            StartChosenGameMode();
        }

        private void StartChosenGameMode()
        {
            switch (UserChoice)
            {
                case UserChoice.SinglePlayer:
                    _autoPlayer = new AutoPlayer();
                    Thread mySingleWorker = new Thread(NewThread);
                    mySingleWorker.Start();
                    break;
                //case UserChoice.PlayerVSPlayer:
                //    _isPlayerVSPlayer = true;
                //    secondPlayer = new SecondPlayer();
                //    Thread myMultiWorker = new Thread(ThreadFunctionForTheBall);
                //    myMultiWorker.Start();
                //    break;
            }
        }

        private void NewThread()
        {
            do
            {
                //CollisionCheck?.Invoke();
                UpdateUnits?.Invoke();
                _boardManager.PrintGameField();
                Thread.Sleep(1300);

            } while (!IsGameOver);
        }

        /// <summary>
        /// Thread #2 that controls the -ball- and the -auto player-.
        /// Called if the user has chosen to play as single player.
        /// Creates a do-while loop.
        /// Firstly, moves the -auto player- and the -ball- repeatedly along the borders.
        /// Secondly, checks if there was a goal (ball collided with left/right edge) - if so, checks who scored it (Puts +1 to his score) -> if the score reachs 5 the game has ended.
        ///                                                                           - if not, continue to the next if statement.         -> if not, resets the ball's and the auto-player's values.
        /// Thirdly, checks if the ball collided with any of the borders/players - if so, increments the ball's coordinates according to the collision effect.
        ///                                                                      - if not, moves the ball according to his last coordinates.
        /// Saves the last icon (so it won't be deleted). Changes the ball -X- and -Y- as needed. Sets the last icon the his previous location. Sets the ball to his new location.
        /// </summary>
        private void ThreadFunctionForTheBall_AutoPlayer()
        {
            Console.Clear();
            _isFirstPlayer = false;
            _isAI = false;
            _isGoal = false;
            char temp = CharacterUtilities.EMPTY_PIXEL;
            int ballXDiraction = 1, ballYDiraction = 0;

            do
            {
                if (_isGoal)
                {
                    //CheckWhoScored(_isFirstPlayer);
                    BoardManager.GameField[_ball.Position.Y, _ball.Position.Y] = temp;
                    temp = CharacterUtilities.EMPTY_PIXEL;

                    _ball.CreateBallInconsistently(ref ballYDiraction, ref ballXDiraction, _isPlayerVSPlayer);
                    _autoPlayer.SetsAIAtMiddle();

                    _isGoal = false;
                    Thread.Sleep(1300);

                    if (_firstPlayer.GoalCount == GOALS_TO_REACH)
                    {
                        IsGameOver = true;
                        Console.SetCursorPosition(37, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(_gameUI.PlayerOne + " wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        _highscore.HighscoreWriter(_gameUI.PlayerOne, _firstPlayer.GoalCount, _autoPlayer.GoalCount);
                        break;
                    }
                    else if (_autoPlayer.GoalCount == GOALS_TO_REACH)
                    {
                        IsGameOver = true;
                        Console.SetCursorPosition(35, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Computer wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(30, 16);
                        Console.WriteLine(_gameUI.PlayerOne + ", good luck next time.");
                        break;
                    }
                }

                if (!IsCollidedWithAnObject(ref _isGoal, temp, ref ballXDiraction, ref ballYDiraction, ref _isFirstPlayer, ref _isAI))
                {
                    _ball.BallMovementLogic(ref temp, this, ballXDiraction, ballYDiraction);
                }

            } while (!IsGameOver);
            _ball.SetBackToOrigin();
            AutoPlayerValueReset();
        }

        /// <summary>
        /// Thread #3 that controls the -ball-.
        /// Called if the user has chosen to play as player vs player.
        /// Creates a do-while loop.
        /// Firstly, moves the -ball- repeatedly along the borders.
        /// Secondly, checks if there was a goal (ball collided with left/right edge) - if so, checks who scored it (Puts +1 to his score) -> if the score reachs 5 the game has ended.
        ///                                                                           - if not, continue to the next if statement.         -> if not, resets the ball's and the second player's values.
        /// Thirdly, checks if the ball collided with any of the borders/players - if so, increments the ball's coordinates according to the collision effect.
        ///                                                                      - if not, moves the ball according to his last coordinates.
        /// Saves the last icon (so it won't be deleted). Changes the ball -X- and -Y- as needed. Sets the last icon the his previous location. Sets the ball to his new location.
        /// </summary>
        //private void ThreadFunctionForTheBall()
        //{
        //    Console.Clear();
        //    _isFirstPlayer = false;
        //    _isAI = false;
        //    _isGoal = false;
        //    char temp = CharacterUtilities.EMPTY_PIXEL;
        //    int ballXDiraction = 1, ballYDiraction = 0;

        //    do
        //    {
        //        if (_isGoal)
        //        {
        //            ChecksWhoScored(_isFirstPlayer, _isPlayerVSPlayer);
        //            BoardManager.GameField[_ball.Point.Y, _ball.Point.X] = temp;
        //            temp = CharacterUtilities.EMPTY_PIXEL;

        //            _ball.CreateBallInconsistently(ref ballYDiraction, ref ballXDiraction, _isPlayerVSPlayer);

        //            _isGoal = false;
        //            Thread.Sleep(1300);

        //            if (_firstPlayer.GoalCount == GOALS_TO_REACH)
        //            {
        //                IsGameOver = true;
        //                Console.SetCursorPosition(37, 15);
        //                Console.ForegroundColor = ConsoleColor.Blue;
        //                Console.WriteLine(_gameUI.PlayerOne + " wins!");
        //                Console.ForegroundColor = ConsoleColor.White;
        //                _highscore.HighscoreWriter(_gameUI.PlayerOne, _gameUI.PlayerTwo, _firstPlayer.GoalCount, secondPlayer.GoalCount);
        //                break;
        //            }
        //            else if (secondPlayer.GoalCount == GOALS_TO_REACH)
        //            {
        //                IsGameOver = true;
        //                Console.SetCursorPosition(35, 15);
        //                Console.ForegroundColor = ConsoleColor.Blue;
        //                Console.WriteLine(_gameUI.PlayerTwo + " wins!");
        //                Console.ForegroundColor = ConsoleColor.White;
        //                _highscore.HighscoreWriter(_gameUI.PlayerOne, _gameUI.PlayerTwo, _firstPlayer.GoalCount, secondPlayer.GoalCount);
        //                break;
        //            }
        //        }

        //        _boardManager.PrintGameField();

        //        _ball.HandleCollision();

        //        if (!IsCollidedWithAnObject(ref _isGoal, temp, ref ballXDiraction, ref ballYDiraction, ref _isFirstPlayer, ref _isAI))
        //        {
        //            _ball.BallMovementLogic(ref temp, this, ballXDiraction, ballYDiraction);
        //        }
        //    } while (!IsGameOver);
        //    _ball.SetBackToOrigin();
        //    ClearTheColumn(BoardManager.GameField);
        //}

        ////Score method for Player VS Player.
        //private void ChecksWhoScored(bool isManual, bool isMultiPlayers)
        //{
        //    if (isManual)
        //    {
        //        int location = 0;
        //        _firstPlayerGoalCount++;
        //        PrintsTheCurrentScore(_firstPlayerGoalCount, location);
        //    }
        //    else
        //    {
        //        int location = 83;
        //        SecondPlayerGoalCount++;
        //        PrintCurrentScore(SecondPlayerGoalCount, location);
        //    }
        //}

        private bool IsCollidedWithAnObject(ref bool isGoal, char temp, ref int xDiraction, ref int yDiraction, ref bool isManual, ref bool isOtherPlayer)
        {
            bool isNothing = false;

            //Checks if the ball collided with a paddle.
            if (temp == CharacterUtilities.PLAYER_ICON)
            {
                PaddleEdge collidedWithBall = PaddleEdge.None;
                WhichPartCollidedWithTheBall(ref collidedWithBall);
                switch (collidedWithBall)
                {
                    case PaddleEdge.UpperEdge:
                        yDiraction = -1;
                        break;
                    case PaddleEdge.MiddleEdge:
                        yDiraction = 0;
                        break;
                    case PaddleEdge.BottomEdge:
                        yDiraction = 1;
                        break;
                }
                xDiraction *= (-1);
            }

            //Checks if the ball collided with the top/bottom edge.
            else if (temp == CharacterUtilities.TOP_AND_BOTTOM_EDGES)
            {
                yDiraction *= (-1);
            }

            //Checks if the ball collided with the left/right edge - if so, checks which side of the edge the ball collided with -> Saves the result -> returns that a goal has occurred.
            else if (temp == CharacterUtilities.LEFT_AND_RIGHT_EDGES)
            {
                if (_ball.Position.X >= 89)
                {
                    isOtherPlayer = false;
                    isManual = true;
                }
                else
                {
                    isOtherPlayer = true;
                    isManual = false;
                }
                return isGoal = true;
            }
            return isNothing;
        }

        private void WhichPartCollidedWithTheBall(ref PaddleEdge collidedWithBall)
        {
            //Gets the last auto-player, first-player and second-player -yAxis- values.
            int autoTemp = 0, firstPlayerTemp = _firstPlayer.Point.Y, secondPlayerTemp = 0;

            if (_isPlayerVSPlayer)
                secondPlayerTemp = secondPlayer.Point.Y;
            else
                autoTemp = _autoPlayer.Point.Y;

            for (int paddlePart = 0; paddlePart < 5; paddlePart++)
            {
                //Checks which part of the paddle collided with the ball - if so, saves that part.
                //First player and second player paddles.
                if (_isPlayerVSPlayer)
                {
                    if (BoardManager.GameField[firstPlayerTemp, _firstPlayer.Point.X] == BoardManager.GameField[_ball.Position.Y, _ball.Position.X] ||
                        (BoardManager.GameField[secondPlayerTemp, secondPlayer.Point.X] == BoardManager.GameField[_ball.Position.Y, _ball.Position.X]))
                    {
                        ifCollidedWithAPaddle(ref collidedWithBall, paddlePart);
                        break;
                    }
                    secondPlayerTemp++;
                }
                else
                {
                    //Checks which part of the paddle collided with the ball - if so, saves that part.
                    //First player and computer paddles.
                    if (BoardManager.GameField[firstPlayerTemp, _firstPlayer.Point.X] == BoardManager.GameField[_ball.Position.Y, _ball.Position.X] ||
                        (BoardManager.GameField[autoTemp, _autoPlayer.Point.X] == BoardManager.GameField[_ball.Position.Y, _ball.Position.X]))
                    {
                        ifCollidedWithAPaddle(ref collidedWithBall, paddlePart);
                        break;
                    }
                    autoTemp++;
                }
                firstPlayerTemp++;
            }
        }

        private void ifCollidedWithAPaddle(ref PaddleEdge collidedWithBall, int i)
        {
            if ((i == 0) || (i == 1))
                collidedWithBall = PaddleEdge.UpperEdge;
            else if (i == 2)
                collidedWithBall = PaddleEdge.MiddleEdge;
            else
                collidedWithBall = PaddleEdge.BottomEdge;
        }

        //Sets back the last icon that the ball has deleted.
        public void SaveLastIcon(char icon)
        {
            if (icon == CharacterUtilities.PLAYER_ICON || icon == CharacterUtilities.TOP_AND_BOTTOM_EDGES)
                BoardManager.GameField[_ball.Position.Y, _ball.Position.X] = icon;
            else
                BoardManager.GameField[_ball.Position.Y, _ball.Position.X] = CharacterUtilities.EMPTY_PIXEL;
        }

        //Creates a switch statement in a Do-While loop to set the isRestarting boolean.
        public bool IsGameRestart()
        {
            GameStatus gameStatus = GameStatus.None;
            Console.CursorVisible = true;

            SwitchingToGameStatus(GetUserOption(), ref gameStatus);

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

        private int GetUserOption()
        {
            int oneOrTwo;
            do
            {
                Console.Clear();
                UserInterfaceUtilities.PrintPongTitle();
                Console.SetCursorPosition(28, 7);
                Console.Write("Will you want to restart the game?");
                Console.SetCursorPosition(26, 8);
                Console.Write("Enter -1- to restart or -2- to exit: ");
            } while (!int.TryParse(Console.ReadLine(), out oneOrTwo));
            return oneOrTwo;
        }

        private void SwitchingToGameStatus(int oneOrTwo, ref GameStatus gameStatus)
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
                    SwitchingToGameStatus(GetUserOption(), ref gameStatus);
                    break;
            }
        }

        //A random value for the ball starting position.
        public static int RandomNumer()
        {
            int horizontalOrVertical = 0;
            Random rnd = new Random();
            horizontalOrVertical = rnd.Next(0, 3);

            if (horizontalOrVertical == 0)
                horizontalOrVertical = -1;
            else if (horizontalOrVertical == 1)
                horizontalOrVertical = 0;
            else
                horizontalOrVertical = 1;

            return horizontalOrVertical;
        }

        private void AutoPlayerValueReset()
        {
            //The next 2 lines resets the auto-player's coordinates.
            ClearTheColumn(BoardManager.GameField);
        }

        private void ClearTheColumn(char[,] gameField)
        {
            for (int i = 1; i < 22; i++)
            {
                for (int j = 87; j <= 87; j++)
                {
                    gameField[i, j] = ' ';
                }
            }
        }
    }
}