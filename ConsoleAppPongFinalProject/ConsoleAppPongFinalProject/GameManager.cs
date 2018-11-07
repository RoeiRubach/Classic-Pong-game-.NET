﻿using System;
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
        private char[,] gameField;
        private const int fieldHight = 23;
        private const int fieldWidth = 90;
        private const int gameOver = 5;
        private bool isGameOver = false;
        private int manualGoalCount, autoGoalCount;

        //The next 7 lines initiates the objects.
        UserInterface userInterface = new UserInterface();
        Scoreboard scoreboard = new Scoreboard();
        Board board = new Board();
        Ball ball = new Ball((fieldWidth / 2), (fieldHight / 2));
        AutoPlayer autoPlayer = new AutoPlayer((fieldWidth - 3), ((fieldHight / 2) - 8));
        ManualPlayer manualPlayer = new ManualPlayer(2, ((fieldHight / 2) - 2));
        Highscore highscore = new Highscore();

        public GameManager()
        {
            gameField = new char[fieldHight, fieldWidth];
            board.SetsTheBoard(gameField);
            ball.SetsTheBallPosition(gameField);
            autoPlayer.SetsTheAutoPlayerPosition(gameField);
            manualPlayer.SetsTheManualPlayerPosition(gameField);
        }

        //Controls the game logic and runs it.
        public void Start()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = ConsoleColor.White;
            manualGoalCount = 0;
            autoGoalCount = 0;
            int userChoice = 0;
            userInterface.MainMenu(ref userChoice);

            switch (userChoice)
            {
                case 1:
                    Thread myWorker = new Thread(ThreadFunctionForTheBall_AutoPlayer);
                    myWorker.Start();
                    break;
                case 2:
                    Console.WriteLine("Coming soon.");
                    Environment.Exit(0);
                    break;
            }

            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        //Checks if the ManualPlayer gets out the border.
                        if (gameField[manualPlayer.yAxis - 1, manualPlayer.xAxis] != GameManager.topBottomEdge)
                        {
                            manualPlayer.yAxis--;
                            gameField[manualPlayer.yAxis + 5, manualPlayer.xAxis] = ' ';
                        }
                        manualPlayer.SetsTheManualPlayerPosition(gameField);
                        break;
                    case ConsoleKey.DownArrow:
                        //Checks if the ManualPlayer gets out the border.
                        if (gameField[manualPlayer.yAxis + 5, manualPlayer.xAxis] != GameManager.topBottomEdge)
                        {
                            manualPlayer.yAxis++;
                            gameField[manualPlayer.yAxis - 1, manualPlayer.xAxis] = ' ';
                        }
                        manualPlayer.SetsTheManualPlayerPosition(gameField);
                        break;
                    default:
                        //Checks if the game has ended.
                        if ((manualGoalCount == gameOver) || (autoGoalCount == gameOver))
                        {
                            Console.SetCursorPosition(0, 28);
                            Console.WriteLine("Press any key to continue..");
                            Console.ReadKey(true);
                        }
                        break;
                }

            } while (!isGameOver);

            IsGameRestart();
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
            bool isGoal = false, isReachTop = false, isManual = false, isAuto = false;
            isGameOver = false;
            char temp = ' ';
            int ballXDiraction = 1, ballYDiraction = 0;
            //First score for the manual player.
            scoreboard.PrintsZero(0);
            //First score for the auto player.
            scoreboard.PrintsZero(83);

            do
            {
                if (isGoal)
                {
                    if (isManual)
                    {
                        int location = 0;
                        manualGoalCount++;
                        PrintsTheCurrentScore(manualGoalCount, location);
                    }
                    else
                    {
                        int location = 83;
                        autoGoalCount++;
                        PrintsTheCurrentScore(autoGoalCount, location);
                    }

                    gameField[ball.yAxis, ball.xAxis] = temp;
                    temp = ' ';

                    //The next 5 lines spawns the ball at inconsistently coordinates.
                    ball.yAxis = (RandomNumer() + RandomNumer() + RandomNumer() + RandomNumer() + (fieldHight / 2));
                    ball.xAxis = (2 + RandomNumer() + (fieldWidth / 2));
                    ballYDiraction = RandomNumer();
                    ballXDiraction = -1;
                    ball.SetsTheBallPosition(gameField);
                    //The next 4 lines sets the auto-player's coordinates at the middle field.
                    autoPlayer.ClearTheColumn(gameField);
                    autoPlayer.yAxis = ((fieldHight / 2) - 2);
                    autoPlayer.xAxis = (fieldWidth - 3);
                    autoPlayer.SetsTheAutoPlayerPosition(gameField);

                    isGoal = false;
                    Thread.Sleep(1300);

                    if (manualGoalCount == gameOver)
                    {
                        isGameOver = true;
                        Console.SetCursorPosition(37, 15);
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine(userInterface.player1 + " wins!");
                        Console.ForegroundColor = ConsoleColor.White;
                        highscore.HighscoreWriter(userInterface.player1, manualGoalCount, autoGoalCount);
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
                        highscore.HighscoreWriter(userInterface.player1, manualGoalCount, autoGoalCount);
                        break;
                    }
                }
                AutoPlayerMovementLogic(ref isReachTop);
                PrintsTheGameField();
                if (!IsCollidedWithAnObject(ref isGoal, temp, ref ballXDiraction, ref ballYDiraction, ref isManual, ref isAuto))
                {
                    BallMovementLogic(ref temp, ref ballXDiraction, ref ballYDiraction);
                }
            } while (!isGameOver);
            BallValueReset();
            AutoPlayerValueReset(ref isReachTop);
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

        private bool IsCollidedWithAnObject(ref bool isGoal, char temp, ref int xDiraction, ref int yDiraction, ref bool isManual, ref bool isAuto)
        {
            bool isNothing = false;

            //Checks if the ball collided with a paddle.
            if (temp == GameManager.playerIcon)
            {
                //Gets the last auto-player and manual-player -yAxis- values.
                int autoTemp = autoPlayer.yAxis, manualTemp = manualPlayer.yAxis, collidedWithBall = 0;

                for (int i = 0; i < 5; i++)
                {
                    //Checks which part of the paddle collided with the ball - if so, saves that part.
                    if (gameField[manualTemp, manualPlayer.xAxis] == gameField[ball.yAxis, ball.xAxis] || (gameField[autoTemp, autoPlayer.xAxis] == gameField[ball.yAxis, ball.xAxis]))
                    {
                        collidedWithBall = i;
                    }
                    autoTemp++;
                    manualTemp++;
                }
                switch (collidedWithBall)
                {
                    case 0:
                    case 1:
                        yDiraction = -1;
                        break;
                    case 2:
                        yDiraction = 0;
                        break;
                    case 3:
                    case 4:
                        yDiraction = 1;
                        break;
                    default:
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
                    isAuto = false;
                    isManual = true;
                }
                else
                {
                    isAuto = true;
                    isManual = false;
                }
                return isGoal = true;
            }
            return isNothing;
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
                gameField[ball.yAxis, ball.xAxis] = ' ';
            }
        }

        //Location represents the left side (manual-player) and the right side (auto-player) of the print.
        private void PrintsTheCurrentScore(int currentScore, int location)
        {

            scoreboard.ClearTheScore(location);
            switch (currentScore)
            {
                case 1:
                    scoreboard.PrintsOne(location);
                    break;
                case 2:
                    scoreboard.PrintsTwo(location);
                    break;
                case 3:
                    scoreboard.PrintsThree(location);
                    break;
                case 4:
                    scoreboard.PrintsFour(location);
                    break;
                case 5:
                    scoreboard.PrintsFive(location);
                    break;
                default:
                    break;
            }
        }

        //Creates a switch statement in a Do-While loop to set the isRestarting boolean.
        private bool IsGameRestart()
        {
            int yesOrNo;
            bool isRestarting = false;
            Console.CursorVisible = true;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue;

            userInterface.PrintsTheTitle();
            Console.SetCursorPosition(28, 7);
            Console.Write("Will you want to restart the game?");
            Console.SetCursorPosition(26, 8);
            Console.Write("Enter -1- to restart or -2- to exit: ");
            do
            {
                Int32.TryParse(Console.ReadLine(), out yesOrNo);
                switch (yesOrNo)
                {
                    case 1:
                        Console.Clear();
                        Start();
                        isRestarting = true;
                        break;
                    case 2:
                        Console.SetCursorPosition(28, 10);
                        Console.WriteLine("Thank you for playing. Goodbye.");
                        Environment.Exit(0);
                        break;
                    default:
                        IsGameRestart();
                        break;
                }
            } while (!isRestarting);
            return isRestarting;
        }
        
        //A random value for the ball starting position.
        private int RandomNumer()
        {
            int horizontalOrVertical = 0;
            Random rnd = new Random();
            horizontalOrVertical = rnd.Next(0,3);
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
            //The next 5 lines resets the auto-player's coordinates.
            isReachTop = false;
            autoPlayer.ClearTheColumn(gameField);
            autoPlayer.yAxis = ((fieldHight / 2) - 8);
            autoPlayer.xAxis = (fieldWidth - 3);
            autoPlayer.SetsTheAutoPlayerPosition(gameField);
        }
    }
}