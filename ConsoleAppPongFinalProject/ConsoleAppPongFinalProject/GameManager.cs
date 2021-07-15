using System;
using System.Threading;

namespace ConsoleAppPongFinalProject
{
    class GameManager
    {
        public const int GOALS_TO_REACH = 5;

        private bool _isGoal = false;
        private bool _isGameOver = false;
        private bool _isFirstPlayerScored = false;
        private bool _isPVP = false;

        private UserInterface _userInterface = new UserInterface();
        private Scoreboard _scoreboard;
        private Board _board;
        private Ball _ball;
        private Player _firstPlayer;
        private Player _autoPlayer;
        private Player _secondPlayer;

        public GameManager()
        {
            _board = new Board();
            _ball = new Ball(Board.HalfFieldWidth, Board.HalfFieldHight);
            _ball.SetBallPosition(_board.GameField);
        }

        public void Start()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;

            GetUserChoice();
        }

        private void GetUserChoice()
        {
            UserChoice userChoice = UserChoice.None;
            _userInterface.MainMenu(ref userChoice);

            _firstPlayer = new Player(2, Board.HalfFieldHight - 2, _board.GameField);
            Thread mySingleWorker;

            switch (userChoice)
            {
                case UserChoice.SinglePlayer:
                    _autoPlayer = new Player(Board.FIELD_WIDTH - 3, Board.HalfFieldHight, _board.GameField);
                    mySingleWorker = new Thread(ThreadFunctionForTheBall_AutoPlayer);
                    mySingleWorker.Start();
                    HandlePlayerInput();
                    break;
                case UserChoice.MultiPlayers:
                    _isPVP = true;
                    _secondPlayer = new Player(Board.FIELD_WIDTH - 3, Board.HalfFieldHight - 2, _board.GameField);
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
                        if (!_board.IsPaddleReachTopBorder(_firstPlayer.YAxis, _firstPlayer.XAxis))
                        {
                            _firstPlayer.YAxis--;
                            _board.ClearBottomPaddleEdge(_firstPlayer.YAxis, _firstPlayer.XAxis);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (!_board.IsPaddleReachBottomBorder(_firstPlayer.YAxis, _firstPlayer.XAxis))
                        {
                            _firstPlayer.YAxis++;
                            _board.ClearTopPaddleEdge(_firstPlayer.YAxis, _firstPlayer.XAxis);
                        }
                        break;
                }
                _firstPlayer.SetPlayerPosition(_board.GameField);
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
                        if (!_board.IsPaddleReachTopBorder(_firstPlayer.YAxis, _firstPlayer.XAxis))
                        {
                            _firstPlayer.YAxis--;
                            _board.ClearBottomPaddleEdge(_firstPlayer.YAxis, _firstPlayer.XAxis);
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (!_board.IsPaddleReachBottomBorder(_firstPlayer.YAxis, _firstPlayer.XAxis))
                        {
                            _firstPlayer.YAxis++;
                            _board.ClearTopPaddleEdge(_firstPlayer.YAxis, _firstPlayer.XAxis);
                        }
                        break;
                    case ConsoleKey.W:
                        if (!_board.IsPaddleReachTopBorder(_secondPlayer.YAxis, _secondPlayer.XAxis))
                        {
                            _secondPlayer.YAxis--;
                            _board.ClearBottomPaddleEdge(_secondPlayer.YAxis, _secondPlayer.XAxis);
                        }
                        break;
                    case ConsoleKey.S:
                        if (!_board.IsPaddleReachBottomBorder(_secondPlayer.YAxis, _secondPlayer.XAxis))
                        {
                            _secondPlayer.YAxis++;
                            _board.ClearTopPaddleEdge(_secondPlayer.YAxis, _secondPlayer.XAxis);
                        }
                        break;
                }
                _firstPlayer.SetPlayerPosition(_board.GameField);
                _secondPlayer.SetPlayerPosition(_board.GameField);

            } while (!_isGameOver);
        }

        private void PrintGameField()
        {
            _userInterface.PrintPongTitle();
            for (int i = 0; i < _board.GameField.GetLength(0); i++)
            {
                for (int j = 0; j < _board.GameField.GetLength(1); j++)
                {
                    Console.Write(_board.GameField[i, j]);
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
            int ballXDiraction = -1, ballYDiraction = 0;
            _scoreboard = new Scoreboard();

            do
            {
                HandleAIMovement(ref isReachTop);
                PrintGameField();
                IsCollidedWithAnObject(temp, ref ballXDiraction, ref ballYDiraction);

                if (_isGoal)
                {
                    SetAIAtMid();
                    CheckWhoScored(_isFirstPlayerScored);
                    OnGoalScored(ref temp, ref ballXDiraction, ref ballYDiraction);

                    if (IsReachMaxGoals(_firstPlayer.Score))
                    {
                        _isGameOver = true;
                        _userInterface.PrintWinner(_userInterface.PlayerOne);
                        Highscore highscore = new Highscore();
                        highscore.HighscoreWriter(_userInterface.PlayerOne, _firstPlayer.Score, _autoPlayer.Score);
                        break;
                    }
                    else if (IsReachMaxGoals(_autoPlayer.Score))
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
            isReachTop = false;
        }

        private void OnGoalScored(ref char temp, ref int ballXDiraction, ref int ballYDiraction)
        {
            _board.GameField[_ball.YAxis, _ball.XAxis] = temp;
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
                    CheckWhoScored(_isFirstPlayerScored, _isPVP);
                    OnGoalScored(ref temp, ref ballXDiraction, ref ballYDiraction);

                    if (IsReachMaxGoals(_firstPlayer.Score))
                    {
                        _isGameOver = true;
                        _userInterface.PrintWinner(_userInterface.PlayerOne);
                        Highscore highscore = new Highscore();
                        highscore.HighscoreWriter(_userInterface.PlayerOne, _userInterface.PlayerTwo , _firstPlayer.Score, _secondPlayer.Score);
                        break;
                    }
                    else if (IsReachMaxGoals(_secondPlayer.Score))
                    {
                        _isGameOver = true;
                        _userInterface.PrintWinner(_userInterface.PlayerTwo);
                        Highscore highscore = new Highscore();
                        highscore.HighscoreWriter(_userInterface.PlayerOne ,_userInterface.PlayerTwo, _firstPlayer.Score, _secondPlayer.Score);
                        break;
                    }
                }

                HandleBallMovement(ref temp, ref ballXDiraction, ref ballYDiraction);

            } while (!_isGameOver);
            ResetBallValue();
        }

        private void CreateBallInconsistently(ref int ballYDiraction, ref int ballXDiraction)
        {
            //Spawns the ball at Inconsistently coordinates.
            _ball.YAxis = RandomNumer() + RandomNumer() + RandomNumer() + RandomNumer() + Board.HalfFieldHight;
            _ball.XAxis = 2 + RandomNumer() + Board.HalfFieldWidth;
            ballYDiraction = RandomNumer();

            if (_isPVP)
                ballXDiraction *= (-1);
            else
                ballXDiraction = -1;

            _ball.SetBallPosition(_board.GameField);
        }

        private void SetAIAtMid()
        {
            //Sets the auto-player's coordinates at the middle field.
            EraseAIColumnLeftovers(_board.GameField);
            _autoPlayer.YAxis = Board.HalfFieldHight - 2;
            _autoPlayer.XAxis = Board.FIELD_WIDTH - 3;
            _autoPlayer.SetPlayerPosition(_board.GameField);
        }

        //Score method for the first player aginst the computer.
        private void CheckWhoScored(bool isSoloPlay)
        {
            if (isSoloPlay)
            {
                _firstPlayer.IncreaseScoreByOne();
                PrintScore(_firstPlayer.Score, location: 0);
            }
            else
            {
                _autoPlayer.IncreaseScoreByOne();
                PrintScore(_autoPlayer.Score, location: 83);
            }
        }

        //Score method for the first player aginst the second player.
        private void CheckWhoScored(bool isSoloPlay, bool isMultiPlayers)
        {
            if (isSoloPlay)
            {
                _firstPlayer.IncreaseScoreByOne();
                PrintScore(_firstPlayer.Score, location: 0);
            }
            else
            {
                _secondPlayer.IncreaseScoreByOne();
                PrintScore(_secondPlayer.Score, location: 83);
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
            temp = _board.GameField[_ball.YAxis, _ball.XAxis];

            //Sets the ball to his new location.
            _ball.SetBallPosition(_board.GameField);
        }

        private void HandleAIMovement(ref bool isReachTop)
        {
            if (!_board.IsPaddleReachTopBorder(_autoPlayer.YAxis, _autoPlayer.XAxis) && (!isReachTop))
            {
                _autoPlayer.YAxis--;
                _board.ClearBottomPaddleEdge(_autoPlayer.YAxis, _autoPlayer.XAxis);
            }
            else if (!_board.IsPaddleReachBottomBorder(_autoPlayer.YAxis, _autoPlayer.XAxis))
            {
                isReachTop = true;
                _autoPlayer.YAxis++;
                _board.ClearTopPaddleEdge(_autoPlayer.YAxis, _autoPlayer.XAxis);
            }
            else
                isReachTop = false;

            _autoPlayer.SetPlayerPosition(_board.GameField);
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
            else if (temp == CharacterUtilities.TOP_BOTTOM_BORDER_ICON)
                yDiraction *= (-1);

            //Checks if the ball collided with the left/right edge - if so, checks which side of the edge the ball collided with -> Saves the result -> returns that a goal has occurred.
            else if (temp == CharacterUtilities.LEFT_RIGHT_BORDER_ICON)
            {
                if (_ball.XAxis >= 89)
                    _isFirstPlayerScored = true;
                else
                    _isFirstPlayerScored = false;

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
                    if (_board.GameField[firstPlayerTemp, _firstPlayer.XAxis] == _board.GameField[_ball.YAxis, _ball.XAxis] ||
                        (_board.GameField[secondPlayerTemp, _secondPlayer.XAxis] == _board.GameField[_ball.YAxis, _ball.XAxis]))
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
                    if (_board.GameField[firstPlayerTemp, _firstPlayer.XAxis] == _board.GameField[_ball.YAxis, _ball.XAxis] ||
                        (_board.GameField[autoTemp, _autoPlayer.XAxis] == _board.GameField[_ball.YAxis, _ball.XAxis]))
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
                _board.GameField[_ball.YAxis, _ball.XAxis] = temp;
                return;
            }
            else if (temp == CharacterUtilities.TOP_BOTTOM_BORDER_ICON)
            {
                _board.GameField[_ball.YAxis, _ball.XAxis] = temp;
                return;
            }
            else
                _board.GameField[_ball.YAxis, _ball.XAxis] = CharacterUtilities.EMPTY_PIXEL;
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
            _board.GameField[_ball.YAxis, _ball.XAxis] = CharacterUtilities.EMPTY_PIXEL;
            //The next 3 lines resets the ball's coordinates.
            _ball.YAxis = Board.HalfFieldHight;
            _ball.XAxis = Board.HalfFieldWidth;
            _ball.SetBallPosition(_board.GameField);
        }

        private void EraseAIColumnLeftovers(char[,] gameField)
        {
            for (int i = 1; i < 22; i++)
            {
                for (int j = 87; j <= 87; j++)
                {
                    gameField[i, j] = CharacterUtilities.EMPTY_PIXEL;
                }
            }
        }
    }
}