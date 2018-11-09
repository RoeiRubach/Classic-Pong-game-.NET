using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleAppPongFinalProject
{
    class GameManager
    {
        public const char ballIcon = 'o';
        public const char CursorIcon = '■';
        public const char playerIcon = '█';
        public const char topBottomEdge = '═';
        public const char leftRightEdge = '║';
        private const char emptyPixel = ' ';
        private char[,] gameField;
        private const int fieldHight = 23;
        private const int fieldWidth = 90;
        private const int gameOver = 1;
        private bool isGoal = false;
        private bool isGameOver = false;
        private bool isFirstPlayer = false;
        private bool isOtherPlayer = false;
        private bool isMultiPlayers = false;
        private int firstPlayerGoalCount, autoGoalCount, secondPlayerGoalCount;

        //The next 7 lines initiates the objects.
        UserInterface userInterface = new UserInterface();
        Scoreboard scoreboard = new Scoreboard();
        Board board = new Board();
        Ball ball = new Ball((fieldWidth / 2), (fieldHight / 2));
        AutoPlayer autoPlayer;
        FirstPlayer firstPlayer = new FirstPlayer(2, ((fieldHight / 2) - 2));
        SecondPlayer secondPlayer;
        Highscore highscore = new Highscore();

        public GameManager()
        {
            gameField = new char[fieldHight, fieldWidth];
            board.SetsTheBoard(gameField);
            ball.SetsTheBallPosition(gameField);
            firstPlayer.SetsTheFirstPlayerPosition(gameField);
        }

        //Controls the game logic and runs it.
        public void Start()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            firstPlayerGoalCount = 0;

            GetUserChoice();
            StartManualPlayerLoop();
            IsGameRestart();
        }

        private void GetUserChoice()
        {
            UserChoice userChoice = UserChoice.None;
            userInterface.MainMenu(ref userChoice);

            switch (userChoice)
            {
                case UserChoice.SinglePlayer:
                    autoGoalCount = 0;
                    autoPlayer = new AutoPlayer((fieldWidth - 3), ((fieldHight / 2) - 8));
                    autoPlayer.SetsTheAutoPlayerPosition(gameField);
                    Thread mySingleWorker = new Thread(ThreadFunctionForTheBall_AutoPlayer);
                    mySingleWorker.Start();
                    break;
                case UserChoice.MultiPlayer:
                    secondPlayerGoalCount = 0;
                    secondPlayer = new SecondPlayer((fieldWidth - 3), ((fieldHight / 2) - 2));
                    secondPlayer.SetsTheSecondPlayerPosition(gameField);
                    Thread myMultiWorker = new Thread(ThreadFunctionForTheBall);
                    myMultiWorker.Start();
                    break;
            }
        }

        private void StartManualPlayerLoop()
        {
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        //Checks if the FirstPlayer gets out the border.
                        if (gameField[firstPlayer.yAxis - 1, firstPlayer.xAxis] != GameManager.topBottomEdge)
                        {
                            firstPlayer.yAxis--;
                            gameField[firstPlayer.yAxis + 5, firstPlayer.xAxis] = ' ';
                        }
                        firstPlayer.SetsTheFirstPlayerPosition(gameField);
                        break;
                    case ConsoleKey.DownArrow:
                        //Checks if the FirstPlayer gets out the border.
                        if (gameField[firstPlayer.yAxis + 5, firstPlayer.xAxis] != GameManager.topBottomEdge)
                        {
                            firstPlayer.yAxis++;
                            gameField[firstPlayer.yAxis - 1, firstPlayer.xAxis] = ' ';
                        }
                        firstPlayer.SetsTheFirstPlayerPosition(gameField);
                        break;
                    case ConsoleKey.W:
                        //Checks if the SecondPlayer gets out the border.
                        if (gameField[secondPlayer.yAxis - 1, secondPlayer.xAxis] != GameManager.topBottomEdge)
                        {
                            secondPlayer.yAxis--;
                            gameField[secondPlayer.yAxis + 5, secondPlayer.xAxis] = ' ';
                        }
                        secondPlayer.SetsTheSecondPlayerPosition(gameField);
                        break;
                    case ConsoleKey.S:
                        //Checks if the SecondPlayer gets out the border.
                        if (gameField[secondPlayer.yAxis + 5, secondPlayer.xAxis] != GameManager.topBottomEdge)
                        {
                            secondPlayer.yAxis++;
                            gameField[secondPlayer.yAxis - 1, secondPlayer.xAxis] = ' ';
                        }
                        secondPlayer.SetsTheSecondPlayerPosition(gameField);
                        break;
                    default:
                        //Checks if the game has ended.
                        if ((firstPlayerGoalCount == gameOver) || (autoGoalCount == gameOver) || (secondPlayerGoalCount == gameOver))
                        {
                            Console.SetCursorPosition(0, 28);
                            Console.WriteLine("Press any key to continue..");
                            Console.ReadKey(true);
                        }
                        break;
                }

            } while (!isGameOver);
        }

        private void PrintsTheGameField()
        {
            userInterface.PrintsTheTitle();
            for (int i = 0; i < gameField.GetLength(0); i++)
            {
                for (int j = 0; j < gameField.GetLength(1); j++)
                {
                    Console.Write(gameField[i, j]);
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
            Console.Clear();
            bool isReachTop = false;
            isFirstPlayer = false;
            isOtherPlayer = false;
            isGoal = false;
            isGameOver = false;
            char temp = emptyPixel;
            int ballXDiraction = 1, ballYDiraction = 0;
            //First score for the manual player.
            scoreboard.GetsTheScore(0, 0);
            //First score for the auto player.
            scoreboard.GetsTheScore(0, 83);

            do
            {
                if (isGoal)
                {
                    ChecksWhoScored(isFirstPlayer);
                    gameField[ball.yAxis, ball.xAxis] = temp;
                    temp = emptyPixel;

                    CreatesBallInconsistently(ref ballYDiraction, ref ballXDiraction, isMultiPlayers);
                    SetsAutoPlayerAtMid();

                    isGoal = false;
                    Thread.Sleep(1300);

                    if (firstPlayerGoalCount == gameOver)
                    {
                        isGameOver = true;
                        Console.SetCursorPosition(37, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(userInterface.player1 + " wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        highscore.HighscoreWriter(userInterface.player1, firstPlayerGoalCount, autoGoalCount);
                        break;
                    }
                    else if (autoGoalCount == gameOver)
                    {
                        isGameOver = true;
                        Console.SetCursorPosition(35, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Computer wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(30, 16);
                        Console.WriteLine(userInterface.player1 + ", good luck next time.");
                        break;
                    }
                }
                AutoPlayerMovementLogic(ref isReachTop);
                PrintsTheGameField();
                if (!IsCollidedWithAnObject(ref isGoal, temp, ref ballXDiraction, ref ballYDiraction, ref isFirstPlayer, ref isOtherPlayer))
                {
                    BallMovementLogic(ref temp, ref ballXDiraction, ref ballYDiraction);
                }
            } while (!isGameOver);
            BallValueReset();
            AutoPlayerValueReset(ref isReachTop);
        }

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
        private void ThreadFunctionForTheBall()
        {
            Console.Clear();
            isFirstPlayer = false;
            isOtherPlayer = false;
            isGoal = false;
            isGameOver = false;
            isMultiPlayers = true;
            char temp = emptyPixel;
            int ballXDiraction = 1, ballYDiraction = 0;
            //First score for the first player.
            scoreboard.GetsTheScore(0, 0);
            //First score for the second player.
            scoreboard.GetsTheScore(0, 83);

            do
            {
                if (isGoal)
                {
                    ChecksWhoScored(isFirstPlayer , isMultiPlayers);
                    gameField[ball.yAxis, ball.xAxis] = temp;
                    temp = emptyPixel;

                    CreatesBallInconsistently(ref ballYDiraction, ref ballXDiraction, isMultiPlayers);

                    isGoal = false;
                    Thread.Sleep(1300);

                    if (firstPlayerGoalCount == gameOver)
                    {
                        isGameOver = true;
                        Console.SetCursorPosition(37, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(userInterface.player1 + " wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        highscore.HighscoreWriter(userInterface.player1, userInterface.player2 ,firstPlayerGoalCount, secondPlayerGoalCount);
                        break;
                    }
                    else if (secondPlayerGoalCount == gameOver)
                    {
                        isGameOver = true;
                        Console.SetCursorPosition(35, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(userInterface.player2 + " wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        highscore.HighscoreWriter(userInterface.player1 ,userInterface.player2, firstPlayerGoalCount, secondPlayerGoalCount);
                        break;
                    }
                }
                PrintsTheGameField();
                if (!IsCollidedWithAnObject(ref isGoal, temp, ref ballXDiraction, ref ballYDiraction, ref isFirstPlayer, ref isOtherPlayer))
                {
                    BallMovementLogic(ref temp, ref ballXDiraction, ref ballYDiraction);
                }
            } while (!isGameOver);
            BallValueReset();
            secondPlayer.ClearTheColumn(gameField);
        }

        private void CreatesBallInconsistently(ref int ballYDiraction, ref int ballXDiraction, bool isTwoPlayers)
        {
            //Spawns the ball at Inconsistently coordinates.
            ball.yAxis = (RandomNumer() + RandomNumer() + RandomNumer() + RandomNumer() + (fieldHight / 2));
            ball.xAxis = (2 + RandomNumer() + (fieldWidth / 2));
            ballYDiraction = RandomNumer();
            if (isTwoPlayers)
            {
                ballXDiraction *= (-1);
            }
            else
            {
                ballXDiraction = -1;
            }
            ball.SetsTheBallPosition(gameField);
        }

        private void SetsAutoPlayerAtMid()
        {
            //Sets the auto-player's coordinates at the middle field.
            autoPlayer.ClearTheColumn(gameField);
            autoPlayer.yAxis = ((fieldHight / 2) - 2);
            autoPlayer.xAxis = (fieldWidth - 3);
            autoPlayer.SetsTheAutoPlayerPosition(gameField);
        }

        //Score method for the first player aginst the computer.
        private void ChecksWhoScored(bool isManual)
        {
            if (isManual)
            {
                int location = 0;
                firstPlayerGoalCount++;
                PrintsTheCurrentScore(firstPlayerGoalCount, location);
            }
            else
            {
                int location = 83;
                autoGoalCount++;
                PrintsTheCurrentScore(autoGoalCount, location);
            }
        }

        //Score method for the first player aginst the second player.
        private void ChecksWhoScored(bool isManual, bool isMultiPlayers)
        {
            if (isManual)
            {
                int location = 0;
                firstPlayerGoalCount++;
                PrintsTheCurrentScore(firstPlayerGoalCount, location);
            }
            else
            {
                int location = 83;
                secondPlayerGoalCount++;
                PrintsTheCurrentScore(secondPlayerGoalCount, location);
            }
        }

        private void BallMovementLogic(ref char temp, ref int xDiraction, ref int yDiraction)
        {
            //Saves the last icon (so it won't be deleted).
            KeepsTheWantedIcon(temp);

            //Changes the ball -X- and -Y- as needed.
            ball.xAxis += xDiraction;
            ball.yAxis += yDiraction;

            //Sets the last icon the his previous location.
            temp = gameField[ball.yAxis, ball.xAxis];

            //Sets the ball to his new location.
            ball.SetsTheBallPosition(gameField);
        }

        private void AutoPlayerMovementLogic(ref bool isReachTop)
        {
            //Checks if the paddle reached the upper edge.
            if ((gameField[autoPlayer.yAxis - 1, autoPlayer.xAxis] != GameManager.topBottomEdge) && (!isReachTop))
            {
                autoPlayer.yAxis--;
                gameField[autoPlayer.yAxis + 5, autoPlayer.xAxis] = ' ';
            }
            //Checks if the paddle reached the bottom edge.
            else if (gameField[autoPlayer.yAxis + 5, autoPlayer.xAxis] != GameManager.topBottomEdge)
            {
                isReachTop = true;
                autoPlayer.yAxis++;
                gameField[autoPlayer.yAxis - 1, autoPlayer.xAxis] = ' ';
            }
            else
            {
                isReachTop = false;
            }
            //Sets the paddle to his new location.
            autoPlayer.SetsTheAutoPlayerPosition(gameField);
        }

        private bool IsCollidedWithAnObject(ref bool isGoal, char temp, ref int xDiraction, ref int yDiraction, ref bool isManual, ref bool isOtherPlayer)
        {
            bool isNothing = false;

            //Checks if the ball collided with a paddle.
            if (temp == GameManager.playerIcon)
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
            else if (temp == GameManager.topBottomEdge)
            {
                yDiraction *= (-1);
            }

            //Checks if the ball collided with the left/right edge - if so, checks which side of the edge the ball collided with -> Saves the result -> returns that a goal has occurred.
            else if (temp == GameManager.leftRightEdge)
            {
                if (ball.xAxis >= 89)
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
            int autoTemp = 0, firstPlayerTemp = firstPlayer.yAxis, secondPlayerTemp = 0;

            if (isMultiPlayers)
            {
                secondPlayerTemp = secondPlayer.yAxis;
            }
            else
            {
                autoTemp = autoPlayer.yAxis;
            }

            for (int i = 0; i < 5; i++)
            {
                //Checks which part of the paddle collided with the ball - if so, saves that part.
                //First player and second player paddles.
                if (isMultiPlayers)
                {
                    if (gameField[firstPlayerTemp, firstPlayer.xAxis] == gameField[ball.yAxis, ball.xAxis] || (gameField[secondPlayerTemp, secondPlayer.xAxis] == gameField[ball.yAxis, ball.xAxis]))
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
                    if (gameField[firstPlayerTemp, firstPlayer.xAxis] == gameField[ball.yAxis, ball.xAxis] || (gameField[autoTemp, autoPlayer.xAxis] == gameField[ball.yAxis, ball.xAxis]))
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
            {
                collidedWithBall = CollidedWithBall.UpperEdge;
            }
            else if (i == 2)
            {
                collidedWithBall = CollidedWithBall.MiddleEdge;
            }
            else
            {
                collidedWithBall = CollidedWithBall.BottomEdge;
            }
        }

        //Sets back the last icon that the ball has deleted.
        private void KeepsTheWantedIcon(char temp)
        {

            if (temp == GameManager.playerIcon)
            {
                gameField[ball.yAxis, ball.xAxis] = temp;
                return;
            }
            else if (temp == GameManager.topBottomEdge)
            {
                gameField[ball.yAxis, ball.xAxis] = temp;
                return;
            }
            else
            {
                gameField[ball.yAxis, ball.xAxis] = emptyPixel;
            }
        }

        //Location represents the left side (manual-player) and the right side (auto-player) of the print.
        private void PrintsTheCurrentScore(int currentScore, int location)
        {
            scoreboard.GetsTheScore(currentScore, location);
        }

        //Creates a switch statement in a Do-While loop to set the isRestarting boolean.
        private void IsGameRestart()
        {
            GameStatus gameStatus = GameStatus.None;
            Console.CursorVisible = true;

            SwitchingToGameStatus(GetsTheUserOption(), ref gameStatus);

            switch (gameStatus)
            {
                case GameStatus.Restart:
                    Console.Clear();
                    Start();
                    break;
                case GameStatus.End:
                    Console.SetCursorPosition(28, 10);
                    Console.WriteLine("Thank you for playing. Goodbye.");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
            }
        }

        private int GetsTheUserOption()
        {
            int oneOrTwo;
            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Blue;
                userInterface.PrintsTheTitle();
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
            {
                horizontalOrVertical = -1;
            }
            else if (horizontalOrVertical == 1)
            {
                horizontalOrVertical = 0;
            }
            else
            {
                horizontalOrVertical = 1;
            }
            return horizontalOrVertical;
        }

        private void BallValueReset()
        {
            gameField[ball.yAxis, ball.xAxis] = ' ';
            //The next 3 lines resets the ball's coordinates.
            ball.yAxis = (fieldHight / 2);
            ball.xAxis = (fieldWidth / 2);
            ball.SetsTheBallPosition(gameField);
        }

        private void AutoPlayerValueReset(ref bool isReachTop)
        {
            //The next 2 lines resets the auto-player's coordinates.
            isReachTop = false;
            autoPlayer.ClearTheColumn(gameField);
        }
    }
}
