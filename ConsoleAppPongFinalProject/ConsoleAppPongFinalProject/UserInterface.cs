using System;

namespace ConsoleAppPongFinalProject
{
    class UserInterface
    {
        private const int LEFT_SET_CURSOR = 28;
        private int _leftForCursorIcon = 22, _topForCursorIcon;
        public string PlayerOne { get; set; }
        public string PlayerTwo { get; set; }

        private Instructions _instructions = new Instructions();
        private Highscore _highscore = new Highscore();

        /// <summary>
        /// A method that controls the UI -Main Menu- and returns the user choice.
        /// Creates a Do-While loop with an inside switch statement that gets keys input.
        /// 3 options - A, playing aginst the computer. B, playing aginst another player. C, view the high score.
        /// returns the user choice as an integer.
        /// </summary>
        /// <param name="userChoice"></param>
        /// <returns></returns>
        public void MainMenu(ref UserChoice userChoice)
        {
            _topForCursorIcon = 3;
            bool isPressed = false;
            Console.Clear();

            do
            {
                PrintPlayerOne();
                PrintPlayerTwo();
                PrintHighscore();
                Console.ForegroundColor = ConsoleColor.Blue;
                PrintPongTitle();
                Console.ForegroundColor = ConsoleColor.White;
                MainMenuInstructions();

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    //Controls the cursor location.
                    case ConsoleKey.UpArrow:
                        if (_topForCursorIcon > 9)
                        {
                            _topForCursorIcon -= 6;
                            CursorLook(ref _leftForCursorIcon, ref _topForCursorIcon);
                        }
                        break;
                    //Controls the cursor location.
                    case ConsoleKey.DownArrow:
                        if (_topForCursorIcon < 21)
                        {
                            _topForCursorIcon += 6;
                            CursorLook(ref _leftForCursorIcon, ref _topForCursorIcon);
                        }
                        break;
                    //Controls the user chooice.
                    case ConsoleKey.Enter:
                        switch (_topForCursorIcon)
                        {
                            //User has chosen to play the 1Player.
                            case 9:
                                _instructions.ClearBoard();
                                PlayerOne = _instructions.SetPlayerName(1);
                                _instructions.PrintPlayerOneInstructions(PlayerOne);
                                PrintPressToStart();
                                userChoice = UserChoice.SinglePlayer;
                                isPressed = true;
                                break;
                            //User has chosen to play the 2Players.
                            case 15:
                                _instructions.ClearBoard();
                                PlayerOne = _instructions.SetPlayerName(1);
                                PlayerTwo = _instructions.SetPlayerName(2);
                                _instructions.PrintPlayerTwoInsructions(PlayerOne, PlayerTwo);
                                PrintPressToStart();
                                userChoice = UserChoice.MultiPlayers;
                                isPressed = true;
                                break;
                            //User has chosen to view the high score.
                            case 21:
                                _highscore.PrintsHighscoreAsTitle();
                                _highscore.HighscoreReader();
                                Console.ReadKey(true);
                                MainMenu(ref userChoice);
                                isPressed = true;
                                break;
                            default:
                                break;
                        }
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                }

            } while (!isPressed);
        }

        private void CursorLook(ref int leftForCursor, ref int topForCursor)
        {
            //9 = Next to 1Player. 15 = Next to 2Players. 21 = Next to Highscore.
            //Clears the -Yaxis- of the cursor so it won't get copied.
            for (int i = 9; i < 23; i+=6)
            {
                Console.SetCursorPosition(22,i);
                Console.Write("\t  ");
            }
            Console.SetCursorPosition(leftForCursor, topForCursor);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(GameManager.CURSOR_ICON);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void PrintPlayerOne()
        {
            Console.SetCursorPosition(LEFT_SET_CURSOR, 7);
            Console.Write("  _   ___ _                   ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 8);
            Console.Write(" / | | _ \\ |__ _ _  _ ___ _ _ ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 9);
            Console.Write(" | | |  _/ / _` | || / -_) '_|");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 10);
            Console.Write(" |_| |_| |_\\__,_|\\_, \\___|_|  ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 11);
            Console.WriteLine("                 |__/         ");
        }

        private void PrintPlayerTwo()
        {
            Console.SetCursorPosition(LEFT_SET_CURSOR, 13);
            Console.Write("  ___   ___ _                   ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 14);
            Console.Write(" |_  ) | _ \\ |__ _ _  _ ___ _ _ ___");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 15);
            Console.Write("  / /  |  _/ / _` | || / -_) '_(_-<");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 16);
            Console.Write(" /___| |_| |_\\__,_|\\_, \\___|_| /__/");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 17);
            Console.WriteLine("                   |__/         ");
        }

        private void PrintHighscore()
        {
            Console.SetCursorPosition(LEFT_SET_CURSOR, 19);
            Console.Write("  _  _ _      _                       ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 20);
            Console.Write(" | || (_)__ _| |_  ___ __ ___ _ _ ___ ");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 21);
            Console.Write(" | __ | / _` | ' \\(_-</ _/ _ \\ '_/ -_)");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 22);
            Console.Write(" |_||_|_\\__, |_||_/__/\\__\\___/_| \\___|");
            Console.SetCursorPosition(LEFT_SET_CURSOR, 23);
            Console.WriteLine("        |___/                         ");
        }

        public void PrintPongTitle()
        {
            Console.SetCursorPosition(33, 0);
            Console.Write("  ___  ___  _  _  ___ ");
            Console.SetCursorPosition(33, 1);
            Console.Write(" | _ \\/ _ \\| \\| |/ __|");
            Console.SetCursorPosition(33, 2);
            Console.Write(" |  _/ (_) | .` | (_ |");
            Console.SetCursorPosition(33, 3);
            Console.WriteLine(" |_|  \\___/|_|\\_|\\___|");
        }

        private void PrintPressToStart()
        {
            Console.SetCursorPosition(0, 20);
            Console.WriteLine("Press anything to -start- and good luck!");
            Console.ReadLine();
        }

        private void MainMenuInstructions()
        {
            Console.SetCursorPosition(0, 27);
            Console.WriteLine("Use the -UpArrow- and the -DownArrow- keys to navigate or the -Escape- key to exit.");
            Console.SetCursorPosition(0, 28);
            Console.WriteLine("Use the -Enter- key to choose.");
        }
    }
}