using System;

namespace ConsoleAppPongFinalProject
{
    class GameUI
    {
        public string PlayerOne;
        public string PlayerTwo;

        private Instructions _instructions = new Instructions();
        private Highscore _highscore = new Highscore();
        private int _leftForCursorIcon = 22, _topForCursorIcon;

        /// <summary>
        /// A method that controls the UI -Main Menu- and returns the user choice.
        /// Creates a Do-While loop with an inside switch statement that gets keys input.
        /// 3 options - A, playing aginst the computer. B, playing aginst another player. C, view the high score.
        /// returns the user choice as an integer.
        /// </summary>
        /// <param name="userChoice"></param>
        /// <returns></returns>
        public void SetUserChoice()
        {
            _topForCursorIcon = 3;
            bool isPressed = false;
            Console.Clear();

            do
            {
                UserInterfaceUtilities.PrintPongTitle();
                UserInterfaceUtilities.PrintTitles();

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
                                PlayerOne = SetPlayerName(1);
                                _instructions.PrintSoloPlayerInstructions(PlayerOne);
                                UserInterfaceUtilities.PrintPressToStart();
                                GameManager.UserChoice = UserChoice.SinglePlayer;
                                break;

                            case (int)UserOptions.PvP:
                                PlayerOne = SetPlayerName(1);
                                PlayerTwo = SetPlayerName(2);
                                _instructions.PrintPVPInstructions(PlayerOne, PlayerTwo);
                                UserInterfaceUtilities.PrintPressToStart();
                                GameManager.UserChoice = UserChoice.PlayerVSPlayer;
                                break;

                            case (int)UserOptions.Highscore:
                                UserInterfaceUtilities.PrintHighscoreAsTitle();
                                _highscore.HighscoreReader();
                                Console.ReadKey(true);
                                SetUserChoice();
                                break;
                        }
                        isPressed = true;
                        break;

                    case ConsoleKey.Escape:
                        Console.Clear();
                        Environment.Exit(0);
                        break;
                }
            } while (!isPressed);
        }

        private string SetPlayerName(int whichPlayer)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            DisplayNameWriting(whichPlayer);
            string playerName = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            return playerName;
        }

        private void DisplayNameWriting(int whichPlayer)
        {
            Console.SetCursorPosition(3, 7);
            switch (whichPlayer)
            {
                case 1:
                    Console.Write("Enter -first player- name: ");
                    break;
                case 2:
                    Console.Write("Enter -second player- name: ");
                    break;
            }
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