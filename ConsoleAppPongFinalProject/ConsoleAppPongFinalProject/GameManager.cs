using System;
using System.Threading;

namespace ConsoleAppPongFinalProject
{
    class GameManager
    {
        public const int GOALS_TO_REACH = 5;
        private const char EMPTY_PIXEL = ' ';
        private bool _isGoal = false;
        private bool _isGameOver = false;
        private bool _isFirstPlayer = false;
        private bool _isOtherPlayer = false;
        private bool _isPlayerVSPlayer = false;
        private int _firstPlayerGoalCount, _AIGoalCount, _secondPlayerGoalCount;

        //The next 7 lines initiates the objects.
        private UserInterface _userInterface;
        private ScoreDisplayHandler _scoreboard = new ScoreDisplayHandler();
        private BoardManager _boardManager;
        private Ball _ball;
        private FirstPlayer _firstPlayer = new FirstPlayer(2, ((BoardManager.FIELD_HIGHT / 2) - 2));
        private AutoPlayer _autoPlayer;
        private SecondPlayer _secondPlayer;
        private Highscore _highscore = new Highscore();

        public GameManager()
        {
            _userInterface = new UserInterface();
            _boardManager = new BoardManager();
            _ball = new Ball((BoardManager.FIELD_WIDTH / 2), (BoardManager.FIELD_HIGHT / 2));
            _ball.SetBallPosition(BoardManager.GameField);
            _firstPlayer.SetFirstPlayerPosition(_boardManager);
        }

        //Controls the game logic and runs it.
        public void Start()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            _firstPlayerGoalCount = 0;

            GetUserChoice();
        }

        private void GetUserChoice()
        {
            UserChoice userChoice = UserChoice.None;
            _userInterface.MainMenu(ref userChoice);

            switch (userChoice)
            {
                case UserChoice.SinglePlayer:
                    _AIGoalCount = 0;
                    _autoPlayer = new AutoPlayer((FIELD_WIDTH - 3), ((FIELD_HIGHT / 2) - 8));
                    _autoPlayer.SetAutoPlayerPosition(_boardManager);
                    Thread mySingleWorker = new Thread(ThreadFunctionForTheBall_AutoPlayer);
                    mySingleWorker.Start();
                    StartSinglePlayerLoop();
                    break;
                case UserChoice.PlayerVSPlayer:
                    _isPlayerVSPlayer = true;
                    _secondPlayerGoalCount = 0;
                    _secondPlayer = new SecondPlayer((FIELD_WIDTH - 3), ((FIELD_HIGHT / 2) - 2));
                    _secondPlayer.SetSecondPlayerPosition(_boardManager);
                    Thread myMultiWorker = new Thread(ThreadFunctionForTheBall);
                    myMultiWorker.Start();
                    StartPlayerVSPlayerLoop();
                    break;
            }
        }

        private void StartSinglePlayerLoop()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        //Checks if the FirstPlayer gets out the border.
                        if (_boardManager[_firstPlayer.YAxis - 1, _firstPlayer.XAxis] != CharacterUtilities.TOP_AND_BOTTOM_EDGES)
                        {
                            _firstPlayer.YAxis--;
                            _boardManager[_firstPlayer.YAxis + 5, _firstPlayer.XAxis] = ' ';
                        }
                        _firstPlayer.SetFirstPlayerPosition(_boardManager);
                        break;
                    case ConsoleKey.DownArrow:
                        //Checks if the FirstPlayer gets out the border.
                        if (_boardManager[_firstPlayer.YAxis + 5, _firstPlayer.XAxis] != CharacterUtilities.TOP_AND_BOTTOM_EDGES)
                        {
                            _firstPlayer.YAxis++;
                            _boardManager[_firstPlayer.YAxis - 1, _firstPlayer.XAxis] = ' ';
                        }
                        _firstPlayer.SetFirstPlayerPosition(_boardManager);
                        break;
                    default:
                        //Checks if the game has ended.
                        if ((_firstPlayerGoalCount == GOALS_TO_REACH) || (_AIGoalCount == GOALS_TO_REACH) || (_secondPlayerGoalCount == GOALS_TO_REACH))
                        {
                            Console.SetCursorPosition(0, 28);
                            Console.WriteLine("Press any key to continue..");
                            Console.ReadKey(true);
                        }
                        break;
                }

            } while (!_isGameOver);
        }

        private void StartPlayerVSPlayerLoop()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        //Checks if the FirstPlayer gets out the border.
                        if (_boardManager[_firstPlayer.YAxis - 1, _firstPlayer.XAxis] != CharacterUtilities.TOP_AND_BOTTOM_EDGES)
                        {
                            _firstPlayer.YAxis--;
                            _boardManager[_firstPlayer.YAxis + 5, _firstPlayer.XAxis] = ' ';
                        }
                        _firstPlayer.SetFirstPlayerPosition(_boardManager);
                        break;
                    case ConsoleKey.DownArrow:
                        //Checks if the FirstPlayer gets out the border.
                        if (_boardManager[_firstPlayer.YAxis + 5, _firstPlayer.XAxis] != CharacterUtilities.TOP_AND_BOTTOM_EDGES)
                        {
                            _firstPlayer.YAxis++;
                            _boardManager[_firstPlayer.YAxis - 1, _firstPlayer.XAxis] = ' ';
                        }
                        _firstPlayer.SetFirstPlayerPosition(_boardManager);
                        break;
                    case ConsoleKey.W:
                        //Checks if the SecondPlayer gets out the border.
                        if (_boardManager[_secondPlayer.YAxis - 1, _secondPlayer.XAxis] != CharacterUtilities.TOP_AND_BOTTOM_EDGES)
                        {
                            _secondPlayer.YAxis--;
                            _boardManager[_secondPlayer.YAxis + 5, _secondPlayer.XAxis] = ' ';
                        }
                        _secondPlayer.SetSecondPlayerPosition(_boardManager);
                        break;
                    case ConsoleKey.S:
                        //Checks if the SecondPlayer gets out the border.
                        if (_boardManager[_secondPlayer.YAxis + 5, _secondPlayer.XAxis] != CharacterUtilities.TOP_AND_BOTTOM_EDGES)
                        {
                            _secondPlayer.YAxis++;
                            _boardManager[_secondPlayer.YAxis - 1, _secondPlayer.XAxis] = ' ';
                        }
                        _secondPlayer.SetSecondPlayerPosition(_boardManager);
                        break;
                    default:
                        //Checks if the game has ended.
                        if ((_firstPlayerGoalCount == GOALS_TO_REACH) || (_AIGoalCount == GOALS_TO_REACH) || (_secondPlayerGoalCount == GOALS_TO_REACH))
                        {
                            Console.SetCursorPosition(0, 28);
                            Console.WriteLine("Press any key to continue..");
                            Console.ReadKey(true);
                        }
                        break;
                }

            } while (!_isGameOver);
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
            bool isReachTop = false;
            _isFirstPlayer = false;
            _isOtherPlayer = false;
            _isGoal = false;
            char temp = EMPTY_PIXEL;
            int ballXDiraction = 1, ballYDiraction = 0;
            //First score for the manual player.
            _scoreboard.PrintCurrentScore(0, 0);
            //First score for the auto player.
            _scoreboard.PrintCurrentScore(0, 83);

            do
            {
                if (_isGoal)
                {
                    ChecksWhoScored(_isFirstPlayer);
                    _boardManager[_ball.YAxis, _ball.XAxis] = temp;
                    temp = EMPTY_PIXEL;

                    CreatesBallInconsistently(ref ballYDiraction, ref ballXDiraction, _isPlayerVSPlayer);
                    SetsAutoPlayerAtMid();

                    _isGoal = false;
                    Thread.Sleep(1300);

                    if (_firstPlayerGoalCount == GOALS_TO_REACH)
                    {
                        _isGameOver = true;
                        Console.SetCursorPosition(37, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(_userInterface.PlayerOne + " wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        _highscore.HighscoreWriter(_userInterface.PlayerOne, _firstPlayerGoalCount, _AIGoalCount);
                        break;
                    }
                    else if (_AIGoalCount == GOALS_TO_REACH)
                    {
                        _isGameOver = true;
                        Console.SetCursorPosition(35, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Computer wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(30, 16);
                        Console.WriteLine(_userInterface.PlayerOne + ", good luck next time.");
                        break;
                    }
                }
                AutoPlayerMovementLogic(ref isReachTop);
                PrintGameField();
                if (!IsCollidedWithAnObject(ref _isGoal, temp, ref ballXDiraction, ref ballYDiraction, ref _isFirstPlayer, ref _isOtherPlayer))
                {
                    BallMovementLogic(ref temp, ref ballXDiraction, ref ballYDiraction);
                }
            } while (!_isGameOver);
            BallValueReset();
            AutoPlayerValueReset(ref isReachTop);
        }

        private void PrintGameField()
        {
            _userInterface.PrintPongTitle();
            for (int i = 0; i < _boardManager.GetLength(0); i++)
            {
                for (int j = 0; j < _boardManager.GetLength(1); j++)
                {
                    Console.Write(_boardManager[i, j]);
                }
                Console.WriteLine();
            }
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
        private void ThreadFunctionForTheBall()
        {
            Console.Clear();
            _isFirstPlayer = false;
            _isOtherPlayer = false;
            _isGoal = false;
            char temp = EMPTY_PIXEL;
            int ballXDiraction = 1, ballYDiraction = 0;
            //First score for the first player.
            _scoreboard.PrintCurrentScore(0, 0);
            //First score for the second player.
            _scoreboard.PrintCurrentScore(0, 83);

            do
            {
                if (_isGoal)
                {
                    ChecksWhoScored(_isFirstPlayer , _isPlayerVSPlayer);
                    _boardManager[_ball.YAxis, _ball.XAxis] = temp;
                    temp = EMPTY_PIXEL;

                    CreatesBallInconsistently(ref ballYDiraction, ref ballXDiraction, _isPlayerVSPlayer);

                    _isGoal = false;
                    Thread.Sleep(1300);

                    if (_firstPlayerGoalCount == GOALS_TO_REACH)
                    {
                        _isGameOver = true;
                        Console.SetCursorPosition(37, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(_userInterface.PlayerOne + " wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        _highscore.HighscoreWriter(_userInterface.PlayerOne, _userInterface.PlayerTwo ,_firstPlayerGoalCount, _secondPlayerGoalCount);
                        break;
                    }
                    else if (_secondPlayerGoalCount == GOALS_TO_REACH)
                    {
                        _isGameOver = true;
                        Console.SetCursorPosition(35, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(_userInterface.PlayerTwo + " wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        _highscore.HighscoreWriter(_userInterface.PlayerOne ,_userInterface.PlayerTwo, _firstPlayerGoalCount, _secondPlayerGoalCount);
                        break;
                    }
                }
                PrintGameField();
                if (!IsCollidedWithAnObject(ref _isGoal, temp, ref ballXDiraction, ref ballYDiraction, ref _isFirstPlayer, ref _isOtherPlayer))
                {
                    BallMovementLogic(ref temp, ref ballXDiraction, ref ballYDiraction);
                }
            } while (!_isGameOver);
            BallValueReset();
            _secondPlayer.ClearTheColumn(_boardManager);
        }

        private void CreatesBallInconsistently(ref int ballYDiraction, ref int ballXDiraction, bool isTwoPlayers)
        {
            //Spawns the ball at Inconsistently coordinates.
            _ball.YAxis = (RandomNumer() + RandomNumer() + RandomNumer() + RandomNumer() + (FIELD_HIGHT / 2));
            _ball.XAxis = (2 + RandomNumer() + (FIELD_WIDTH / 2));
            ballYDiraction = RandomNumer();
            if (isTwoPlayers)
            {
                ballXDiraction *= (-1);
            }
            else
            {
                ballXDiraction = -1;
            }
            _ball.SetBallPosition(_boardManager);
        }

        private void SetsAutoPlayerAtMid()
        {
            //Sets the auto-player's coordinates at the middle field.
            _autoPlayer.ClearTheColumn(_boardManager);
            _autoPlayer.YAxis = ((FIELD_HIGHT / 2) - 2);
            _autoPlayer.XAxis = (FIELD_WIDTH - 3);
            _autoPlayer.SetAutoPlayerPosition(_boardManager);
        }

        //Score method for the first player aginst the computer.
        private void ChecksWhoScored(bool isManual)
        {
            if (isManual)
            {
                int location = 0;
                _firstPlayerGoalCount++;
                PrintsTheCurrentScore(_firstPlayerGoalCount, location);
            }
            else
            {
                int location = 83;
                _AIGoalCount++;
                PrintsTheCurrentScore(_AIGoalCount, location);
            }
        }

        //Score method for the first player aginst the second player.
        private void ChecksWhoScored(bool isManual, bool isMultiPlayers)
        {
            if (isManual)
            {
                int location = 0;
                _firstPlayerGoalCount++;
                PrintsTheCurrentScore(_firstPlayerGoalCount, location);
            }
            else
            {
                int location = 83;
                _secondPlayerGoalCount++;
                PrintsTheCurrentScore(_secondPlayerGoalCount, location);
            }
        }

        private void BallMovementLogic(ref char temp, ref int xDiraction, ref int yDiraction)
        {
            //Saves the last icon (so it won't be deleted).
            KeepsTheWantedIcon(temp);

            //Changes the ball -X- and -Y- as needed.
            _ball.XAxis += xDiraction;
            _ball.YAxis += yDiraction;

            //Sets the last icon the his previous location.
            temp = _boardManager[_ball.YAxis, _ball.XAxis];

            //Sets the ball to his new location.
            _ball.SetBallPosition(_boardManager);
        }

        private void AutoPlayerMovementLogic(ref bool isReachTop)
        {
            //Checks if the paddle reached the upper edge.
            if ((_boardManager[_autoPlayer.YAxis - 1, _autoPlayer.XAxis] != CharacterUtilities.TOP_AND_BOTTOM_EDGES) && (!isReachTop))
            {
                _autoPlayer.YAxis--;
                _boardManager[_autoPlayer.YAxis + 5, _autoPlayer.XAxis] = ' ';
            }
            //Checks if the paddle reached the bottom edge.
            else if (_boardManager[_autoPlayer.YAxis + 5, _autoPlayer.XAxis] != CharacterUtilities.TOP_AND_BOTTOM_EDGES)
            {
                isReachTop = true;
                _autoPlayer.YAxis++;
                _boardManager[_autoPlayer.YAxis - 1, _autoPlayer.XAxis] = ' ';
            }
            else
            {
                isReachTop = false;
            }
            //Sets the paddle to his new location.
            _autoPlayer.SetAutoPlayerPosition(_boardManager);
        }

        private bool IsCollidedWithAnObject(ref bool isGoal, char temp, ref int xDiraction, ref int yDiraction, ref bool isManual, ref bool isOtherPlayer)
        {
            bool isNothing = false;

            //Checks if the ball collided with a paddle.
            if (temp == CharacterUtilities.PLAYER_ICON)
            {
                CollidedWithBall collidedWithBall = CollidedWithBall.None;
                WhichPartCollidedWithTheBall(ref collidedWithBall);
                switch (collidedWithBall)
                {
                    case CollidedWithBall.UpperEdge:
                        yDiraction = -1;
                        break;
                    case CollidedWithBall.MiddleEdge:
                        yDiraction = 0;
                        break;
                    case CollidedWithBall.BottomEdge:
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
                if (_ball.XAxis >= 89)
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

        private void WhichPartCollidedWithTheBall(ref CollidedWithBall collidedWithBall)
        {
            //Gets the last auto-player, first-player and second-player -yAxis- values.
            int autoTemp = 0, firstPlayerTemp = _firstPlayer.YAxis, secondPlayerTemp = 0;

            if (_isPlayerVSPlayer)
                secondPlayerTemp = _secondPlayer.YAxis;
            else
                autoTemp = _autoPlayer.YAxis;

            for (int i = 0; i < 5; i++)
            {
                //Checks which part of the paddle collided with the ball - if so, saves that part.
                //First player and second player paddles.
                if (_isPlayerVSPlayer)
                {
                    if (_boardManager[firstPlayerTemp, _firstPlayer.XAxis] == _boardManager[_ball.YAxis, _ball.XAxis] || (_boardManager[secondPlayerTemp, _secondPlayer.XAxis] == _boardManager[_ball.YAxis, _ball.XAxis]))
                    {
                        ifCollidedWithAPaddle(ref collidedWithBall, i);
                        break;
                    }
                    secondPlayerTemp++;
                }
                else
                {
                    //Checks which part of the paddle collided with the ball - if so, saves that part.
                    //First player and computer paddles.
                    if (_boardManager[firstPlayerTemp, _firstPlayer.XAxis] == _boardManager[_ball.YAxis, _ball.XAxis] || (_boardManager[autoTemp, _autoPlayer.XAxis] == _boardManager[_ball.YAxis, _ball.XAxis]))
                    {
                        ifCollidedWithAPaddle(ref collidedWithBall, i);
                        break;
                    }
                    autoTemp++;
                }
                firstPlayerTemp++;
            }
        }

        private void ifCollidedWithAPaddle(ref CollidedWithBall collidedWithBall, int i)
        {
            if ((i == 0) || (i == 1))
                collidedWithBall = CollidedWithBall.UpperEdge;
            else if (i == 2)
                collidedWithBall = CollidedWithBall.MiddleEdge;
            else
                collidedWithBall = CollidedWithBall.BottomEdge;
        }

        //Sets back the last icon that the ball has deleted.
        private void KeepsTheWantedIcon(char icon)
        {
            if (icon == CharacterUtilities.PLAYER_ICON)
            {
                _boardManager[_ball.YAxis, _ball.XAxis] = icon;
                return;
            }
            else if (icon == CharacterUtilities.TOP_AND_BOTTOM_EDGES)
            {
                _boardManager[_ball.YAxis, _ball.XAxis] = icon;
                return;
            }
            else
                _boardManager[_ball.YAxis, _ball.XAxis] = EMPTY_PIXEL;
        }

        //Location represents the left side (manual-player) and the right side (auto-player) of the print.
        private void PrintsTheCurrentScore(int currentScore, int location)
        {
            _scoreboard.PrintCurrentScore(currentScore, location);
        }

        //Creates a switch statement in a Do-While loop to set the isRestarting boolean.
        public bool IsGameRestart()
        {
            GameStatus gameStatus = GameStatus.None;
            Console.CursorVisible = true;

            SwitchingToGameStatus(GetsTheUserOption(), ref gameStatus);

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

        private int GetsTheUserOption()
        {
            int oneOrTwo;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                _userInterface.PrintPongTitle();
                Console.SetCursorPosition(28, 7);
                Console.Write("Will you want to restart the game?");
                Console.SetCursorPosition(26, 8);
                Console.Write("Enter -1- to restart or -2- to exit: ");
            } while (!Int32.TryParse(Console.ReadLine(), out oneOrTwo));
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
                    SwitchingToGameStatus(GetsTheUserOption(), ref gameStatus);
                    break;
            }
        }

        //A random value for the ball starting position.
        private int RandomNumer()
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

        private void BallValueReset()
        {
            _boardManager[_ball.YAxis, _ball.XAxis] = ' ';
            //The next 3 lines resets the ball's coordinates.
            _ball.YAxis = (FIELD_HIGHT / 2);
            _ball.XAxis = (FIELD_WIDTH / 2);
            _ball.SetBallPosition(_boardManager);
        }

        private void AutoPlayerValueReset(ref bool isReachTop)
        {
            //The next 2 lines resets the auto-player's coordinates.
            isReachTop = false;
            _autoPlayer.ClearTheColumn(_boardManager);
        }
    }
}
