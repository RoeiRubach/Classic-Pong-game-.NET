using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPongFinalProject
{
    class UserInterface
    {
        private const int leftForSetCursor = 28;
        private int leftForCursorIcon = 22, topForCursorIcon;
        public string player1 { get; set; }
        public string player2 { get; set; }

        Instructions instructions = new Instructions();
        Highscore highscore = new Highscore();

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
            topForCursorIcon = 3;
            bool isPressed = false;
            Console.Clear();

            do
            {
                Prints1Player();
                Prints2Players();
                PrintsHighScores();
                Console.ForegroundColor = ConsoleColor.Blue;
                PrintsTheTitle();
                Console.ForegroundColor = ConsoleColor.White;
                MainMenuInstructions();

                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    //Controls the cursor location.
                    case ConsoleKey.UpArrow:
                        if (topForCursorIcon > 9)
                        {
                            topForCursorIcon -= 6;
                            CursorLook(ref leftForCursorIcon, ref topForCursorIcon);
                        }
                        break;
                    //Controls the cursor location.
                    case ConsoleKey.DownArrow:
                        if (topForCursorIcon < 21)
                        {
                            topForCursorIcon += 6;
                            CursorLook(ref leftForCursorIcon, ref topForCursorIcon);
                        }
                        break;
                    //Controls the user chooice.
                    case ConsoleKey.Enter:
                        switch (topForCursorIcon)
                        {
                            //User has chosen to play the 1Player.
                            case 9:
                                instructions.ClearsTheBoard();
                                player1 = instructions.SetsPlayerName(1);
                                instructions.Prints1PlayerInstructions(player1);
                                PrintsPressToStart();
                                userChoice = UserChoice.SinglePlayer;
                                isPressed = true;
                                break;
                            //User has chosen to play the 2Players.
                            case 15:
                                instructions.ClearsTheBoard();
                                player1 = instructions.SetsPlayerName(1);
                                player2 = instructions.SetsPlayerName(2);
                                instructions.Prints2PlayersInstructions(player1, player2);
                                PrintsPressToStart();
                                userChoice = UserChoice.MultiPlayer;
                                isPressed = true;
                                break;
                            //User has chosen to view the high score.
                            case 21:
                                highscore.PrintsHighscoreAsTitle();
                                highscore.HighscoreReader();
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
            Console.Write(GameManager.CursorIcon);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void Prints1Player()
        {
            Console.SetCursorPosition(leftForSetCursor, 7);
            Console.Write("  _   ___ _                   ");
            Console.SetCursorPosition(leftForSetCursor, 8);
            Console.Write(" / | | _ \\ |__ _ _  _ ___ _ _ ");
            Console.SetCursorPosition(leftForSetCursor, 9);
            Console.Write(" | | |  _/ / _` | || / -_) '_|");
            Console.SetCursorPosition(leftForSetCursor, 10);
            Console.Write(" |_| |_| |_\\__,_|\\_, \\___|_|  ");
            Console.SetCursorPosition(leftForSetCursor, 11);
            Console.WriteLine("                 |__/         ");
        }

        private void Prints2Players()
        {
            Console.SetCursorPosition(leftForSetCursor, 13);
            Console.Write("  ___   ___ _                   ");
            Console.SetCursorPosition(leftForSetCursor, 14);
            Console.Write(" |_  ) | _ \\ |__ _ _  _ ___ _ _ ___");
            Console.SetCursorPosition(leftForSetCursor, 15);
            Console.Write("  / /  |  _/ / _` | || / -_) '_(_-<");
            Console.SetCursorPosition(leftForSetCursor, 16);
            Console.Write(" /___| |_| |_\\__,_|\\_, \\___|_| /__/");
            Console.SetCursorPosition(leftForSetCursor, 17);
            Console.WriteLine("                   |__/         ");
        }

        private void PrintsHighScores()
        {
            Console.SetCursorPosition(leftForSetCursor, 19);
            Console.Write("  _  _ _      _                       ");
            Console.SetCursorPosition(leftForSetCursor, 20);
            Console.Write(" | || (_)__ _| |_  ___ __ ___ _ _ ___ ");
            Console.SetCursorPosition(leftForSetCursor, 21);
            Console.Write(" | __ | / _` | ' \\(_-</ _/ _ \\ '_/ -_)");
            Console.SetCursorPosition(leftForSetCursor, 22);
            Console.Write(" |_||_|_\\__, |_||_/__/\\__\\___/_| \\___|");
            Console.SetCursorPosition(leftForSetCursor, 23);
            Console.WriteLine("        |___/                         ");
        }

        public void PrintsTheTitle()
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

        private void PrintsPressToStart()
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
