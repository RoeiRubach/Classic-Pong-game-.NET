using System;

namespace ConsoleAppPongFinalProject
{
    public class GameUI
    {
        public string PlayerOne;
        public string PlayerTwo;

        private Instructions _instructions = new Instructions();
        private Highscore _highscore = new Highscore();
        private int _leftForCursorIcon = 22;
        private int _topForCursorIcon;

        public GameUI()
        {
            GameManager.GameOver += DisplayWinnerOnGameOver;
        }

        private void DisplayWinnerOnGameOver()
        {
            GameManager.GameOver -= DisplayWinnerOnGameOver;

            switch (GameManager.UserChoice)
            {
                case UserChoice.SinglePlayer:

                    Console.SetCursorPosition(37, 15);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(PlayerOne + " wins!");
                    Console.ForegroundColor = ConsoleColor.White;


                    Console.SetCursorPosition(35, 15);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("Computer wins!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.SetCursorPosition(30, 16);
                    Console.WriteLine(PlayerOne + ", good luck next time.");
                    break;

                case UserChoice.PlayerVSPlayer:

                    Console.SetCursorPosition(37, 15);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(PlayerOne + " wins!");
                    Console.ForegroundColor = ConsoleColor.White;


                    Console.SetCursorPosition(35, 15);
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(PlayerTwo + " wins!");
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }

        /// <summary>
        /// A method that controls the UI -Main Menu- and returns the user choice.
        /// Creates a Do-While loop with an inside switch statement that gets keys input.
        /// 3 options - A, playing aginst the computer. B, playing aginst another player. C, view the high score.
        /// returns the user choice as an integer.
        /// </summary>
        /// <param name="userChoice"></param>
        /// <returns></returns>
        public void MainMenu()
        {
            _topForCursorIcon = 3;
            bool hasChoosen = false;
            Console.Clear();

            do
            {
                UserInterfaceUtilities.PrintMainMenuTitles();

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (_topForCursorIcon > 9)
                        {
                            _topForCursorIcon -= 6;
                            DrawCursor();
                        }
                        break;

                    case ConsoleKey.DownArrow:
                        if (_topForCursorIcon < 21)
                        {
                            _topForCursorIcon += 6;
                            DrawCursor();
                        }
                        break;

                    case ConsoleKey.Enter:
                        switch (_topForCursorIcon)
                        {
                            case (int)UserOptions.SoloPlayer:
                                _instructions.ClearBoard();
                                PlayerOne = _instructions.SetPlayerName(1);
                                _instructions.PrintPlayerOneInstructions(PlayerOne);
                                UserInterfaceUtilities.PrintPressToStart();
                                GameManager.UserChoice = UserChoice.SinglePlayer;
                                break;

                            case (int)UserOptions.PvP:
                                _instructions.ClearBoard();
                                PlayerOne = _instructions.SetPlayerName(1);
                                PlayerTwo = _instructions.SetPlayerName(2);
                                _instructions.PrintPlayerTwoInstructions(PlayerOne, PlayerTwo);
                                UserInterfaceUtilities.PrintPressToStart();
                                GameManager.UserChoice = UserChoice.PlayerVSPlayer;
                                break;

                            case (int)UserOptions.Highscore:
                                UserInterfaceUtilities.PrintHighscoreAsTitle();
                                _highscore.HighscoreReader();
                                Console.ReadKey(true);
                                MainMenu();
                                break;
                        }
                        hasChoosen = true;
                        break;

                    case ConsoleKey.Escape:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                }
            } while (!hasChoosen);
        }

        private void DrawCursor()
        {
            ClearCopiedCursors();

            Console.SetCursorPosition(_leftForCursorIcon, _topForCursorIcon);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(CharacterUtilities.CURSOR_ICON);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ClearCopiedCursors()
        {
            for (int i = 9; i < 23; i += 6)
            {
                Console.SetCursorPosition(22, i);
                Console.Write("\t  ");
            }
        }
    }
}