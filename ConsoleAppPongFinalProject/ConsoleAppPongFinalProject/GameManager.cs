using System;
using System.Threading;

namespace ConsoleAppPongFinalProject
{
    class GameManager
    {
        public const int FIELD_HIGHT = 23;
        public const int FIELD_WIDTH = 90;
        public const int GOALS_TO_REACH = 5;

        private char[,] _gameField;
        private bool _isGoal = false;
        private bool _isGameOver = false;
        private bool _isFirstPlayer = false;
        private bool _isAI = false;
        private bool _isPVP = false;
        private int _firstPlayerGoalCount, _autoGoalCount, _secondPlayerGoalCount;

        //The next 7 lines initiates the objects.
        UserInterface _userInterface = new UserInterface();
        Scoreboard _scoreboard;
        Board _board = new Board();
        Ball _ball = new Ball((FIELD_WIDTH / 2), (FIELD_HIGHT / 2));
        FirstPlayer _firstPlayer = new FirstPlayer(2, ((FIELD_HIGHT / 2) - 2));
        AutoPlayer _autoPlayer;
        SecondPlayer _secondPlayer;
        Highscore _highscore = new Highscore();

        public GameManager()
        {
            _gameField = new char[FIELD_HIGHT, FIELD_WIDTH];
            _board.SetBoard(_gameField);
            _ball.SetBallPosition(_gameField);
            _firstPlayer.SetFirstPlayerPosition(_gameField);
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
                    _autoGoalCount = 0;
                    _autoPlayer = new AutoPlayer((FIELD_WIDTH - 3), ((FIELD_HIGHT / 2) - 8));
                    _autoPlayer.SetAutoPlayerPosition(_gameField);
                    Thread mySingleWorker = new Thread(ThreadFunctionForTheBall_AutoPlayer);
                    mySingleWorker.Start();
                    HandlePlayerInput();
                    break;
                case UserChoice.MultiPlayers:
                    _isPVP = true;
                    _secondPlayerGoalCount = 0;
                    _secondPlayer = new SecondPlayer((FIELD_WIDTH - 3), ((FIELD_HIGHT / 2) - 2));
                    _secondPlayer.SetSecondPlayerPosition(_gameField);
                    Thread myMultiWorker = new Thread(ThreadFunctionForTheBall_PVP);
                    myMultiWorker.Start();
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
                        if (!IsPaddleReachTopBorder(_firstPlayer.YAxis, _firstPlayer.XAxis))
                        {
                            _firstPlayer.YAxis--;
                            ClearBottomPaddleEdge(_firstPlayer.YAxis, _firstPlayer.XAxis);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (!IsPaddleReachBottomBorder(_firstPlayer.YAxis, _firstPlayer.XAxis))
                        {
                            _firstPlayer.YAxis++;
                            ClearTopPaddleEdge(_firstPlayer.YAxis, _firstPlayer.XAxis);
                        }
                        break;
                }
                _firstPlayer.SetFirstPlayerPosition(_gameField);
            } while (!_isGameOver);
        }

        private void ClearTopPaddleEdge(int y, int x) => _gameField[y - 1, x] = CharacterUtilities.EMPTY_PIXEL;

        private void ClearBottomPaddleEdge(int y, int x) => _gameField[y + 5, x] = CharacterUtilities.EMPTY_PIXEL;

        private bool IsPaddleReachBottomBorder(int y, int x) => _gameField[y + 5, x] == CharacterUtilities.TOP_BOTTOM_EDGE_ICON;

        private bool IsPaddleReachTopBorder(int y, int x) => _gameField[y - 1, x] == CharacterUtilities.TOP_BOTTOM_EDGE_ICON;

        private void HandlePlayersInput()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (!IsPaddleReachTopBorder(_firstPlayer.YAxis, _firstPlayer.XAxis))
                        {
                            _firstPlayer.YAxis--;
                            ClearBottomPaddleEdge(_firstPlayer.YAxis, _firstPlayer.XAxis);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (!IsPaddleReachBottomBorder(_firstPlayer.YAxis, _firstPlayer.XAxis))
                        {
                            _firstPlayer.YAxis++;
                            ClearTopPaddleEdge(_firstPlayer.YAxis, _firstPlayer.XAxis);
                        }
                        break;
                    case ConsoleKey.W:
                        if (!IsPaddleReachTopBorder(_secondPlayer.YAxis, _secondPlayer.XAxis))
                        {
                            _secondPlayer.YAxis--;
                            ClearBottomPaddleEdge(_secondPlayer.YAxis, _secondPlayer.XAxis);
                        }
                        break;
                    case ConsoleKey.S:
                        if (!IsPaddleReachBottomBorder(_secondPlayer.YAxis, _secondPlayer.XAxis))
                        {
                            _secondPlayer.YAxis++;
                            ClearTopPaddleEdge(_secondPlayer.YAxis, _secondPlayer.XAxis);
                        }
                        break;
                }
                _firstPlayer.SetFirstPlayerPosition(_gameField);
                _secondPlayer.SetSecondPlayerPosition(_gameField);

            } while (!_isGameOver);
        }

        private void PrintGameField()
        {
            _userInterface.PrintPongTitle();
            for (int i = 0; i < _gameField.GetLength(0); i++)
            {
                for (int j = 0; j < _gameField.GetLength(1); j++)
                {
                    Console.Write(_gameField[i, j]);
                }
                Console.WriteLine();
            }
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
            bool isReachTop = false;
            Console.Clear();
            char temp = CharacterUtilities.EMPTY_PIXEL;
            int ballXDiraction = 1, ballYDiraction = 0;
            _scoreboard = new Scoreboard();

            do
            {
                HandleAIMovement(ref isReachTop);
                PrintGameField();
                IsCollidedWithAnObject(temp, ref ballXDiraction, ref ballYDiraction);

                if (_isGoal)
                {
                    SetAIAtMid();
                    CheckWhoScored(_isFirstPlayer);
                    OnGoalScored(ref temp, ref ballXDiraction, ref ballYDiraction);

                    if (IsReachMaxGoals(_firstPlayerGoalCount))
                    {
                        _isGameOver = true;
                        _userInterface.PrintWinner(_userInterface.PlayerOne);
                        _highscore.HighscoreWriter(_userInterface.PlayerOne, _firstPlayerGoalCount, _autoGoalCount);
                        break;
                    }
                    else if (IsReachMaxGoals(_autoGoalCount))
                    {
                        _isGameOver = true;
                        _userInterface.PrintWinner("Computer");
                        Console.SetCursorPosition(30, 16);
                        Console.WriteLine(_userInterface.PlayerOne + ", good luck next time.");
                        break;
                    }
                }

                HandleBallMovement(ref temp, ref ballXDiraction, ref ballYDiraction);

            } while (!_isGameOver);

            ResetBallValue();
            ResetAIValue(ref isReachTop);
        }

        private void OnGoalScored(ref char temp, ref int ballXDiraction, ref int ballYDiraction)
        {
            _gameField[_ball.YAxis, _ball.XAxis] = temp;
            temp = CharacterUtilities.EMPTY_PIXEL;
            CreateBallInconsistently(ref ballYDiraction, ref ballXDiraction);
            _isGoal = false;
            Thread.Sleep(1300);
        }

        private bool IsReachMaxGoals(int currentGoals) => currentGoals == GOALS_TO_REACH;

        /// <summary>
        /// Thread #3 that controls the -ball-.
        /// Called if the user has chosen to play as multi player.
        /// Creates a do-while loop.
        /// Firstly, moves the -ball- repeatedly along the borders.
        /// Secondly, checks if there was a goal (ball collided with left/right edge) - if so, checks who scored it (Puts +1 to his score) -> if the score reachs 5 the game has ended.
        ///                                                                           - if not, continue to the next if statement.         -> if not, resets the ball's and the second player's values.
        /// Thirdly, checks if the ball collided with any of the borders/players - if so, increments the ball's coordinates according to the collision effect.
        ///                                                                      - if not, moves the ball according to his last coordinates.
        /// Saves the last icon (so it won't be deleted). Changes the ball -X- and -Y- as needed. Sets the last icon the his previous location. Sets the ball to his new location.
        /// </summary>
        private void ThreadFunctionForTheBall_PVP()
        {
            Console.Clear();
            char temp = CharacterUtilities.EMPTY_PIXEL;
            int ballXDiraction = 1, ballYDiraction = 0;
            _scoreboard = new Scoreboard();

            do
            {
                PrintGameField();
                IsCollidedWithAnObject(temp, ref ballXDiraction, ref ballYDiraction);

                if (_isGoal)
                {
                    CheckWhoScored(_isFirstPlayer, _isPVP);
                    OnGoalScored(ref temp, ref ballXDiraction, ref ballYDiraction);

                    if (IsReachMaxGoals(_firstPlayerGoalCount))
                    {
                        _isGameOver = true;
                        _userInterface.PrintWinner(_userInterface.PlayerOne);
                        _highscore.HighscoreWriter(_userInterface.PlayerOne, _userInterface.PlayerTwo ,_firstPlayerGoalCount, _secondPlayerGoalCount);
                        break;
                    }
                    else if (IsReachMaxGoals(_secondPlayerGoalCount))
                    {
                        _isGameOver = true;
                        _userInterface.PrintWinner(_userInterface.PlayerTwo);
                        _highscore.HighscoreWriter(_userInterface.PlayerOne ,_userInterface.PlayerTwo, _firstPlayerGoalCount, _secondPlayerGoalCount);
                        break;
                    }
                }

                HandleBallMovement(ref temp, ref ballXDiraction, ref ballYDiraction);

            } while (!_isGameOver);
            ResetBallValue();
            _secondPlayer.ClearColumn(_gameField);
        }

        private void CreateBallInconsistently(ref int ballYDiraction, ref int ballXDiraction)
        {
            //Spawns the ball at Inconsistently coordinates.
            _ball.YAxis = (RandomNumer() + RandomNumer() + RandomNumer() + RandomNumer() + (FIELD_HIGHT / 2));
            _ball.XAxis = (2 + RandomNumer() + (FIELD_WIDTH / 2));
            ballYDiraction = RandomNumer();

            if (_isPVP)
                ballXDiraction *= (-1);
            else
                ballXDiraction = -1;

            _ball.SetBallPosition(_gameField);
        }

        private void SetAIAtMid()
        {
            //Sets the auto-player's coordinates at the middle field.
            _autoPlayer.ClearColumn(_gameField);
            _autoPlayer.YAxis = ((FIELD_HIGHT / 2) - 2);
            _autoPlayer.XAxis = (FIELD_WIDTH - 3);
            _autoPlayer.SetAutoPlayerPosition(_gameField);
        }

        //Score method for the first player aginst the computer.
        private void CheckWhoScored(bool isSoloPlay)
        {
            if (isSoloPlay)
            {
                _firstPlayerGoalCount++;
                PrintScore(_firstPlayerGoalCount, location: 0);
            }
            else
            {
                _autoGoalCount++;
                PrintScore(_autoGoalCount, location: 83);
            }
        }

        //Score method for the first player aginst the second player.
        private void CheckWhoScored(bool isSoloPlay, bool isMultiPlayers)
        {
            if (isSoloPlay)
            {
                _firstPlayerGoalCount++;
                PrintScore(_firstPlayerGoalCount, location: 0);
            }
            else
            {
                _secondPlayerGoalCount++;
                PrintScore(_secondPlayerGoalCount, location: 83);
            }
        }

        private void HandleBallMovement(ref char temp, ref int xDiraction, ref int yDiraction)
        {
            //Saves the last icon (so it won't be deleted).
            KeepWantedIcon(temp);

            //Changes the ball -X- and -Y- as needed.
            _ball.XAxis += xDiraction;
            _ball.YAxis += yDiraction;

            //Sets the last icon the his previous location.
            temp = _gameField[_ball.YAxis, _ball.XAxis];

            //Sets the ball to his new location.
            _ball.SetBallPosition(_gameField);
        }

        private void HandleAIMovement(ref bool isReachTop)
        {
            if (!IsPaddleReachTopBorder(_autoPlayer.YAxis, _autoPlayer.XAxis) && (!isReachTop))
            {
                _autoPlayer.YAxis--;
                ClearBottomPaddleEdge(_autoPlayer.YAxis, _autoPlayer.XAxis);
            }
            else if (!IsPaddleReachBottomBorder(_autoPlayer.YAxis, _autoPlayer.XAxis))
            {
                isReachTop = true;
                _autoPlayer.YAxis++;
                ClearTopPaddleEdge(_autoPlayer.YAxis, _autoPlayer.XAxis);
            }
            else
                isReachTop = false;

            _autoPlayer.SetAutoPlayerPosition(_gameField);
        }

        private void IsCollidedWithAnObject(char temp, ref int xDiraction, ref int yDiraction)
        {
            //Checks if the ball collided with a paddle.
            if (temp == CharacterUtilities.PLAYER_ICON)
            {
                PaddleEdge collidedWithBall = PaddleEdge.None;
                WhichPaddleEdgeCollidedWithBall(ref collidedWithBall);
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
            else if (temp == CharacterUtilities.TOP_BOTTOM_EDGE_ICON)
                yDiraction *= (-1);

            //Checks if the ball collided with the left/right edge - if so, checks which side of the edge the ball collided with -> Saves the result -> returns that a goal has occurred.
            else if (temp == CharacterUtilities.LEFT_RIGHT_EDGE_ICON)
            {
                if (_ball.XAxis >= 89)
                {
                    _isAI = false;
                    _isFirstPlayer = true;
                }
                else
                {
                    _isAI = true;
                    _isFirstPlayer = false;
                }
                _isGoal = true;
            }
        }

        private void WhichPaddleEdgeCollidedWithBall(ref PaddleEdge collidedWithBall)
        {
            //Gets the last auto-player, first-player and second-player -yAxis- values.
            int autoTemp = 0, firstPlayerTemp = _firstPlayer.YAxis, secondPlayerTemp = 0;

            if (_isPVP)
                secondPlayerTemp = _secondPlayer.YAxis;
            else
                autoTemp = _autoPlayer.YAxis;

            for (int i = 0; i < 5; i++)
            {
                //Checks which part of the paddle collided with the ball - if so, saves that part.
                //First player and second player paddles.
                if (_isPVP)
                {
                    if (_gameField[firstPlayerTemp, _firstPlayer.XAxis] == _gameField[_ball.YAxis, _ball.XAxis] || (_gameField[secondPlayerTemp, _secondPlayer.XAxis] == _gameField[_ball.YAxis, _ball.XAxis]))
                    {
                        GetCollidedPaddleEdge(ref collidedWithBall, i);
                        break;
                    }
                    secondPlayerTemp++;
                }
                else
                {
                    //Checks which part of the paddle collided with the ball - if so, saves that part.
                    //First player and computer paddles.
                    if (_gameField[firstPlayerTemp, _firstPlayer.XAxis] == _gameField[_ball.YAxis, _ball.XAxis] || (_gameField[autoTemp, _autoPlayer.XAxis] == _gameField[_ball.YAxis, _ball.XAxis]))
                    {
                        GetCollidedPaddleEdge(ref collidedWithBall, i);
                        break;
                    }
                    autoTemp++;
                }
                firstPlayerTemp++;
            }
        }

        private void GetCollidedPaddleEdge(ref PaddleEdge collidedWithBall, int i)
        {
            if ((i == 0) || (i == 1))
                collidedWithBall = PaddleEdge.UpperEdge;
            else if (i == 2)
                collidedWithBall = PaddleEdge.MiddleEdge;
            else
                collidedWithBall = PaddleEdge.BottomEdge;
        }

        //Sets back the last icon that the ball has deleted.
        private void KeepWantedIcon(char temp)
        {

            if (temp == CharacterUtilities.PLAYER_ICON)
            {
                _gameField[_ball.YAxis, _ball.XAxis] = temp;
                return;
            }
            else if (temp == CharacterUtilities.TOP_BOTTOM_EDGE_ICON)
            {
                _gameField[_ball.YAxis, _ball.XAxis] = temp;
                return;
            }
            else
                _gameField[_ball.YAxis, _ball.XAxis] = CharacterUtilities.EMPTY_PIXEL;
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
                _userInterface.PrintPongTitle();
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

        //A random value for the ball starting position.
        private int RandomNumer()
        {
            Random rnd = new Random();
            int horizontalOrVertical = rnd.Next(0, 3);

            if (horizontalOrVertical == 0)
                horizontalOrVertical = -1;

            else if (horizontalOrVertical == 1)
                horizontalOrVertical = 0;

            else
                horizontalOrVertical = 1;

            return horizontalOrVertical;
        }

        private void ResetBallValue()
        {
            _gameField[_ball.YAxis, _ball.XAxis] = CharacterUtilities.EMPTY_PIXEL;
            //The next 3 lines resets the ball's coordinates.
            _ball.YAxis = (FIELD_HIGHT / 2);
            _ball.XAxis = (FIELD_WIDTH / 2);
            _ball.SetBallPosition(_gameField);
        }

        private void ResetAIValue(ref bool isReachTop)
        {
            //The next 2 lines resets the auto-player's coordinates.
            isReachTop = false;
            _autoPlayer.ClearColumn(_gameField);
        }
    }
}